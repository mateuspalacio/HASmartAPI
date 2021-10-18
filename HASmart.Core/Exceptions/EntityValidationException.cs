using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using HASmart.Core.Architecture;
using HASmart.Core.Extensions;


namespace HASmart.Core.Exceptions {
    public class EntityValidationException : Exception {
        public Type EntityType { get; }
        public IEnumerable<ValidationResult> Errors { get; }

        public EntityValidationException(Type t, IEnumerable<ValidationResult> errors) 
            : base($"Erros ao validar a entidade do tipo {t.Name}: " + 
                   errors.Select(x => x.ErrorMessage).ToCsv()) {

            this.EntityType = t;
            this.Errors = errors;
        }

        public EntityValidationException(Type t, string property, string errorMessage) 
            : base($"Erros ao validar a entidade do tipo {t.Name}: {property}: {errorMessage}") {

            this.EntityType = t;
            this.Errors = new List<ValidationResult> {
                new ValidationResult(errorMessage, new []{property})    
            };
        }
    }
}
