using PIS_Project.Controllers.DataControllers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PIS_Project.CustomValidate;
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
            Female = 2
        }
        /// <summary>
        /// Перечисление для типа животного
        /// </summary>
        /// Значения битовой маски:
        /// 1|0-Собака|Кошка;
        /// 01|10|11-Мальнькая|Средняя|Большая;
        /// 1|0-Длинношёрстная|Короткошёрстная;
        /// 1|0-Волос прямой|Волос кудрявый
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
        [Display(Name = "Характеристика")]
        public AnimalType type { get; set; }
        [NotMapped]
        public string stringAnimalType
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
                return result;
            }
        }
        [NotMapped]
        public Dictionary<string, string> setAnimalTypeValues
        { get; set; }

        [NotMapped]
        public Dictionary<string, string> getAnimalTypeValues
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
        [DateLimit(ErrorMessage = "Дата рождения статуса должна быть меньше или равна текущей")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime birthday { get; set; }
        [Display(Name = "Идентификационная метка")]
        [Range(1, int.MaxValue, ErrorMessage = "Значение должно быть больше 0")]
        public int id_mark { get; set; }
        [Display(Name = "Номер чипа")]
        [Range(0, int.MaxValue, ErrorMessage = "Значение должно быть больше 0")]
        public int id_chip { get; set; }
        [Display(Name = "Кличка")]
        [Required]
        [RegularExpression("([А-ЯA-Z][а-яa-zА-ЯA-Z-' ]+)", ErrorMessage = "Некорректное имя. Должно начинаться с заглавной буквы и не содержать цифры")]
        public string name { get; set; }
        [Display(Name = "Фото")]
        public byte[] photo { get; set; }
        [RegularExpression("([А-ЯA-Z][а-яa-zА-ЯA-Z-',;!. ]+)", ErrorMessage = "Некорректное значение. Приметы не должны содержать цифры")]
        [Display(Name = "Приметы")]
        public string spec_mark { get; set; }
        [Display(Name = "Признаки наличия владельца")]
        public string owner_traits { get; set; }


        [NotMapped]
        public Dictionary<string, bool> getOwnerTraits
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
        public Dictionary<string, bool> setOwnerTraits { get; set; }
        [Display(Name = "Текущий статус")]
        [Range(0, int.MaxValue, ErrorMessage = "Значение должно быть больше 0")]
        public int id_status { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DateLimitAttribute(ErrorMessage = "Дата установки статуса должна быть меньше или равна текущей")]
        [Display(Name = "Дата установки статуса")]
        public DateTime date_status_change { get; set; }
        [Display(Name = "ID Муниципального образования")]
        [Range(0, int.MaxValue, ErrorMessage = "Значение должно быть больше или равно 1")]
        [Required]
        public int ID_MU { get; set; }
        [Display(Name = "Населенный пункт")]
        [Required]
        public string local_place { get; set; }
        [Display(Name = "Документ")]
        public byte[] document { get; set; }
        [Display(Name = "Скан-образ акта первичного осмотра")]
        public byte[] scan_frame_1 { get; set; }
        [Display(Name = "Скан-образ акта первичного осмотра")]
        public byte[] scan_frame_2 { get; set; }
        [Display(Name = "Дата стерелизации")]
        [DateLimit(ErrorMessage = "Дата стерилизации статуса должна быть меньше или равна текущей")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime sterilization_date { get; set; }

        [NotMapped]
        [Display(Name = "Муниципальное образование")]
        public string MU { get; set; }

        [NotMapped]
        public string Status { get; set; }
    }
}