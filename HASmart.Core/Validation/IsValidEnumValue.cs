using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HASmart.Core.Extensions;


namespace HASmart.Core.Validation {
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class IsValidEnumValue : ValidationAttribute {
        public Type Type { get; }

        public IsValidEnumValue(Type type) {
            if (!type.IsEnum) {
                throw new ArgumentException("O valor passado para o Enum deve ser válido");
            }

            this.Type = type;
        }

        public string GetErrorMessage() {
            string error = this.ErrorMessage ?? $"O Valor fornecido não é válido para o tipo {this.Type.Name}.";
            error = error + " Valores possíveis: " + EnumExtensions.ListValues(this.Type);
            return error;
        }


        protected override ValidationResult IsValid(object value, ValidationContext validationContext) {
            if(value == null && this.Type.ToString().Equals("HASmart.Core.Entities.TipoComorbidade")){
                return ValidationResult.Success;
            } else {
                if (Enum.IsDefined(this.Type, value ) && (int)value != 0) {
                    return ValidationResult.Success;
                } else {
                    return new ValidationResult(this.GetErrorMessage());
                }
            }
        }
    }
}