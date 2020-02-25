using AppCore.Services;
using AppCore.Services.Base;
using AutoMapper.QueryableExtensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;
using WissAppEF.Contexts;
using WissAppEntities.Entities;
using WissAppWebApi.Attributes;
using WissAppWebApi.Configs;
using WissAppWebApi.Models;

namespace WissAppWebApi.Controllers
{
    [RoutePrefix("api/Users")]
    [ClaimsAuthorize(ClaimType = "role", ClaimValue = "admin")] //user işlemlerini sadece admin yapabilir.
    public class UsersController : ApiController
    {
        DbContext db;
        ServiceBase<Users> userService;
        public UsersController()
        {
            db = new WissAppContext();
            userService = new Service<Users>(db);
        }

       
        public IHttpActionResult Get()
        {
            try
            {
                var entities = userService.GetEntities(); //etity çektik

                //1. yol ama 2.yol olarak automapper kullanacağız
                //var model = entities.Select(e => new UsersModel()
                //{
                //    Id = e.Id,
                //    RoleId = e.RoleId,
                //    UserName = e.UserName,
                //    Password = e.Password,
                //    EMail = e.EMail,
                //    School = e.School,
                //    Location = e.Location,
                //    BirthDate = e.BirthDate,
                //    Gender = e.Gender,
                //    Active = e.Active
                //}).ToList();

                      //2.yol
                //var model = Mapping.mapper.Map<List<Users>, List<UsersModel>>(entities);
                      //3.yol
                //var model = Mapping.mapper.Map<List<UsersModel>>(entities);
                //4. yol ama bu Listeler için
                var model = userService.GetEntityQuery().ProjectTo<UsersModel>(MappingConfig.mapperConfiguration).ToList(); //projectTo Query üzerinden dönüşüm yapıyor ama listeler için 3.yol tek olanlar içinde kullanılabilir
                return Ok(model);
            }
            catch(Exception)
            {
                return BadRequest();
            }
            
        }


       
        public IHttpActionResult Get(int id)
        {
            try
            {
                var entity = userService.GetEntity(id);//tek kayıt alıcam 
                var model = Mapping.mapper.Map<UsersModel>(entity);
                return Ok(model);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        
        public IHttpActionResult Post(UsersModel usersModel)
        {
            try
            {
                var entity = Mapping.mapper.Map<Users>(usersModel);
                userService.AddEntity(entity);
                var model = Mapping.mapper.Map<UsersModel>(entity);//redirectTo yapıyorduk get e mvc de burada böyle yapılmıyor çünkü liste dönersen güvenlik açığı oluyor // bunu böyle yaptığında restful service deniliyor
                return Ok(model);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        
        public IHttpActionResult Put(UsersModel usersModel)
        {
            try
            {
                var entity = userService.GetEntity(usersModel.Id);
                entity.BirthDate = usersModel.BirthDate;
                entity.EMail = usersModel.EMail;
                entity.Gender = usersModel.Gender;
                entity.Active = usersModel.Active;
                entity.Location = usersModel.Location;
                entity.Password = usersModel.Password;
                entity.RoleId = usersModel.RoleId;
                entity.School = usersModel.School;
                entity.UserName = usersModel.UserName;
                userService.UpdateEntity(entity);
                var model = Mapping.mapper.Map<UsersModel>(entity);
                return Ok(model);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        
        public IHttpActionResult Delete(int id)
        {
            try
            {
                var entity = userService.GetEntity(id);
                userService.DeleteEntity(entity);
                var model = Mapping.mapper.Map<UsersModel>(entity);
                return Ok(model); //hangi kaydı sildiğini görmesi için.
            }
            catch (Exception)
            {
                return BadRequest();
            }

        }

        [Route("GetAll")]
        public IHttpActionResult GetAll()
        {
            try
            {
                var entities = userService.GetEntities();
                var resultEntities = JsonConvert.SerializeObject(entities , Formatting.None, new JsonSerializerSettings() {  ReferenceLoopHandling = ReferenceLoopHandling.Ignore});
                return Ok(JsonConvert.DeserializeObject(resultEntities));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [Route("Logout")]
        [HttpGet]
        public IHttpActionResult Logout()
        {
            var principal = RequestContext.Principal as ClaimsPrincipal;
            if(principal.Identity.IsAuthenticated)
            {
                UserConfig.AddLoggedOutUser(principal.FindFirst(e => e.Type == "user").Value); //principal mevcut kullanıcı.
                return Ok("User logged out.");
            }
            return BadRequest("User did not login.");
        }
    }
}
