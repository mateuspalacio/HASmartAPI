using HASmart.Core.Architecture;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HASmart.Core.Entities.DTOs
{
    public class RelatorioPostDTO : DTO
    {
        public const string mensagemErroRelatorio = "O campo Relatorio é obrigatório.";
        public string RelatorNome { get; set; }

        [Required(ErrorMessage = mensagemErroRelatorio)]
        public string RelatorioCidadao { get; set; }
        public TipoContato TipoContato { get; set; }
        public DateTime DataRelatorio { get; set; }
        public bool Success { get; set; }
    }
}
