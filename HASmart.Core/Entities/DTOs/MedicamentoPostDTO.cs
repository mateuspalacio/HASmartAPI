using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using HASmart.Core.Architecture;


namespace HASmart.Core.Entities.DTOs {
    public class MedicamentoPostDTO : DTO {
        [Required(ErrorMessage = "O campo 'Nome' é obrigatório")]
        public string Nome { get; set; }

    }
}
