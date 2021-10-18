using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using HASmart.Core.Architecture;
using HASmart.Core.Validation;


namespace HASmart.Core.Entities.DTOs {
    public class DispencacaoPostDTO : DTO {
        [NotZero]
        public long MedicaoId { get; set; }

        [EnforceLength(minimum: 1)]
        public List<MedicamentoPostDTO> Medicamentos { get; set; }
    }
}
