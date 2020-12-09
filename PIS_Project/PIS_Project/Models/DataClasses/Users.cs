using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PIS_Project.Models.DataClasses
{
    public class Users : ApplicationUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [NotMapped]
        public override string Id { get; set; }

        public int ID { get { return int.Parse(Id); } set { Id = value.ToString(); } }
        public string FIO { get { return UserName; } set { UserName = value; } }
        public string email { get { return Email; } set { Email = value; } }
        public string phone { get; set; }
        public int ID_organization { get; set; }
        public string Organization { get; set; }
        public Guid SIN { get; set; }
        public string password { get; set; }
        public bool Confirmed { get; set; }
        public byte[] Doc { get; set; }
        public int ID_role { get; set; }
        public string Role { get; set; }
    }
}