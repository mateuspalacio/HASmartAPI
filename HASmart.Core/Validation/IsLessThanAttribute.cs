using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace HASmart.Core.Validation {
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class IsLessThanAttribute : ValidationAttribute {
        public string OtherProperty { get; }
        public override bool RequiresValidationContext => true;
        public IsLessThanAttribute(string otherProperty) {
            this.OtherProperty = otherProperty ?? throw new ArgumentNullException(nameof(otherProperty));
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext) {
            PropertyInfo otherPropertyInfo = validationContext.ObjectType.GetRuntimeProperty(OtherProperty);
            if (otherPropertyInfo == null) {
                return new ValidationResult(ErrorMessage);
            } else if (otherPropertyInfo.GetIndexParameters().Any()) {
                return new ValidationResult(ErrorMessage);
            } else {
                IComparable v1 = (IComparable)value;
                IComparable v2 = (IComparable)otherPropertyInfo.GetValue(validationContext.ObjectInstance, null);
                return v1.CompareTo(v2) < 0 ? null : new ValidationResult(ErrorMessage);
            }
        }
    }
}
