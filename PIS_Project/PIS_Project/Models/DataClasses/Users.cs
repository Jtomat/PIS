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
        public int ID { get; set; }
        public string FIO { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public int ID_organization { get; set; }
        public Guid SIN { get; set; }
        public string password { get; set; }
        public bool Confirmed { get; set; }
        public byte[] Doc { get; set; }
        public int ID_role { get; set; }
    }
}