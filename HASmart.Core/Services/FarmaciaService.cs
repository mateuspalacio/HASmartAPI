using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HASmart.Core.Architecture;
using HASmart.Core.Entities;
using HASmart.Core.Entities.DTOs;
using HASmart.Core.Exceptions;
using HASmart.Core.Extensions;
using HASmart.Core.Repositories;


namespace HASmart.Core.Services {
    public class FarmaciaService : IServiceBase<Farmacia> {
        private IFarmaciaRepository FarmaciaRepositorio { get; }
        private ICidadaoRepository CidadaoRepositorio { get; }
        private IMapper Mapper { get; }

        public FarmaciaService(IFarmaciaRepository farmaciaRepositorio, ICidadaoRepository cidadaoRepositorio, IMapper mapper) {
            this.FarmaciaRepositorio = farmaciaRepositorio;
            this.CidadaoRepositorio = cidadaoRepositorio;
            this.Mapper = mapper;
        }


        /*public async Task<Operador> GetOperador(string login, string senha) {
            Operador operador = null;
            if (login == "admin" && senha == "12345") { 
                operador = await this.FarmaciaRepositorio.BuscarOperador(1);
            }

            return operador ?? throw new EntityNotFoundException(typeof(Operador), "Operador não encontrado");
        }*/

        public async Task<Cidadao> RegistrarMedicao(Guid cidadaoId, MedicaoPostDTO dto) {
            dto.ThrowIfInvalid();

            Cidadao c = await this.CidadaoRepositorio.BuscarViaId(cidadaoId);
            if (c == null) {
                throw new EntityNotFoundException(typeof(Cidadao));
            }

            if (!c.PodeRealizarMedicao) {
                throw new EntityValidationException(typeof(Cidadao), "Permissão", "O cidadão não está habilitado para fazer uma medição no momento");
            }

            Medicao m = this.Mapper.Map<Medicao>(dto);
            m.CidadaoId = cidadaoId;
            m.DataHora = DateTime.Now;
            if (c.Medicoes != null)
                c.Medicoes.Add(m);
            else
            {
                c.Medicoes = new List<Medicao>();
                c.Medicoes.Add(m);
            }
                

            return await this.CidadaoRepositorio.Atualizar(c);
        }
        public async Task<Farmacia> CadastrarFarmacia(FarmaciaPostDTO dto) {
            dto.ThrowIfInvalid();

            Farmacia c = Mapper.Map<Farmacia>(dto);
            if (await this.FarmaciaRepositorio.AlreadyExists(c.Cnpj)) {
                throw new EntityValidationException(c.GetType(), "Farmacia", "Já existe uma farmacia com o mesmo CNPJ");
            }
            return await this.FarmaciaRepositorio.Cadastrar(c);
        }

        public async Task<Farmacia> BuscarViaId(Guid id) {
            return await this.FarmaciaRepositorio.BuscarViaId(id);   
        }

        public async Task<Farmacia> BuscarViaCNPJ(string cnpj){
            if (string.IsNullOrEmpty(cnpj) || !cnpj.ToCharArray().All(char.IsDigit) || cnpj.Length != 14) {
                throw new EntityValidationException(typeof(Cidadao), "CNPJ", "O CNPJ buscado não está na formatação adequada. CNPJs devem ser compostos por apenas 14 digitos numéricos.");
            }

            return await this.FarmaciaRepositorio.BuscarViaCNPJ(cnpj);
        }

        

        //public async Task<Farmacia> CadastrarFarmacia(FarmaciaPostDTO dto) {
        //    dto.ThrowIfInvalid();

        //    Farmacia c = Mapper.Map<Farmacia>(dto);
        //   /* if (await this.Repository.AlreadyExists(c.Cpf, c.Rg)) {
        //        throw new EntityValidationException(c.GetType(), "Cidadão", "Já existe um cidadão com o mesmo CPF ou RG");
        //    }*/
        //    return await this.FarmaciaRepositorio.Cadastrar(c);
        //}

        //public async Task<Farmacia> BuscarViaId(int id) {
        //    return await this.FarmaciaRepositorio.BuscarViaId(id);   
        //}

        /*public async Task<Cidadao> RegistrarDispencacao(long cidadaoId, DispencacaoPostDTO dto) {
            dto.ThrowIfInvalid();

            Cidadao c = await this.CidadaoRepositorio.BuscarViaId(cidadaoId);
            if (c == null) {
                throw new EntityNotFoundException(typeof(Cidadao));
            }

            if (!c.PodeRealizarDispencacao) {
                throw new EntityValidationException(typeof(Cidadao), "Permissão", "O cidadão não está habilitado para fazer uma dispensação no momento");
            }

            Medicao m = c.Medicoes.FirstOrDefault(x => x.Id == dto.MedicaoId);
            if (m == null) {
                throw new EntityNotFoundException(typeof(Medicao));
            }

            Dispencacao d = this.Mapper.Map<Dispencacao>(dto);
            d.CidadaoId = cidadaoId;
            d.DataHora = DateTime.Now;
            c.Dispencacoes.Add(d);
            return await this.CidadaoRepositorio.Atualizar(c);
        }*/
    }
}
