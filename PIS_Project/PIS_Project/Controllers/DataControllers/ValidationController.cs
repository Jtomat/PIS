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
                    //var result = converter.ConvertFrom(data.Value);
                    //
                    var result = new object();
                    try
                    {
                        result = converter.ConvertFrom(data.Value);
                    }
                    catch
                    {
                        try {
                            result = Convert.FromBase64String(data.Value.ToString());
                        }
                        catch {
                            try
                            {
                                result = converter.ConvertFrom((data.Value as string[])[0]);
                            }
                            catch
                            {
                                result = Convert.FromBase64String((data.Value as string[])[0]);
                            }
                        }
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