using AppCore.Services;
using AppCore.Services.Base;
using AutoMapper.QueryableExtensions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using WissAppEF.Contexts;
using WissAppEntities.Entities;
using WissAppWebApi.Models;

namespace WissAppWebApi.Controllers
{
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
                var entities = userService.GetEntities(); //etitity çektik

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
    }
}
