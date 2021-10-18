using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HASmart.Core.Architecture;
using HASmart.Core.Validation;
using System.ComponentModel;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations.Schema;

namespace HASmart.Core.Entities {
    public class Dispencacao : Entity {
        [JsonIgnore]
        [Column(TypeName = "CHAR(36)")]
        public Guid CidadaoId { get; set; }
        public List<Medicamento> Medicamentos { get; set; }
        
        [DataType(DataType.Date)]
        [DefaultValue("01/01/2000")]
        public DateTime DataHora { get; set; }
    }
}