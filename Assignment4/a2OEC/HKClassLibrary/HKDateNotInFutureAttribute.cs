using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace HKClassLibrary
{
    public class HKDateNotInFutureAttribute : ValidationAttribute
    {
        public HKDateNotInFutureAttribute()
        {
            ErrorMessage = "{0} is future time. Please select valid date";
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(Convert.ToDateTime(value) <= DateTime.Now || value == null)
            {
                return ValidationResult.Success;
            }
            return new ValidationResult(string.Format(ErrorMessage, validationContext.DisplayName));
        }
    }
}
