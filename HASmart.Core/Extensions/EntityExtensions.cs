using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using HASmart.Core.Architecture;
using HASmart.Core.Exceptions;


namespace HASmart.Core.Extensions {
    public static class EntityExtensions {
        public static void ThrowIfInvalid<T>(this T entity) where T : DTO {
            List<ValidationResult> result = new List<ValidationResult>();
            bool validated = true;
            validated &= Validator.TryValidateObject(entity, new ValidationContext(entity), result, validateAllProperties: true);
            foreach (PropertyInfo property in typeof(T).GetProperties().Where(p => p.PropertyType.BaseType == typeof(DTO))) {
                object value = property.GetValue(entity);
                // Quem detecta o null como erro é a própria entidade utilizando o [Required].
                if (value != null) {
                    validated &= Validator.TryValidateObject(value, new ValidationContext(value), result, validateAllProperties: true);
                }
            }

            if (!validated) {
                throw new EntityValidationException(typeof(T), result);
            }
        }
    }
}
