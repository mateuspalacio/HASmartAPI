using System;
using System.ComponentModel.DataAnnotations;
using HASmart.Core.Architecture;
using HASmart.Core.Validation;


namespace HASmart.Core.Entities {
    public class EnderecoPostDTO : DTO {
        private const string mensagemErroCEPRequired = "O campo CEP e obrigatório";

        private const string mensagemErroRuaRequired = "O campo Rua é obrigatório";
        
        private const string mensagemErroNumeroRequired = "O campo Numero é obrigatório";

        private const string mensagemErroCidadeRequired = "O campo cidade é obrigatório";

        private const string mensagemErroEstadoRequired = "O campo estado é obrigatório";

        private const string mensagemErroCEP = "CEP deve ser composto por 8 digitos";


        [Required(ErrorMessage = mensagemErroCEPRequired)]
        [RegularExpression(@"^\d{8}", ErrorMessage = mensagemErroCEP)]
        public string CEP { get; set; }

        [Required(ErrorMessage = mensagemErroRuaRequired)]
        public string Rua { get; set; }

        [Required(ErrorMessage = mensagemErroNumeroRequired)]
        public string Numero { get; set; }
        public string Complemento { get; set; }

        [Required(ErrorMessage = mensagemErroCidadeRequired)]
        public string Cidade { get; set; }

        [Required(ErrorMessage = mensagemErroEstadoRequired)]
        public string Estado { get; set; }
    }
}
