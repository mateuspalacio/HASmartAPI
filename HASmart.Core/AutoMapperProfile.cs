using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using HASmart.Core.Entities;
using HASmart.Core.Entities.DTOs;


namespace HASmart.Core {
    public class AutoMapperProfile : Profile {
        public AutoMapperProfile() {
            this.CreateMap<EnderecoPostDTO, Endereco>(MemberList.Source);
            this.CreateMap<FarmaciaPostDTO, Farmacia>(MemberList.Source);
            this.CreateMap<DadosPessoaisDTO, DadosPessoais>(MemberList.Source);
            this.CreateMap<FarmaciaPostDTO, Farmacia>(MemberList.Source);
            this.CreateMap<EnderecoFarmaciaPostDTO, EnderecoFarmacia>(MemberList.Source);
            this.CreateMap<IndicadorRiscoHASDTO, IndicadorRiscoHAS>(MemberList.Source);
            this.CreateMap<CidadaoPostDTO, Cidadao>(MemberList.Source);
            this.CreateMap<AfericaoPostDTO, Afericao>(MemberList.Source);
            this.CreateMap<MedicaoPostDTO, Medicao>(MemberList.Source);
            this.CreateMap<MedicamentoPostDTO, Medicamento>(MemberList.Source);
            this.CreateMap<RegistroPostDTO, Registro>(MemberList.Source);
            this.CreateMap<MedicoPostDTO, Medico>(MemberList.Source);
            this.CreateMap<MedicoOperadorPutDto, Medico>(MemberList.Source);
            this.CreateMap<RelatorioPostDTO, Relatorio>(MemberList.Source);
            //this.CreateMap<DispencacaoPostDTO, Dispencacao>(MemberList.Source);
        }
    }
}
