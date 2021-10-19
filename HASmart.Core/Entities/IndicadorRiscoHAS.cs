using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using HASmart.Core.Architecture;
using HASmart.Core.Validation;
using Microsoft.EntityFrameworkCore;

namespace HASmart.Core.Entities {
    public enum TipoDiabetes { 
        Invalido = 0, 

        Nao = 1, Não = 1, 
        Tipo1 = 2, Tipo_1 = 2, 
        Tipo2 = 3, Tipo_2 = 3, 
        DiabetesGestante = 4, Diabetes_Gestante = 4
    }
    public enum TipoFumante {
        Invalido = 0, 

        NaoFumante = 1, NãoFumante = 1, Nao_Fumante = 1, Não_Fumante = 1,
        Fumante = 2,
        ExFumante = 3, Ex_Fumante = 3
    }
    public enum TipoComorbidade {
        NaoInformou = 0, NãoImformou = 0,Nao_Informou = 0, Não_Informou = 0,
        Nao = 1, Não = 1,
        Sim = 2
    }
    public class IndicadorRiscoHAS : EntityProperty {
        public const string mensagemErroAltura = "O campo altura deve estar no intervalo entre 0.5 e 3.0 metros";
        public const string mensagemErroDiabetes = "O valor no campo Diabetes não foi reconhecido.";
        public const string mensagemErroFumante = "O valor no campo Fumante não foi reconhecido.";
        public const string mensagemErroHistoricoAvc = "O valor no campo HistoricoAvc não foi reconhecido.";
        public const string mensagemErroDoencaRenalCronica = "O valor no campo DoencaRenalCronica não foi reconhecido.";
        public const string mensagemErroInsuficienciaCardiaca = "O valor no campo InsuficienciaCardiaca não foi reconhecido.";
        public const string mensagemErroHistoricoInfarto = "O valor no campo HistoricoInfarto não foi reconhecido.";
        public const string mensagemErroDoencaArterialObstrutivaPeriferica = "O valor no campo DoencaArterialObstrutivaPeriferica não foi reconhecido.";
        public const string mensagemErroRetinopatiaHipertensiva = "O valor no campo RetinopatiaHipertensiva não foi reconhecido.";
        [JsonIgnore]
        [Column(TypeName = "CHAR(36)")]
        public Guid Id { get; set; }
        [ForeignKey("Cidadao")]
        [JsonIgnore]
        [Column(TypeName = "CHAR(36)")]
        public Guid Cidadao_Id { get; set; }
        [JsonIgnore]
        public Cidadao Cidadao { get; set; }
        [Required]
        //[Range(0.5f, 3.0f, ErrorMessage = mensagemErroAltura)]
        public float Altura { get; set; }
        [Required]
        [IsValidEnumValue(typeof(TipoDiabetes), ErrorMessage = mensagemErroDiabetes)]
        public TipoDiabetes Diabetico { get; set; }
        [Required]
        [IsValidEnumValue(typeof(TipoFumante), ErrorMessage = mensagemErroFumante)]
        public TipoFumante Fumante { get; set; }
        [IsValidEnumValue(typeof(TipoComorbidade), ErrorMessage = mensagemErroHistoricoAvc)]
        public TipoComorbidade HistoricoAvc { get; set; }
        [IsValidEnumValue(typeof(TipoComorbidade), ErrorMessage = mensagemErroDoencaRenalCronica)]
        public TipoComorbidade DoencaRenalCronica { get; set; }
        [IsValidEnumValue(typeof(TipoComorbidade), ErrorMessage = mensagemErroInsuficienciaCardiaca)]
        public TipoComorbidade InsuficienciaCardiaca { get; set; }
        [IsValidEnumValue(typeof(TipoComorbidade), ErrorMessage = mensagemErroHistoricoInfarto)]
        public TipoComorbidade HistoricoInfarto { get; set; }
        [IsValidEnumValue(typeof(TipoComorbidade), ErrorMessage = mensagemErroDoencaArterialObstrutivaPeriferica)]
        public TipoComorbidade DoencaArterialObstrutivaPeriferica { get; set; }
        [IsValidEnumValue(typeof(TipoComorbidade), ErrorMessage = mensagemErroRetinopatiaHipertensiva)]
        public TipoComorbidade RetinopatiaHipertensiva { get; set; }
    }
}
