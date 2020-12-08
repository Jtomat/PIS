using PIS_Project.Controllers.DataControllers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PIS_Project.Models.DataClasses
{
    public class Card
    {
        [Flags()]
        public enum SexAnimal : int
        {
            Male = 1,
            Female = 2,
            Germafrodit = Male | Female
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public SexAnimal sex { get; set; }
        public int type { get; set; }
        public DateTime birthday { get; set; }
        public int id_mark { get; set; }
        public int id_chip { get; set; }
        public string name { get; set; }
        public byte[] photo { get; set; }
        public string spec_mark { get; set; }
        public string owner_traits { get; set; }
        public int id_status { get; set; }
        public DateTime date_status_change { get; set; }
        public int ID_MU { get; set; }
        public string local_place { get; set; }
        public byte[] document { get; set; }
        public byte[] scan_frame { get; set; }
        public DateTime sterilization_date { get; set; }

        public string Status
        {
            get;// { return (new CardRegister()).GetStatusByID(id_status).Name; }
            set;
        }
        public string MU
        {
            get;// { return (new CardRegister()).GetMUByID(ID_MU).Name; }
            set;
        }
    }
}