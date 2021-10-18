using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HASmart.Core.Architecture;
using HASmart.Core.Validation;


namespace HASmart.Core.Entities.DTOs {
    public class FarmaciaPostDTO: DTO {
      
        public const string mensagemErroNome = "O campo nome é obrigatório.";
        public const string mensagemErroRazãoSocial = "O campo Razão Social é obrigatório.";
        public const string mensagemErroCnpj = "O campo Cnpj é obrigatório e deve ser composto por 14 números.";
        public const string mensagemErroEndereco = "O campo Endereço é obrigatório.";

        [Required(ErrorMessage=mensagemErroNome)]
        public string NomeFantasia { get; set; }
        [Required(ErrorMessage=mensagemErroRazãoSocial)]
        public string RazaoSocial { get; set; }
        [Required(ErrorMessage=mensagemErroCnpj)]
        [RegularExpression(@"^\d{14}", ErrorMessage = mensagemErroCnpj)]
        public string Cnpj { get; set; }
        [Required(ErrorMessage=mensagemErroEndereco)]
        public EnderecoPostDTO Endereco { get; set; }

    }
}