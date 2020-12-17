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
            Germafrodit = Male | Female,
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
            CatSmallLongHairedWireHaired = 0b_0000_0111,
            CatLargeShortHairedWireHaired = 0b_0000_1101,
            CatLargeLongHairedWireHaired = 0b_0000_1111,
            CatMediumShortHairedWireHaired = 0b_0000_1001,
            CatMediumLongHairedWireHaired = 0b_0000_1011,
            DogSmallShortHairedCurly = 0b_0001_0100,
            DogSmallLongHairedCurly = 0b_0001_0110,
            DogLargeShortHairedCurly = 0b_0001_1100,
            DogLargeLongHairedCurly = 0b_0001_1110,
            DogMediumShortHairedCurly = 0b_0001_1000,
            DogMediumLongHairedCurly = 0b_0001_1010,
            CatSmallShortHairedCurly = 0b_0000_0100,
            CatSmallLongHairedCurly = 0b_0000_0110,
            CatLargeShortHairedCurly = 0b_0000_1100,
            CatLargeLongHairedCurly = 0b_0000_1110,
            CatMediumShortHairedCurly = 0b_0001_1000,
            CatMediumLongHairedCurly = 0b_0000_1010
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Display(Name = "Пол")]
        public SexAnimal sex { get; set; }
        [Display(Name = "Характеристика животного")]
        public AnimalType type { get; set; }
        [NotMapped]
        public string StringAnimalType
        {
            get
            {
                var result = "";
                var endig = sex == SexAnimal.Male ? "ый" : "ая";
                var mask = new int[]
                {
                    (((int)type) & (1 << 4))!=0?1:0, //Вид
                    ((((((int)type) & (1 << 3))!=0?1:0)*10))+(((((int)type) & (1 << 2)))!=0?1:0), //Размер
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
                        result += " большой";
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
                        result = $"длинношёрстн{endig} " + result;
                        break;
                    case 0:
                        result = $"короткошёрстн{endig} " + result;
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
                    result = "Ну на этом наши полномочия всё...";
                return result;
            }
        }
        [NotMapped]
        public Dictionary<string, string> SetAnimalTypeValues
        { get; set; }

        [NotMapped]
        public Dictionary<string, string> GetAnimalTypeValues
        {
            get
            {
                Dictionary<string, string> animal_types = new Dictionary<string, string>();
                var mask = new int[]
                 {
                    (((int)type) & (1 << 4))!=0?1:0, //Вид
                    ((((((int)type) & (1 << 3))!=0?1:0)*10))+(((((int)type) & (1 << 2)))!=0?1:0), //Размер
                    ((((int)type) & (1 << 1))!=0?1:0),//Длина шерсти
                    ((((int)type) & (1 << 0))!=0?1:0)//Тип шерсти
                 };
                switch (mask[0])
                {
                    case 0:
                        animal_types["species"] = "0";
                        break;
                    case 1:
                        animal_types["species"] = "1";
                        break;
                }

                switch (mask[1])
                {
                    case 11:
                        animal_types["size"] = "11";
                        break;
                    case 10:
                        animal_types["size"] = "10";
                        break;
                    case 1:
                        animal_types["size"] = "1";
                        break;
                }

                switch (mask[2])
                {

                    case 1:
                        animal_types["hire_size"] = "1";
                        break;
                    case 0:
                        animal_types["hire_size"] = "0";
                        break;
                }

                switch (mask[3])
                {
                    case 1:
                        animal_types["hire_type"] = "1";
                        break;
                    case 0:
                        animal_types["hire_type"] = "0";
                        break;
                }
                return animal_types;
            }
            set { }
        }
        [Display(Name = "Дата рождения")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime birthday { get; set; }
        [Display(Name = "Идентификационная метка")]
        public int id_mark { get; set; }
        [Display(Name = "Номер чипа")]
        public int id_chip { get; set; }
        [Display(Name = "Кличка")]
        public string name { get; set; }
        [Display(Name = "Фото")]
        public byte[] photo { get; set; }
        [Display(Name = "Приметы")]
        public string spec_mark { get; set; }
        [Display(Name = "Признаки наличия владельца")]
        public string owner_traits { get; set; }


        [NotMapped]
        public Dictionary<string, bool> GetOwnerTraits
        {
            get
            {

                Dictionary<string, bool> checkboxValues = new Dictionary<string, bool>();
                checkboxValues["collar"] = false;
                checkboxValues["clothing"] = false;
                checkboxValues["harness"] = false;
                checkboxValues["chip"] = false;

                if (owner_traits != null)
                {
                    string[] ownerTypeValues = owner_traits.Split(',');

                    foreach (string val in ownerTypeValues)
                    {
                        switch (val.Trim())
                        {
                            case "ошейник":
                                checkboxValues["collar"] = true;
                                break;
                            case "шлейка":
                                checkboxValues["harness"] = true;
                                break;
                            case "чип":
                                checkboxValues["chip"] = true;
                                break;
                            case "одежда":
                                checkboxValues["clothing"] = true;
                                break;
                        }

                    }
                }

                return checkboxValues;
            }
        }

        [NotMapped]
        public Dictionary<string, bool> SetOwnerTraits { get; set; }
        [Display(Name = "Текущий статус")]
        public int id_status { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Дата установки статуса")]
        public DateTime date_status_change { get; set; }
        [Display(Name = "ID Муниципального образования")]
        public int ID_MU { get; set; }
        [Display(Name = "Населенный пункт")]
        public string local_place { get; set; }
        [Display(Name = "Документ")]
        public byte[] document { get; set; }
        [Display(Name = "Скан-образ документа")]
        public byte[] scan_frame { get; set; }
        [Display(Name = "Дата стерелизации")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime sterilization_date { get; set; }

        [NotMapped]
        public string MU { get; set; }

        [NotMapped]
        public string Status { get; set; }
    }
}