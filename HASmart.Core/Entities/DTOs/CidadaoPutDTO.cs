using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HASmart.Core.Architecture;
using HASmart.Core.Validation;


namespace HASmart.Core.Entities.DTOs {
    public class CidadaoPutDTO : DTO {
        [Required(ErrorMessage = CidadaoPostDTO.mensagemErroDadosPessoais)]
        public DadosPessoaisDTO DadosPessoais { get; set; }

        [Required(ErrorMessage = CidadaoPostDTO.mensagemErroIndicadorRiscoHAS)]
        public IndicadorRiscoHASDTO IndicadorRiscoHAS { get; set; }
    }
}