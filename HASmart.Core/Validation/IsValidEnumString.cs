using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HASmart.Core.Extensions;


namespace HASmart.Core.Validation {
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class IsValidEnumString : ValidationAttribute {
        public Type Type { get; }

        public IsValidEnumString(Type type) {
            if (!type.IsEnum) {
                throw new ArgumentException("O tipo suprido deve ser um tipo Enum");
            }

            this.Type = type;
        }


        public string GetErrorMessage() {
            string error = this.ErrorMessage ?? $"O Valor fornecido não é convertível para o tipo {this.Type.Name}.";
            error = error + " Valores possíveis: " + EnumExtensions.ListStrings(this.Type);
            return error;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext) {
            if (!(value is string text)) {
                return new ValidationResult(this.GetErrorMessage());
            }

            if (!EnumExtensions.TryParse(this.Type, text, out object result)) {
                return new ValidationResult(this.GetErrorMessage());
            } else {
                return ValidationResult.Success;
            }
        }
    }
}