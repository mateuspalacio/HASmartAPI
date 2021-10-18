using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using HASmart.Core.Architecture;
using HASmart.Core.Extensions;
using HASmart.Core.Validation;
using System.Text.Json.Serialization;

namespace HASmart.Core.Entities {
    public class Cidadao : AggregateRoot {
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Rg { get; set; }
        
        [DataType(DataType.Date)]
        [DefaultValue("01/01/2000")]
        public DateTime DataNascimento { get; set; }

        [DataType(DataType.Date)]
        [DefaultValue("01/01/2000")]
        public DateTime DataCadastro { get; set; } = DateTime.Now.Date;
        public DadosPessoais DadosPessoais { get; set; }
        public IndicadorRiscoHAS IndicadorRiscoHAS { get; set; }
        public List<Medicao> Medicoes { get; set; }
        [JsonIgnore]
        public Medico medicoAtual { get; set; }
        public List<Relatorio> Relatorios { get; set; }
        //[JsonIgnore]
        //public List<Medico> medicosAtendeu { get; set; }
        
        //public List<Dispencacao> Dispencacoes { get; set; }
        public bool PodeRealizarMedicao => DateTime.Now >= DataProximaMedicao;

        
        //public DateTime DataProximaMedicao => DateTime.Now >= DataUltimaMedicao ? DateTime.Now : DataUltimaMedicao;

        
        [DataType(DataType.Date)]
        [DefaultValue("01/01/2000")]
        public DateTime DataProximaMedicao {
            get{
                if(DateTime.Now.Date > DataUltimaMedicao){
                    return DateTime.Now.Date;
                }else{
                    return DataUltimaMedicao.AddDays(0);
                }
            }
        }
        public DateTime DataUltimaMedicao {
            get {
                if (Medicoes == null || Medicoes.Count == 0) {
                    return DateTime.MinValue;
                }
                return Medicoes[^1].DataHora;
            }
        }

        /*public bool PodeRealizarDispencacao => DateTime.Today > DataUltimaDispencacao;
        
        [DataType(DataType.Date)]
        [DefaultValue("00/00/0000")]
        public DateTime DataProximaDispencacao {
            get{
                if(DateTime.Today > DataUltimaDispencacao){
                    return DateTime.Today;
                }else{
                    return DataUltimaDispencacao.AddDays(1);
                }
            }
        }
        
        public DateTime DataUltimaDispencacao {
            get {
                if (Dispencacoes == null || Dispencacoes.Count == 0) {
                    return DateTime.MinValue;
                }

                return Dispencacoes[^1].DataHora;
            }
        }*/
        
    }
}