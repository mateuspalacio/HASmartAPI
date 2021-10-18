using System;
using HASmart.Core.Architecture;
using System.ComponentModel.DataAnnotations;
using HASmart.Core.Validation;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace HASmart.Core.Entities  {
    public class Endereco : EntityProperty {
        [JsonIgnore]
        [Column(TypeName = "CHAR(36)")]
        public Guid Id { get; set; }
        [ForeignKey("DadosPessoais")]
        [JsonIgnore]
        [Column(TypeName = "CHAR(36)")]
        public Guid DadosPessoais_Id { get; set; }
        [JsonIgnore]
        public DadosPessoais DadosPessoais { get; set; }
        public string CEP { get; set; }
        public string Rua { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Cidade {get; set;}
        public string Estado { get;set; }
    }
}
