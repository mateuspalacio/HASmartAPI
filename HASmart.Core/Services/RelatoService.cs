using AutoMapper;
using HASmart.Core.Entities;
using HASmart.Core.Entities.DTOs;
using HASmart.Core.Extensions;
using HASmart.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HASmart.Core.Services
{
    public class RelatoService : IServiceBase<Relatorio>
    {
        public ICidadaoRepository CidadaoRepositorio { get; }
        private IRelatoRepository _relatoRep { get; }

        public IMapper Mapper { get; }

        public RelatoService(ICidadaoRepository cidadaoRepositorio, IRelatoRepository relatoRep, IMapper mapper)
        {
            this.CidadaoRepositorio = cidadaoRepositorio;
            _relatoRep = relatoRep;
            this.Mapper = mapper;
        }
        public async Task<Relatorio> CadastrarRelato(Guid id, RelatorioPostDTO dto)
        {
            dto.ThrowIfInvalid();

            Relatorio r = Mapper.Map<Relatorio>(dto);
            r.CidadaoId = id;
            await _relatoRep.CriarRelato(r);
            return r;
        }
        public async Task<Relatorio> LerRelato(Guid id, Guid cidadaoId)
        {
            var r = await _relatoRep.LerRelato(id, cidadaoId);
            return r;
        }
        public async Task<List<Relatorio>> LerRelatos(Guid cidadaoId)
        {
            var r = await this._relatoRep.LerRelatos(cidadaoId);
            return r;
        }
        public async Task<Relatorio> ApagarRelato(Guid id, Guid cidadaoId)
        {
            var delete = await LerRelato(id, cidadaoId);
            var r = await this._relatoRep.ApagarRelato(delete);
            return r;
        }
        public async Task<Relatorio> AtualizarRelato(Guid id, Guid cidadaoId, RelatorioPostDTO dto)
        {
            dto.ThrowIfInvalid();

            Relatorio r = Mapper.Map<Relatorio>(dto);
            r.CidadaoId = cidadaoId;
            r.Id = id;
            await this._relatoRep.AtualizarRelato(r);
            return r;
        }
    }
}
