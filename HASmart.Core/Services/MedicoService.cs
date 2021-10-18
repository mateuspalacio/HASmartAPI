using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HASmart.Core.Entities;
using HASmart.Core.Entities.DTOs;
using HASmart.Core.Exceptions;
using HASmart.Core.Extensions;
using HASmart.Core.Repositories;
using System.Collections.Generic;
using System;

namespace HASmart.Core.Services
{
    public class MedicoService : IServiceBase<Medico>
    {
        public IMedicoRepository MedicoRepository { get; }
        public CidadaoService CidadaoService { get; }
        public IMapper Mapper { get; }

        public MedicoService(IMedicoRepository medicoRepository, IMapper mapper, CidadaoService cidadaoService){
            this.MedicoRepository = medicoRepository;
            this.Mapper = mapper;
            this.CidadaoService = cidadaoService;
        }
        public async Task<Medico> BuscarViaId(Guid id) {
            return await this.MedicoRepository.BuscarViaId(id);
        }
        public async Task<Medico> BuscarViaCrm(string crm) {
            if (string.IsNullOrEmpty(crm) || !crm.ToCharArray().All(char.IsDigit)) {
                throw new EntityValidationException(typeof(Medico), "CRM", "O CRM buscado não está na formatação adequada. CRMs devem ser compostos por apenas números.");
            }

            return await this.MedicoRepository.BuscarViaCrm(crm);
        }

        public async Task<Medico> CadastrarMedico(MedicoPostDTO dto) {
            dto.ThrowIfInvalid();

            Medico m = Mapper.Map<Medico>(dto);
            if (await this.MedicoRepository.AlreadyExists(m.Crm)) {
                throw new EntityValidationException(m.GetType(), "Medico", "Já existe um medico com o mesmo CRM");
            }
            return await this.MedicoRepository.Cadastrar(m);
        }

        public async Task<Medico> AdicionarCidadaos(Guid id,string[] cpfs){
            Medico m = await this.MedicoRepository.BuscarViaId(id);
            List<Cidadao> cidadaos = new List<Cidadao>();
            foreach(string cpf in cpfs){
                if (await this.CidadaoService.CidadaoRepositorio.AlreadyExists(cpf,cpf)) {
                    Cidadao c = await this.CidadaoService.BuscarViaCpf(cpf);
                    bool conf = true;
                    if(m.cidadaosAtuais != null) {
                        foreach(Cidadao cidadao in m.cidadaosAtuais) {
                            if(c == cidadao){
                                conf = false;
                            }
                        }
                        if(conf){
                            m.cidadaosAtuais.Add(c);
                            //if(c.medicoAtual != null) {
                            //    c.medicosAtendeu.Add(c.medicoAtual);
                            //}
                            c.medicoAtual = m;
                        }
                    } else {
                        if(conf){
                            cidadaos.Add(c);
                            //if(c.medicoAtual != null) {
                            //    c.medicosAtendeu.Add(c.medicoAtual);
                            //}
                            c.medicoAtual = m;
                        }
                    }
                } else {
                    throw new EntityNotFoundException(typeof(Cidadao));
                }
            }
            if(m.cidadaosAtuais == null){
                m.cidadaosAtuais = cidadaos;
            }
            return await this.MedicoRepository.Atualizar(m);
        }
        public async Task<Medico> BuscarOperador(Medico o)
        {
            if (string.IsNullOrEmpty(o.Nome))
            {
                throw new EntityNotFoundException(typeof(Operador));
            }

            return await MedicoRepository.GetOperador(o);
        }
    }
}