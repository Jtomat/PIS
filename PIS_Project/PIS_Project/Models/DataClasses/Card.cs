using PIS_Project.Controllers.DataControllers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
            Мужской = 1,
            Женский = 2,
            Гермафродит = 3,
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("Регистрационный номер карточки учета")]
        public int ID { get; set; }
        [DisplayName("Пол животного")]
        public SexAnimal sex { get; set; }
        [DisplayName("Категория животного")]
        public int type { get; set; }
        [DisplayName("Дата рождения")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime birthday { get; set; }
        [DisplayName("Идентификационная метка")]
        public int id_mark { get; set; }
        [DisplayName("Номер электронного чипа")]
        public int id_chip { get; set; }
        [DisplayName("Кличка")]
        public string name { get; set; }
        [DisplayName("Фото")]
        public byte[] photo { get; set; }
        [DisplayName("Особые приметы")]
        public string spec_mark { get; set; }
        [DisplayName("Наличие признаков владельца")]
        public string[] owner_traits { get; set; }
        [DisplayName("Текущий статус животного в реестре")]
        public int id_status { get; set; }
        [DisplayName("Дата установки статуса")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime date_status_change { get; set; }
        [DisplayName("Номер муниципального образование")]
        public int ID_MU { get; set; }
        [DisplayName("Населенный пункт")]
        public string local_place { get; set; }
        [DisplayName("Документ")]
        public byte[] document { get; set; }
        [DisplayName("Скан-образ")]
        public byte[] scan_frame { get; set; }
        [DisplayName("Дата стерилизации")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime sterilization_date { get; set; }
        [NotMapped]
        public string Status
        {
            get;// { return (new CardRegister()).GetStatusByID(id_status).Name; }
            set;
        }
        [NotMapped]
        public string MU
        {
            get;// { return (new CardRegister()).GetMUByID(ID_MU).Name; }
            set;
        }
    }
}