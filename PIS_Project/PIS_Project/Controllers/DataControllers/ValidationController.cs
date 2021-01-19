using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PIS_Project.Models.DataClasses;
using PIS_Project.Models.DataControllers;
using System.Web.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace PIS_Project.Controllers.DataControllers
{
    /// <summary>
    /// Класс для проверки соответсятвия данных моделям для записи
    /// </summary>
    public static class ValidationController
    {
        /// <summary>
        /// Проверка вилидности значений для выбраного типа
        /// </summary>
        /// <param name="classType">Тип объекта для проверки</param>
        /// <param name="ArrayOfData">Поля и их значения для изменения</param>
        /// <returns>Объект ValidationResponse, содержащий результаты проверки</returns>
        public static ValidationResponse CheckValidation(Type classType,Dictionary<string, object> ArrayOfData)
        {
            var message = "";
            var new_user = Activator.CreateInstance(classType);
            var valid = true;
            foreach (var data in ArrayOfData)
            {
                var t = typeof(Card);
                var pi = t.GetProperty(data.Key);
                bool hasNotMapped = Attribute.IsDefined(pi, typeof(NotMappedAttribute));

                try
                {
                    var prop = new_user.GetType().GetProperty(data.Key);
                    var converter = TypeDescriptor.GetConverter(prop.PropertyType);
                    var result = new object();
                    #region Заполнение поля
                    /*Поле соответветствует нужному типу*/
                    if (data.Value.GetType() == prop.PropertyType)
                    {
                        result = data.Value;
                        prop.SetValue(new_user, result);
                        continue;
                    }
                    /*Поле массив но нужна строка*/
                    if (data.Value.GetType().IsArray)
                    {
                        result = (object)"";
                        foreach (var val in ((Array)data.Value))
                        {
                            result = result.ToString() + val.ToString();
                        }
                    }
                    else
                        result = data.Value;
                    try
                    {
                        /*Стандартное приведение*/
                        if (result.GetType() != prop.PropertyType)
                            result = converter.ConvertFrom(result);
                    }
                    catch
                    {
                        /*Декодирование картинок и файлов*/
                        result = Convert.FromBase64String(result.ToString());
                    }
                    prop.SetValue(new_user, result);
                    #endregion
                }
                catch { valid = false; message += $"Property value {data.Key} failed validation.\n"; }
            }
            if (valid)
                message = "Validation successful";
            return new ValidationResponse()
            {
                Result = valid,
                Information = message,
                ValidData = (valid ? new_user : null)
            };
        }
    }
}