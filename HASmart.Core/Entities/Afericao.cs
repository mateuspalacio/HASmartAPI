using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using HASmart.Core.Architecture;
using HASmart.Core.Validation;


namespace HASmart.Core.Entities {
    public class Afericao : Entity {
        public uint Sistolica { get; set; }
        public uint Diastolica { get; set; }
        [ForeignKey("Medicao")]
        [JsonIgnore]
        [Column(TypeName = "CHAR(36)")]
        public Guid MedicaoId { get; set; }
        [JsonIgnore]
        public Medicao Medicao { get; set; }
    }
}