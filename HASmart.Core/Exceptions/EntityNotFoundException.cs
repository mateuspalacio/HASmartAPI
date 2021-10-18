using System;
using System.Collections.Generic;
using HASmart.Core.Architecture;


namespace HASmart.Core.Exceptions {
    public class EntityNotFoundException : Exception {
        public Type EntityType { get; }

        public EntityNotFoundException(Type t) : base($"Entidate do tipo {t.Name} não encontrada") { 
            this.EntityType = t;
        }
        public EntityNotFoundException(Type t, string message) : base(message) { 
            this.EntityType = t;
        }
    }
}
