using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using WissAppWebApi.Configs;

namespace WissAppWebApi.Attributes
{
    public class ClaimsAuthorizeAttribute : AuthorizationFilterAttribute
    {
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }

        public override Task OnAuthorizationAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            var principal = actionContext.RequestContext.Principal as ClaimsPrincipal;

            if(actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any() || actionContext.ControllerContext.ControllerDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any())
            {
                return Task.FromResult<object>(null);
            }
            if(!principal.Identity.IsAuthenticated)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized); //burada araya giriyoruz.
                return Task.FromResult<object>(null); //method u sonlandırmak için yazılıyor.
            }
            if(UserConfig.GetLoggedOutUsers().Contains(principal.FindFirst( e=> e.Type == "user").Value))
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                return Task.FromResult<object>(null);
            }
            if(!principal.HasClaim(e => e.Type.ToLower().Contains(ClaimType.ToLower()) && ClaimValue.ToLower().Equals(e.Value.ToLower()))) //user ya da role tipinde claim var mı ve admin mi
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized); //burada araya giriyoruz.
                return Task.FromResult<object>(null);
            }
            return Task.FromResult<object>(null);
        }
    }
}