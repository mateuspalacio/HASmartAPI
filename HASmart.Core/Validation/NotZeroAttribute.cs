using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;

namespace HASmart.Core.Validation {
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class NotZeroAttribute : ValidationAttribute {
        public NotZeroAttribute() : base("Este campo nao pode ser zero") { }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext) {
            long v = (long)value;
            return v > 0 ? ValidationResult.Success : new ValidationResult(this.ErrorMessage);
        }
    }
}
