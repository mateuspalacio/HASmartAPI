using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using HASmart.Core.Architecture;
using HASmart.Core.Validation;


namespace HASmart.Core.Entities.DTOs {
    public class CidadaoPostDTO : DTO {
        public const string mensagemErroNome = "O campo nome é obrigatório.";
        public const string mensagemErroCpf = "O campo CPF deve ser composto por 11 números.";
        public const string mensagemErroRg = "O campo RG deve ser composto apenas por números.";
        public const string mensagemErroDataNascimento = "O Cidadao deve ter pelo menos 18 anos de idade.";
        public const string mensagemErroDadosPessoais = "O campo 'DadosPessoais' é obrigatório";
        public const string mensagemErroIndicadorRiscoHAS = "O campo 'IndicadorRiscoHAS' é obrigatório";

        [Required(ErrorMessage = mensagemErroNome)]
        public string Nome { get; set; }
        
        [Required(ErrorMessage = mensagemErroCpf)]
        [RegularExpression(@"^\d{11}", ErrorMessage = mensagemErroCpf)]
        public string Cpf { get; set; }

        [Required(ErrorMessage = mensagemErroRg)]
        [RegularExpression(@"^\d+", ErrorMessage = mensagemErroRg)]
        public string Rg { get; set; }

        [EnforceDateFormat]
        [RequiresMinimumAge(18, mensagemErroDataNascimento)]
        [DataType(DataType.Date)]
        [DefaultValue("01/01/2000")]
        public DateTime? DataNascimento { get; set; }

        [Required(ErrorMessage = mensagemErroDadosPessoais)]
        public DadosPessoaisDTO DadosPessoais { get; set; }
        [Required(ErrorMessage = mensagemErroIndicadorRiscoHAS)]
        public IndicadorRiscoHASDTO IndicadorRiscoHAS { get; set; }
    }
}