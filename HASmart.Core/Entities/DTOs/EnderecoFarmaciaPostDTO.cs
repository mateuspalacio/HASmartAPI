using System;
using System.ComponentModel.DataAnnotations;
using HASmart.Core.Architecture;
using HASmart.Core.Validation;


namespace HASmart.Core.Entities {
    public class EnderecoFarmaciaPostDTO : EnderecoPostDTO {
        private const string mensagemErroMacroregião= "O campo Macroregião é obrigatório";

        private const string mensagemErroValidaçãoMacroregião= "O campo Macroregião é um valor de 1 a 14 que representa uma macroregião específica ao numero";

        [Required(ErrorMessage =  mensagemErroMacroregião)]
        [IsValidEnumValue(typeof(Macroregiao), ErrorMessage = mensagemErroValidaçãoMacroregião)]
        public Macroregiao Macroregiao { get; set; }
    }
}
