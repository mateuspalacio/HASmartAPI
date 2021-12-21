using HASmart.Core.Architecture;
using System.ComponentModel.DataAnnotations;
using HASmart.Core.Validation;
using System.Collections.Generic;

namespace HASmart.Core.Entities.DTOs
{
    public class MedicoPostDTO : DTO
    {
        public const string mensagemErroNome = "O campo nome é obrigatório.";
        public const string mensagemErroCrm = "O campo Crm deve ser composto apenas por números.";

        [Required(ErrorMessage = mensagemErroNome)]
        public string Nome { get; set; }
        public string Crm { get; set; }
        public string Senha { get; set; }
        public string Email { get; set; }

    }
}