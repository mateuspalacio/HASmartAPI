using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HASmart.Core.Architecture;
using HASmart.Core.Validation;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations.Schema;

namespace HASmart.Core.Entities {
    public class Medicamento : Entity {
        [JsonIgnore]
        [Column(TypeName = "CHAR(36)")]
        public Guid Id { get; set; }
        public string Nome { get; set; }
        [JsonIgnore]
        [Column(TypeName = "CHAR(36)")]
        public Guid MedicaoId { get; set; }
        [JsonIgnore]
        public Medicao Medicao { get; set; }

    }
}