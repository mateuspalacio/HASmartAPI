using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HASmart.Core.Architecture;
using HASmart.Core.Validation;


namespace HASmart.Core.Entities.DTOs {
    public class MedicaoPostDTO : DTO {

        private const string mensagemErroCod = "O campo Codigofarm é obrigatório";

        [EnforceLength(minimum: 2, ErrorMessage = "São necessesárias ao menos duas aferições")]
        public List<AfericaoPostDTO> Afericoes { get; set; }

        [Range(30, 400, ErrorMessage = "O campo 'Peso' deve estar entre 30 e 400 kg")]
        public float Peso { get; set; }
        public List<MedicamentoPostDTO> Medicamentos{ get; set;}
        //[Required(ErrorMessage = mensagemErroCod)]
        //public string CodigoFarm { get; set; }
    }
}