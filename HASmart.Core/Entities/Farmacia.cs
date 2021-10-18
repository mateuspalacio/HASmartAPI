using System;
using System.Collections.Generic;
using System.Text;
using HASmart.Core.Architecture;


namespace HASmart.Core.Entities {
    public class Farmacia : AggregateRoot {
        public string NomeFantasia { get; set; }
        public string RazaoSocial { get; set; }
        public string Cnpj { get; set; }
        public EnderecoFarmacia Endereco { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
    }
}
