using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HASmart.Core.Architecture;
using HASmart.Core.Validation;


namespace HASmart.Core.Entities.DTOs {
    public class AfericaoPostDTO : DTO {
        [Range(1, 300, ErrorMessage = "A pressão sistólica deve estar entre 1 e 300")]
        public uint Sistolica { get; set; }

        [Range(1, 300, ErrorMessage = "A pressão diastólica deve estar entre 1 e 300")]
        [IsLessThan(nameof(Sistolica), ErrorMessage = "A pressão diastólica deve ser inferior à sistólica")]
        public uint Diastolica { get; set; }
    }
}