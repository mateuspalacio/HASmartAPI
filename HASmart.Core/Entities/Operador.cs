using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;
using HASmart.Core.Architecture;


namespace HASmart.Core.Entities {
    public class Operador : Entity {
        [JsonIgnore]
        [Column(TypeName = "CHAR(36)")]
        public Guid Id { get; set; }
        public string Nome { get; set; }
        //public string Cpf { get; set; }
        public string Senha { get; set; }
    }
}
