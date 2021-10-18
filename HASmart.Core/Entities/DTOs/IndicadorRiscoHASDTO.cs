using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using HASmart.Core.Architecture;
using HASmart.Core.Validation;


namespace HASmart.Core.Entities.DTOs {
    public class IndicadorRiscoHASDTO : DTO {
        public const string mensagemErroAltura = "O campo altura deve estar no intervalo entre 0.5 e 3.0 metros";
        public const string mensagemErroDiabetes = "O valor no campo Diabetes não foi reconhecido.";
        public const string mensagemErroFumante = "O valor no campo Fumante não foi reconhecido.";
        public const string mensagemErroDoencaCardiovascular = "O campo DoencaCardiovascular ê obrigatôrio";
        public const string mensagemErroHistoricoAvc = "O campo HistoricoAvc ê obrigatôrio";
        public const string mensagemErroDoencaRenalCronica = "O campo DoencaRenalCronica ê obrigatôrio";
        public const string mensagemErroInsuficienciaCardiaca = "O campo InsuficienciaCardiaca ê obrigatôrio";
        public const string mensagemErroHistoricoInfarto = "O campo HistoricoInfarto ê obrigatôrio";
        public const string mensagemErroHistoricoAvcEnum = "O valor no campo HistoricoAvc não foi reconhecido.";
        public const string mensagemErroDoencaRenalCronicaEnum = "O valor no campo DoencaRenalCronica não foi reconhecido.";
        public const string mensagemErroInsuficienciaCardiacaEnum = "O valor no campo InsuficienciaCardiaca não foi reconhecido.";
        public const string mensagemErroHistoricoInfartoEnum = "O valor no campo HistoricoInfarto não foi reconhecido.";
        public const string mensagemErroDoencaArterialObstrutivaPeriferica = "O valor no campo DoencaArterialObstrutivaPeriferica não foi reconhecido.";
        public const string mensagemErroRetinopatiaHipertensiva = "O valor no campo RetinopatiaHipertensiva não foi reconhecido.";

        [Required(ErrorMessage = mensagemErroAltura)]
        [Range(0.5f, 3.0f, ErrorMessage = mensagemErroAltura)]
        public float Altura { get; set; }

        [Required(ErrorMessage = mensagemErroDiabetes)]
        [IsValidEnumValue(typeof(TipoDiabetes), ErrorMessage = mensagemErroDiabetes)]
        public TipoDiabetes Diabetico { get; set; }

        [Required(ErrorMessage = mensagemErroFumante)]
        [IsValidEnumValue(typeof(TipoFumante), ErrorMessage = mensagemErroFumante)]
        public TipoFumante Fumante { get; set; }

        [Required(ErrorMessage = mensagemErroHistoricoAvc)]
        [IsValidEnumValue(typeof(TipoComorbidade), ErrorMessage = mensagemErroRetinopatiaHipertensiva)]
        public TipoComorbidade? HistoricoAvc { get; set; }
        [Required(ErrorMessage = mensagemErroDoencaRenalCronica)]
        [IsValidEnumValue(typeof(TipoComorbidade), ErrorMessage = mensagemErroRetinopatiaHipertensiva)]
        public TipoComorbidade? DoencaRenalCronica { get; set; }
        [Required(ErrorMessage = mensagemErroInsuficienciaCardiaca)]
        [IsValidEnumValue(typeof(TipoComorbidade), ErrorMessage = mensagemErroRetinopatiaHipertensiva)]
        public TipoComorbidade? InsuficienciaCardiaca { get; set; }
        [Required(ErrorMessage = mensagemErroHistoricoInfarto)]
        [IsValidEnumValue(typeof(TipoComorbidade), ErrorMessage = mensagemErroRetinopatiaHipertensiva)]
        public TipoComorbidade? HistoricoInfarto { get; set; }
        [IsValidEnumValue(typeof(TipoComorbidade), ErrorMessage = mensagemErroDoencaArterialObstrutivaPeriferica)]
        public TipoComorbidade? DoencaArterialObstrutivaPeriferica { get; set; }
        [IsValidEnumValue(typeof(TipoComorbidade), ErrorMessage = mensagemErroRetinopatiaHipertensiva)]
        public TipoComorbidade? RetinopatiaHipertensiva { get; set; }
    }
}
