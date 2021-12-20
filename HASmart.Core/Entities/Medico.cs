using HASmart.Core.Architecture;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace HASmart.Core.Entities
{
    public class Medico : AggregateRoot
    {
        public string Nome { get; set; }
        [JsonIgnore]
        public string Crm { get; set; }
        public List<Cidadao> cidadaosAtuais { get; set; }
        [JsonIgnore]
        [Column(TypeName = "CHAR(36)")]
        public Guid Id { get; set; }
        //public string Cpf { get; set; }
        [JsonIgnore]
        public string Senha { get; set; }
        public string Email { get; set; }

        //public List<Cidadao> cidadaosAtendidos { get; set; }  
    }
}