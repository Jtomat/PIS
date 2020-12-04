using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PIS_Project.Models.DataClasses;

namespace PIS_Project.Models.DataControllers
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
                    prop.SetValue(new_user, data.Value);
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