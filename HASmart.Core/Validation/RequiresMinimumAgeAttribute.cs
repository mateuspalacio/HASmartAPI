using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HASmart.Core.Validation {
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class RequiresMinimumAgeAttribute : ValidationAttribute {
        private readonly int requiredAge;
        public RequiresMinimumAgeAttribute(int requiredAge) : base($"A data deve ser não-nula e ser de pelomenos {requiredAge} anos atrás") {
            this.requiredAge = requiredAge;
        }
        public RequiresMinimumAgeAttribute(int requiredAge, string errorMessage) : base(errorMessage) {
            this.requiredAge = requiredAge;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext) {
            if (value != null && value is DateTime data) {
                DateTime referencia = DateTime.Now.AddYears(-this.requiredAge);
                return data < referencia ? ValidationResult.Success : new ValidationResult(this.ErrorMessage);
            } else {
                return new ValidationResult(this.ErrorMessage);
            }
        }
    }
}
