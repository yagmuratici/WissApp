using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WissAppWebApi.Models
{
    public class UsersModel
    {
        public int Id { get; set; }

        public int RoleId { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string EMail { get; set; }

        public string School { get; set; }

        public string Location { get; set; }

        public DateTime? BirthDate { get; set; }

        public string Gender { get; set; }

        public bool Active { get; set; }

    }
}