using System;
using System.Collections.Generic;
using HASmart.Core.Architecture;


namespace HASmart.Core.Exceptions {
    public class EntityConcurrencyException : Exception {
        public Type EntityType { get; }

        public EntityConcurrencyException(Type t) : base($"Entidate do tipo {t.Name} foi alterada concorrentemente") { 
            this.EntityType = t;
        }
    }
}
