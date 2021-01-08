using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PIS_Project.Models.DataClasses;
using PIS_Project.Models.DataControllers;
using System.Web.Mvc;
using System.ComponentModel;

namespace PIS_Project.Controllers.DataControllers
{
    public static class ValidationController
    {
        public static ValidationResponse CheckValidation(Type classType,Dictionary<string, object> ArrayOfData)
        {
            var message = "";
            var new_user = Activator.CreateInstance(classType);
            var valid = true;
            foreach (var data in ArrayOfData)
            {
                try
                {
                    var prop = new_user.GetType().GetProperty(data.Key);
                    var converter = TypeDescriptor.GetConverter(prop.PropertyType);
                    var result = new object();
                    if (data.Value.GetType().IsArray)
                    {
                        result = (object)"";
                        foreach (var val in ((Array)data.Value))
                        {
                            result = result.ToString() + (string)val;
                        }
                    }
                    else
                        result = data.Value;
                    try
                    {
                        if (result.GetType() != prop.PropertyType)
                            result = converter.ConvertFrom(result);
                    }
                    catch
                    {
                        result = Convert.FromBase64String(result.ToString());
                    }
                    prop.SetValue(new_user, result);
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