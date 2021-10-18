using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using HASmart.Core.Architecture;
using Microsoft.EntityFrameworkCore;

namespace HASmart.Core.Entities {
    public class DadosPessoais : EntityProperty {
        [Key]
        [JsonIgnore]
        [Column(TypeName = "CHAR(36)")]
        public Guid DadosPessoaisId { get; set; }
        [ForeignKey("Cidadao")]
        [JsonIgnore]
        [Column(TypeName = "CHAR(36)")]
        public Guid CidadaoId { get; set; }
        public Cidadao Cidadao { get; set; }
        public Endereco Endereco { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string Genero { get; set; }
    }
}
