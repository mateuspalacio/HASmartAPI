using System;
using System.ComponentModel.DataAnnotations;
using HASmart.Core.Architecture;


namespace HASmart.Core.Entities.DTOs {
    public class DadosPessoaisDTO : DTO {
        public const string mensagemErroEndereco = "O campo Endereço é obrigatório.";
        public const string mensagemErroEmail = "Endereço de e-mail inválido.";
        public const string mensagemErroTelefone = "O campo telefone é obrigatório e deve ser composto apenas por números";
        public const string mensagemErroGenero = "O campo gênero é obrigatório deve ser composto apenas por letras";

        [Required(ErrorMessage = mensagemErroEndereco)]
        public EnderecoPostDTO Endereco { get; set; }

        [Required(ErrorMessage = mensagemErroEmail)]
        [EmailAddress(ErrorMessage = mensagemErroEmail)]
        public string Email { get; set; }

        [Required(ErrorMessage = mensagemErroTelefone)]
        [RegularExpression(@"^\d+", ErrorMessage = mensagemErroTelefone)]
        public string Telefone { get; set; }

        [Required(ErrorMessage = mensagemErroGenero)]
        [RegularExpression(@"^\D+", ErrorMessage = mensagemErroGenero)]
        public string Genero { get; set; }
    }
}
