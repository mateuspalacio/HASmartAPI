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

        [Required(ErrorMessage = mensagemErroRelatorio)]
        public string RelatorioCidadao { get; set; }
        public TipoContato TipoContato { get; set; }
        [DataType(DataType.Date)]
        [DefaultValue("20/01/2000")]
        public DateTime DataRelatorio { get; set; }
    }
}
