using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
using System.IO;
using System.Text.Json;
using CsvHelper;

namespace HASmart.Core.Services {
    public class CidadaoService : IServiceBase<Cidadao> {
        public ICidadaoRepository CidadaoRepositorio { get; }
        private IFarmaciaRepository FarmaciaRepositorio { get; }
        private FarmaciaService FarmaciaService{ get; set;}

        public IMapper Mapper { get; }

        public CidadaoService(IFarmaciaRepository farmaciaRepositorio, ICidadaoRepository cidadaoRepositorio,FarmaciaService FarmaciaService, IMapper mapper) {
            this.FarmaciaRepositorio = farmaciaRepositorio;
            this.CidadaoRepositorio = cidadaoRepositorio;
            this.FarmaciaService = FarmaciaService;
            this.Mapper = mapper;
        }
        

        public async Task<IEnumerable<Cidadao>> BuscarTodos(long de, long para) {
            return await this.CidadaoRepositorio.BuscarTodos(de, para);
        }
        public async Task<Cidadao> BuscarViaId(Guid id) {
            return await this.CidadaoRepositorio.BuscarViaId(id);
        }
        public async Task<Cidadao> BuscarViaCpf(string cpf) {
            if (string.IsNullOrEmpty(cpf) || !cpf.ToCharArray().All(char.IsDigit) || cpf.Length != 11) {
                throw new EntityValidationException(typeof(Cidadao), "CPF", "O CPF buscado não está na formatação adequada. CPFs devem ser compostos por apenas 11 digitos numéricos.");
            }

            return await this.CidadaoRepositorio.BuscarViaCpf(cpf);
        }
        public async Task<Cidadao> BuscarViaRg(string rg) {
            if (string.IsNullOrEmpty(rg) || !rg.ToCharArray().All(char.IsDigit)) {
                throw new EntityValidationException(typeof(Cidadao), "RG", "O RG buscado não está na formatação adequada. RGs devem ser compostos por apenas números.");
            }

            return await this.CidadaoRepositorio.BuscarViaRg(rg);
        }

        public async Task<Cidadao> CadastrarCidadao(CidadaoPostDTO dto) {
            dto.ThrowIfInvalid();

            Cidadao c = Mapper.Map<Cidadao>(dto);
            if (await this.CidadaoRepositorio.AlreadyExists(c.Cpf, c.Rg)) {
                throw new EntityValidationException(c.GetType(), "Cidadão", "Já existe um cidadão com o mesmo CPF ou RG");
            }
            return await this.CidadaoRepositorio.Cadastrar(c);
        }

        public async Task<Cidadao> AtualizarCidadao(Guid id, CidadaoPutDTO dto) {
            Cidadao c = await this.BuscarViaId(id);
            if (c == null) {
                throw new EntityNotFoundException(c.GetType());
            }

            dto.ThrowIfInvalid();
            c.DadosPessoais = this.Mapper.Map<DadosPessoais>(dto.DadosPessoais);
            c.IndicadorRiscoHAS = this.Mapper.Map<IndicadorRiscoHAS>(dto.IndicadorRiscoHAS);
            return await this.CidadaoRepositorio.Atualizar(c);
        }

        public async Task<Cidadao> CadastrarCidadaoEMedicao(RegistroPostDTO dto) {
            dto.ThrowIfInvalid();

            Cidadao c = Mapper.Map<Cidadao>(dto.cidadaoPostDTO);
            if (await this.CidadaoRepositorio.AlreadyExists(c.Cpf, c.Rg)) {
                Cidadao cidadao = await BuscarViaRg(c.Rg);
                Cidadao d = await FarmaciaService.RegistrarMedicao(cidadao.Id, dto.MedicaoPostDTO);
                return d;
            }
            await this.CidadaoRepositorio.Cadastrar(c);
            Cidadao cid = await FarmaciaService.RegistrarMedicao(c.Id, dto.MedicaoPostDTO);
            return cid;
        }

