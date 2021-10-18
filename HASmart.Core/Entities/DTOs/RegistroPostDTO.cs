using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using HASmart.Core.Architecture;
using HASmart.Core.Validation;


namespace HASmart.Core.Entities.DTOs {
    public class RegistroPostDTO : DTO {
        [Required]
        public CidadaoPostDTO cidadaoPostDTO { get; set;}
        [Required]
        public MedicaoPostDTO MedicaoPostDTO { get; set;}
    }
}