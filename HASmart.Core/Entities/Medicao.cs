using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HASmart.Core.Architecture;
using HASmart.Core.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace HASmart.Core.Entities {
    // TODO RN08 ainda não definida: Processo de medição
    // TODO RN09 ainda não definida: Formato para registro de pressão arterial
    // TODO RN10 ainda não definida: Formato para registro do peso
    public class Medicao : AggregateRoot {
        [ForeignKey("Cidadao")]
        [JsonIgnore]
        [Column(TypeName = "CHAR(36)")]
        public Guid CidadaoId { get; set; }
        [JsonIgnore]
        public Cidadao Cidadao { get; set; }
        public List<Afericao> Afericoes { get; set; }
        public float Peso { get; set; }
        public List<Medicamento> Medicamentos{ get; set; }
        
        [DataType(DataType.Date)]
        [DefaultValue("01/01/2000")]
        public DateTime DataHora { get; set; }
    }
}
