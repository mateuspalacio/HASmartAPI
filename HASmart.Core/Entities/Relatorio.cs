using HASmart.Core.Architecture;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HASmart.Core.Entities
{
    public enum TipoContato
    {
        Invalido = 0,
        WhatsApp = 1,
        Ligacao = 2,
        SMS = 3,
        Presencial = 4,
        Email = 5,
        Outros = 6
    }
    public class Relatorio : AggregateRoot
    {
        public string RelatorNome { get; set; }
        public string RelatorioCidadao { get; set; }
        [JsonIgnore]
        public Cidadao Cidadao { get; set; }
        [ForeignKey("Cidadao")]
        [JsonIgnore]            
        [Column(TypeName = "CHAR(36)")]
        public Guid CidadaoId { get; set; }
        public TipoContato TipoContato { get; set; }
        public DateTime DataRelatorio { get; set; } = DateTime.Now;
        public bool Success { get; set; }


    }
}
