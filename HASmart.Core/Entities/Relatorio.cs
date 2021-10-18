using HASmart.Core.Architecture;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HASmart.Core.Entities
{
    public class Relatorio : AggregateRoot
    {
        public string RelatorioCidadao { get; set; }
        [JsonIgnore]
        public Cidadao Cidadao { get; set; }
        [ForeignKey("Cidadao")]
        [JsonIgnore]
        [Column(TypeName = "CHAR(36)")]
        public Guid CidadaoId { get; set; }
        public DateTime DataRelatorio { get; set; } = DateTime.Now.Date;

    }
}
