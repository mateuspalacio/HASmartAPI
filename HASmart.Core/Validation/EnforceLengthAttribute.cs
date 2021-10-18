using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HASmart.Core.Validation {
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class EnforceLengthAttribute : ValidationAttribute {
        public int Min { get; }
        public int Max { get; }

        public EnforceLengthAttribute(int minimum = int.MinValue, int maximum = int.MaxValue) {
            this.Min = minimum;
            this.Max = maximum;
            this.ErrorMessage = ErrorMessage;
        }


        public string GetErrorMessage() {
            return this.ErrorMessage ?? $"O tamanho da lista está fora do range [{this.Min}, {this.Max}]";
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext) {
            if (value == null) {
                return new ValidationResult(GetErrorMessage());
            }

            IList lista = (IList)value;
            int count = lista.Count;

            if (count < this.Min || count > this.Max) {
                return new ValidationResult(GetErrorMessage());
            } else {
                return ValidationResult.Success;
            }
        }
    }
}
