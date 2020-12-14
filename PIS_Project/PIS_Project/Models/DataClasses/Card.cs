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
        /// <summary>
        /// Перечисление для типа животного
        /// </summary>
        /// Значения битовой маски:
        /// 1|0-Собака|Кошка;
        /// 01|10|11-Мальнькая|Средняя|Большая;
        /// 1|0-Длинношёрстная|Короткошёрстная;
        /// 0|1-Волос прямой|Волос кудрявый
        public enum AnimalType : int
        {
            DogSmallShortHairedWireHaired = 0b_0001_0101,
            DogSmallLongHairedWireHaired = 0b_0001_0111,
            DogLargeShortHairedWireHaired = 0b_0001_1101,
            DogLargeLongHairedWireHaired = 0b_0001_1111,
            DogMediumShortHairedWireHaired = 0b_0001_1001,
            DogMediumLongHairedWireHaired = 0b_0001_1011,
            CatSmallShortHairedWireHaired = 0b_0000_0101,
            CatSmallLongHairedWireHaired=0b_0000_0111,
            CatLargeShortHairedWireHaired=0b_0000_1101,
            CatLargeLongHairedWireHaired=0b_0000_1111,
            CatMediumShortHairedWireHaired=0b_0000_1001,
            CatMediumLongHairedWireHaired=0b_0000_1011,
            DogSmallShortHairedCurly=0b_0001_0100,
            DogSmallLongHairedCurly=0b_0001_0110,
            DogLargeShortHairedCurly=0b_0001_1100,
            DogLargeLongHairedCurly=0b_0001_1110,
            DogMediumShortHairedCurly=0b_0001_1000,
            DogMediumLongHairedCurly=0b_0001_1010,
            CatSmallShortHairedCurly=0b_0000_0100,
            CatSmallLongHairedCurly=0b_0000_0110,
            CatLargeShortHairedCurly=0b_0000_1100,
            CatLargeLongHairedCurly=0b_0000_1110,
            CatMediumShortHairedCurly=0b_0001_1000,
            CatMediumLongHairedCurly=0b_0000_1010
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("Регистрационный номер карточки учета")]
        public int ID { get; set; }
        [DisplayName("Пол животного")]
        public SexAnimal sex { get; set; }
        public AnimalType type { get; set; }
        [NotMapped]
        public string Type
        {
            get
            {
                var result = "";
                var endig = sex == SexAnimal.Male ? "ый" : "ая";
                var mask = new int[]
                {
                    (((int)type) & (1 << 4))!=0?1:0, //Вид
                    ((((((int)type) & (1 << 3))!=0?1:0)*10)+((((int)type) & (1 << 2)))!=0?1:0), //Размер
                    ((((int)type) & (1 << 1))!=0?1:0),//Длина шерсти
                    ((((int)type) & (1 << 0))!=0?1:0)//Тип шерсти
                };
                if (mask[0] == 1)
                {
                    if (sex == SexAnimal.Male)
                    {
                        result += "кобель";
                    }
                    else
                        result += "сука";
                }
                else
                {
                    if (sex == SexAnimal.Male)
                    {
                        result += "кот";
                    }
                    else
                        result += "кошка";
                }
                result += ", размер";
                switch (mask[1])
                {
                    case 11:
                        result += " больщой";
                        break;
                    case 10:
                        result += " средний";
                        break;
                    case 1:
                        result += " маленький";
                        break;
                }
                switch (mask[2]) 
                {
                    case 1:
                        result = $"длинношёрстн{endig} " +result;
                        break;
                    case 0:
                        result = $"короткошёрстн{endig} "+result;
                        break;
                }
                result += ", шерсть ";
                switch (mask[3])
                {
                    case 1:
                        result += "прямая";
                        break;
                    case 0:
                        result += "волнистая";
                        break;
                }
                if (sex == SexAnimal.Germafrodit)
                    result = "У неё член! Ну на этом наши полномочия всё...";
                return result;
            }
        }
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