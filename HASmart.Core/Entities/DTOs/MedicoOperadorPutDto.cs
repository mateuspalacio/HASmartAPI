using HASmart.Core.Architecture;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HASmart.Core.Entities.DTOs
{
    public class MedicoOperadorPutDto : DTO
    {
        [Required(ErrorMessage = MedicoPostDTO.mensagemErroNome)]
        public string Nome { get; set; }
        [Required(ErrorMessage = MedicoPostDTO.mensagemErroCrm)]
        public string Crm { get; set; }

        [Required(ErrorMessage = "Informe a nova senha")]
        public string Senha { get; set; }
        [Required(ErrorMessage = "Informe o email")]
        public string Email { get; set; }
    }
}
