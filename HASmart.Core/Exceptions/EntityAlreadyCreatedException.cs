using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HASmart.Core.Exceptions
{
    public class EntityAlreadyCreatedException : Exception
    {
        public Type EntityType { get; }

        public EntityAlreadyCreatedException(Type t) : base($"Entidate do tipo {t.Name} já existe")
        {
            this.EntityType = t;
        }
        public EntityAlreadyCreatedException(Type t, string message) : base(message)
        {
            this.EntityType = t;
        }
    }
}