        public async Task<Relatorio> CadastrarRelatorio(Guid id, RelatorioPostDTO dto)
        {
            dto.ThrowIfInvalid();

            Relatorio r = Mapper.Map<Relatorio>(dto);
            r.CidadaoId = id;
            await this.CidadaoRepositorio.Relatorio(r);
            return r;
        }
        public async Task<IEnumerable<Cidadao>> RegistroCSV(string url)
        {
            TextReader reader = new StreamReader(url);
            var csvReader = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture);
            // csvReader.Context.RegisterClassMap<CidadaoMap>();
            var records = new List<Cidadao>();
            csvReader.Read();
            csvReader.ReadHeader();
            while (csvReader.Read())
            {
                var cpf = csvReader.GetField("cpf");
                var rg = csvReader.GetField("rg");
                int index = csvReader.GetField("dataHora").IndexOf(" "); // gets index of first occurrence of blank space, which in this case separates the date from the time.

                if (await this.CidadaoRepositorio.AlreadyExists(cpf, rg))
                {

                    Cidadao cidadao = await BuscarViaCpf(cpf);
                    MedicaoPostDTO medidto = new MedicaoPostDTO()
                    {
                        Afericoes = new List<AfericaoPostDTO> { new AfericaoPostDTO { Sistolica = UInt32.Parse(csvReader.GetField("medicoes.afericoes.sistolica")), Diastolica = UInt32.Parse(csvReader.GetField("medicoes.afericoes.diastolica")) }, new AfericaoPostDTO { Sistolica = UInt32.Parse(csvReader.GetField("medicoes.afericoes.sistolica")), Diastolica = UInt32.Parse(csvReader.GetField("medicoes.afericoes.diastolica")) } },
                        Peso = float.Parse(csvReader.GetField("medicoes.peso"), System.Globalization.CultureInfo.InvariantCulture.NumberFormat),
                        Medicamentos = new List<MedicamentoPostDTO> { new MedicamentoPostDTO { Nome = csvReader.GetField("medicoes.medicamentos") } }

                    };
                    Cidadao d = await FarmaciaService.RegistrarMedicao(cidadao.Id, medidto);
                    var x = d.Medicoes.Count();
                    d.Medicoes[x - 1].DataHora = DateTime.ParseExact((DateTime.ParseExact(csvReader.GetField("dataHora"), "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture)).Substring(0, index), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    await this.CidadaoRepositorio.Atualizar(d);
                    records.Add(d);
                }
                else
                {

                    var record = new Cidadao()
                    {
                        Nome = csvReader.GetField("nome"),
                        Cpf = csvReader.GetField("cpf"),
                        Rg = csvReader.GetField("rg"),
                        DataNascimento = DateTime.ParseExact(DateTime.ParseExact(csvReader.GetField("dataNascimento"), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture),
                        DataCadastro = DateTime.ParseExact((DateTime.ParseExact(csvReader.GetField("dataCadastro"), "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture)).Substring(0, index), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture),
                        DadosPessoais = new DadosPessoais()
                        {
                            Email = csvReader.GetField("dadosPessoais.email"),
                            Telefone = csvReader.GetField("dadosPessoais.telefone"),
                            Genero = csvReader.GetField("dadosPessoais.genero"),
                            Endereco = new Endereco()
                            {
                                CEP = csvReader.GetField("dadosPessoais.endereco.cep"),
                                Rua = csvReader.GetField("dadosPessoais.endereco.rua"),
                                Numero = csvReader.GetField("dadosPessoais.endereco.numero"),
                                Complemento = csvReader.GetField("dadosPessoais.endereco.complemento"),
                                Cidade = csvReader.GetField("dadosPessoais.endereco.cidade"),
                                Estado = csvReader.GetField("dadosPessoais.endereco.estado")
                            }
                        },
                        IndicadorRiscoHAS = new IndicadorRiscoHAS()
                        {
                            Altura = float.Parse(csvReader.GetField("indicadorRiscoHAS.altura"), System.Globalization.CultureInfo.InvariantCulture.NumberFormat),
                            Diabetico = (TipoDiabetes)Enum.Parse(typeof(TipoDiabetes), csvReader.GetField("indicadorRiscoHAS.diabetico")),
                            Fumante = (TipoFumante)Enum.Parse(typeof(TipoFumante), csvReader.GetField("indicadorRiscoHAS.fumante")),
                            DoencaRenalCronica = (TipoComorbidade)Enum.Parse(typeof(TipoComorbidade), csvReader.GetField("indicadorRiscoHAS.doencaRenalCronica")),
                            InsuficienciaCardiaca = (TipoComorbidade)Enum.Parse(typeof(TipoComorbidade), csvReader.GetField("indicadorRiscoHAS.insuficienciaCardiaca")),
                            DoencaArterialObstrutivaPeriferica = (TipoComorbidade)Enum.Parse(typeof(TipoComorbidade), csvReader.GetField("indicadorRiscoHAS.doencaArterialObstrutivaPeriferica")),
                            HistoricoAvc = (TipoComorbidade)Enum.Parse(typeof(TipoComorbidade), csvReader.GetField("indicadorRiscoHAS.historicoAVC")),
                            RetinopatiaHipertensiva = (TipoComorbidade)Enum.Parse(typeof(TipoComorbidade), csvReader.GetField("indicadorRiscoHAS.retinopatiaHipertensiva")),

                        }

                    };
                    await this.CidadaoRepositorio.Cadastrar(record);
                    MedicaoPostDTO medidto = new MedicaoPostDTO()
                    {
                        Afericoes = new List<AfericaoPostDTO> { new AfericaoPostDTO { Sistolica = UInt32.Parse(csvReader.GetField("medicoes.afericoes.sistolica")), Diastolica = UInt32.Parse(csvReader.GetField("medicoes.afericoes.diastolica")) }, new AfericaoPostDTO { Sistolica = UInt32.Parse(csvReader.GetField("medicoes.afericoes.sistolica")), Diastolica = UInt32.Parse(csvReader.GetField("medicoes.afericoes.diastolica")) } },
                        Peso = float.Parse(csvReader.GetField("medicoes.peso"), System.Globalization.CultureInfo.InvariantCulture.NumberFormat),
                        Medicamentos = new List<MedicamentoPostDTO> { new MedicamentoPostDTO { Nome = csvReader.GetField("medicoes.medicamentos") } }

                    };
                    Cidadao d = await FarmaciaService.RegistrarMedicao(record.Id, medidto);
                    var x = d.Medicoes.Count();
                    d.Medicoes[x - 1].DataHora = DateTime.ParseExact((DateTime.ParseExact(csvReader.GetField("dataHora"), "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture)).Substring(0, index), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    await this.CidadaoRepositorio.Atualizar(d);
                    records.Add(d);
                }

            }
            return records;

        }
    }
}
