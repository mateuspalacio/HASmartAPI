using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HASmart.Core.Architecture
{
    public class Entity {
        [Column(TypeName = "CHAR(36)")]
        public Guid Id { get; set; }
    }
}
