using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PIS_Project.CustomValidate
{
    public class DateLimitAttribute : ValidationAttribute, IClientValidatable
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime dateTime = Convert.ToDateTime(value);
            if(dateTime <= DateTime.Now)
            {
                return ValidationResult.Success;
            }
            else
            {
                var errorMessage = FormatErrorMessage(validationContext.DisplayName);

                return new ValidationResult(errorMessage);
            }
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {

;
            string errorMessage = ErrorMessageString;
            ModelClientValidationRule dateGreaterThanRule = new ModelClientValidationRule();
            dateGreaterThanRule.ErrorMessage = errorMessage;
            dateGreaterThanRule.ValidationType = "dategreaterthan"; 
            yield return dateGreaterThanRule;
        }
    }
}