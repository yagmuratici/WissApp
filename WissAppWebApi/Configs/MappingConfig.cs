using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WissAppEntities.Entities;
using WissAppWebApi.Models;

namespace WissAppWebApi
{
    public class MappingConfig //entity i model e map edecek automapper bu yüzden ayarları burada yapıyoruz
    {
        public static readonly MapperConfiguration mapperConfiguration;

        static MappingConfig()
        {
            mapperConfiguration = new MapperConfiguration( c =>
            {
                c.AddProfile<UsersProfile>();
            });
        }
    }

    public class UsersProfile : Profile
    {
        public UsersProfile() //create map i ctor da kullanıyoruz.
        {
            CreateMap<Users, UsersModel>().ReverseMap(); //reverseMap her iki yönlüde çalışması için hem user ı users model e çeviriyor hemde tam tersi.
        }
    }

}