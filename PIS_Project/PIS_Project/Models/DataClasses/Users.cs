using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace PIS_Project.Models.DataClasses
{
    public class Users:IIdentity //: ApplicationUser
    {

       // [NotMapped]
        //public override string Id { get; set; }
       // [NotMapped]
       // public override string UserName { get; set; }
       // [NotMapped]
       // public override string Email { get; set; }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; } //{ return int.Parse(Id); } set { Id = value.ToString(); } }
        public string FIO { get; set; } //{ return UserName; } set { UserName = value; } }
        public string email { get; set; } //{ return Email; } set { Email = value; } }
        public string phone { get; set; }
        public int ID_organization { get; set; }
        [NotMapped]
        public string Organization { get; set; }
        public Guid SIN { get; set; }
        public string password { get; set; }
        public bool Confirmed { get; set; }
        public byte[] Doc { get; set; }
        public int ID_role { get; set; }
        [NotMapped]
        public string Role { get; set; }
        [NotMapped]
        public string Name => FIO;
        [NotMapped]
        public string AuthenticationType => IsAuthenticated?"В сети":"Не в сети";
        [NotMapped]
        public bool IsAuthenticated { get; set; }
    }
}