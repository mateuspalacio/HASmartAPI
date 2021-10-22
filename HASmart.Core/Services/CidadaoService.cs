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
using Microsoft.AspNetCore.Http;
using CsvHelper.Configuration;

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
            c.AnonimoNome = await AnonimizarNome(c.Nome, c.Cpf, c.DataNascimento);
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
        public async Task<List<Cidadao>> BuscarCidadaosPorNome(string name)
        {
            if(string.IsNullOrEmpty(name))
                throw new EntityValidationException(typeof(Cidadao), "Nome", "O nome para busca não pode ser vazio");
            var c = await CidadaoRepositorio.BuscarPorNome(name);
            return c;
        }
        public async Task<List<Cidadao>> BuscarCidadaosPorNomeAnonimo(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new EntityValidationException(typeof(Cidadao), "Nome Anônimo", "O nome anônimo para busca não pode ser vazio");
            var c = await CidadaoRepositorio.BuscarPorNomeAnonimo(name);
            return c;
        }
        public async Task<Cidadao> ApagarCidadao(Guid id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
                throw new EntityValidationException(typeof(Cidadao), "Id", "O Id para apagar não pode ser vazio");

            Cidadao c = await BuscarViaId(id);

            var apagado = await CidadaoRepositorio.ApagarCidadao(c);
            return apagado;
        }
        public async Task<Cidadao> AnonimizarNome(Guid id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
                throw new EntityValidationException(typeof(Cidadao), "Id", "O Id para anonimizar não pode ser vazio");

            Cidadao c = await BuscarViaId(id);
            if (!string.IsNullOrEmpty(c.AnonimoNome))
                throw new EntityAlreadyCreatedException(typeof(Cidadao), "Este cidadão já possui um nome anônimo");

            char[] charsNome = c.Nome.ToCharArray();
            char[] charsCpf = c.Cpf.ToCharArray();

            string nomeCodigoAnonimo = $"{c.DataCadastro.Year}{c.DataCadastro.Hour}{charsNome[0]}{charsCpf[3]}{c.DataCadastro.Minute}{charsNome[charsNome.Length - 1]}{charsCpf[charsCpf.Length - 1]}{DateTime.Now.Millisecond * 1000}HASmart{Convert.ToChar(DateTime.Now.Day)}{Convert.ToChar(c.DataNascimento.Day)}";
            c.AnonimoNome = nomeCodigoAnonimo;

            var update = await CidadaoRepositorio.Atualizar(c);
            return update;

        }
        // este método é pra o próprio sistema anonimizar o cidadao, nao deve ser usado por usuarios
        public async Task<string> AnonimizarNome(string nome, string cpf, DateTime dataNascimento)
        {
            //if (string.IsNullOrEmpty(id.ToString()))
            //    throw new EntityValidationException(typeof(Cidadao), "Id", "O Id para anonimizar não pode ser vazio");

            //Cidadao c = await BuscarViaId(id);
            //if (!string.IsNullOrEmpty(c.AnonimoNome))
            //    throw new EntityAlreadyCreatedException(typeof(Cidadao), "Este cidadão já possui um nome anônimo");

            char[] charsNome = nome.ToCharArray();
            char[] charsCpf = cpf.ToCharArray();

            string nomeCodigoAnonimo = $"{DateTime.Now.Year}{DateTime.Now.Hour}{charsNome[0]}{charsCpf[3]}{DateTime.Now.Minute}{charsNome[charsNome.Length - 1]}{charsCpf[charsCpf.Length - 1]}{DateTime.Now.Millisecond * 1000}HASmart{Convert.ToChar(DateTime.Now.Day)}{Convert.ToChar(dataNascimento.Day)}";

            return nomeCodigoAnonimo;

        }
        public async Task<IEnumerable<Cidadao>> RegistroComArquivo(IFormFile file)
        {
            
            var config = new CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture) { Delimiter = DetectDelimiter(new StreamReader(file.OpenReadStream())), Encoding = Encoding.UTF8 };
            using (var reader = new StreamReader(file.OpenReadStream()))
                using(var csvReader = new CsvReader(reader, config))
                {

                var records = new List<Cidadao>();
                    
                    csvReader.Read();
                    csvReader.ReadHeader();
                    while (csvReader.Read())
                    {
                    var cpfNovo = "0";
                    if (csvReader.GetField("cpf").Length == 10)
                    {
                        cpfNovo += csvReader.GetField("cpf");
                    }
                    else
                    {
                        cpfNovo = csvReader.GetField("cpf");
                    }
                    var cpf = csvReader.GetField("cpf");
                        var rg = csvReader.GetField("rg");
                        int index = csvReader.GetField("dataHora").IndexOf(" "); // gets index of first occurrence of blank space, which in this case separates the date from the time.

                    if (await this.CidadaoRepositorio.AlreadyExists(cpfNovo, rg))
                    {
                        DateTime dtMedicao;
                        if (DateTime.TryParseExact(csvReader.GetField("dataCadastro"), "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None,
                            out dtMedicao
                            ))
                        {
                            dtMedicao = DateTime.ParseExact((DateTime.ParseExact(csvReader.GetField("dataHora"), "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture)).Substring(0, index), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        } else if (DateTime.TryParseExact(csvReader.GetField("dataCadastro"), "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None,
                            out dtMedicao
                            ))
                        {
                            dtMedicao = DateTime.ParseExact((DateTime.ParseExact(csvReader.GetField("dataHora"), "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture)).Substring(0, index), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            dtMedicao = DateTime.ParseExact((DateTime.ParseExact(csvReader.GetField("dataHora"), "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture)).Substring(0, index), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture); ;
                        }
                        Cidadao cidadao = await BuscarViaCpf(cpfNovo);
                            List<AfericaoPostDTO> listAfe = new List<AfericaoPostDTO>();
                            for (int i = 0; i <= 1; i++)
                            {
                                if (!string.IsNullOrEmpty(csvReader.GetField($"medicoes.afericoes.{i}.sistolica")) && !string.IsNullOrEmpty(csvReader.GetField($"medicoes.afericoes.{i}.diastolica")))
                                {
                                    var med = new AfericaoPostDTO { Sistolica = UInt32.Parse(csvReader.GetField($"medicoes.afericoes.{i}.sistolica")), Diastolica = UInt32.Parse(csvReader.GetField($"medicoes.afericoes.{i}.diastolica")) };
                                    listAfe.Add(med);
                                }
                            }
                        float peso = 0;
                        if (!String.IsNullOrEmpty(csvReader.GetField("medicoes.peso")))
                        {
                            peso = float.Parse(csvReader.GetField("medicoes.peso"), System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
                        }
                        MedicaoPostDTO medidto = new MedicaoPostDTO()
                        {

                            Afericoes = listAfe,
                            Peso = peso,
                            Medicamentos = new List<MedicamentoPostDTO> { new MedicamentoPostDTO { Nome = csvReader.GetField("medicoes.medicamentos") } }

                            };
                            Cidadao d = await FarmaciaService.RegistrarMedicao(cidadao.Id, medidto);
                            var x = d.Medicoes.Count();
                            d.Medicoes[x - 1].DataHora = dtMedicao;
                            await this.CidadaoRepositorio.Atualizar(d);
                            records.Add(d);
                        
                        
                        }
                        else 
                        {
                        cpfNovo = "0";
                        float altura = 0;
                        DateTime dtCadastro;
                        DateTime dtMedicao;
                            if(csvReader.GetField("cpf").Length == 10)
                        {
                            cpfNovo += csvReader.GetField("cpf");
                        } else
                        {
                            cpfNovo = csvReader.GetField("cpf");
                        }
                        if (!String.IsNullOrEmpty(csvReader.GetField("indicadorRiscoHAS.altura")))
                        {
                            altura = float.Parse(csvReader.GetField("indicadorRiscoHAS.altura"), System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
                        }
                        if (DateTime.TryParseExact(csvReader.GetField("dataCadastro"), "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None,
                            out dtCadastro
                            ))
                        {
                            dtCadastro = DateTime.ParseExact(DateTime.ParseExact(csvReader.GetField("dataCadastro"), "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).Substring(0, index), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                            dtMedicao = DateTime.ParseExact((DateTime.ParseExact(csvReader.GetField("dataHora"), "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture)).Substring(0, index), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        } else
                        {
                            dtCadastro = DateTime.ParseExact(DateTime.ParseExact(csvReader.GetField("dataCadastro"), "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture).Substring(0, index), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                            dtMedicao = DateTime.ParseExact((DateTime.ParseExact(csvReader.GetField("dataHora"), "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture)).Substring(0, index), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture); ;
                        }
                            var record = new Cidadao()
                            {
                                Nome = csvReader.GetField("nome"),
                                Cpf = cpfNovo,
                                Rg = csvReader.GetField("rg"),
                                DataNascimento = DateTime.ParseExact(csvReader.GetField("dataNascimento"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture),
                                DataCadastro = dtCadastro,
                                DadosPessoais = new DadosPessoais()
                                {
                                    Email = csvReader.GetField("dadosPessoais.email"),
                                    Telefone = csvReader.GetField("dadosPessoais.telefone").Replace("(", "").Replace(")", "").Replace(" ", "").Replace("-",""),
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
                                    Altura = altura,
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
                        List<AfericaoPostDTO> listAfe = new List<AfericaoPostDTO>();
                        for (int i = 0; i <= 1; i++)
                        {
                            if(!string.IsNullOrEmpty(csvReader.GetField($"medicoes.afericoes.{i}.sistolica")) && !string.IsNullOrEmpty(csvReader.GetField($"medicoes.afericoes.{i}.diastolica")))
                            {
                                var med = new AfericaoPostDTO { Sistolica = UInt32.Parse(csvReader.GetField($"medicoes.afericoes.{i}.sistolica")), Diastolica = UInt32.Parse(csvReader.GetField($"medicoes.afericoes.{i}.diastolica")) };
                                listAfe.Add(med);
                            } 
                        }
                        float peso = 0;
                        if (!String.IsNullOrEmpty(csvReader.GetField("medicoes.peso")))
                        {
                            peso = float.Parse(csvReader.GetField("medicoes.peso"), System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
                        }
                        MedicaoPostDTO medidto = new MedicaoPostDTO()
                            {
                               
                                Afericoes = listAfe,
                                Peso = peso,
                                Medicamentos = new List<MedicamentoPostDTO> { new MedicamentoPostDTO { Nome = csvReader.GetField("medicoes.medicamentos") } }

                            };
                            Cidadao d = await FarmaciaService.RegistrarMedicao(record.Id, medidto);
                            var x = d.Medicoes.Count();
                            d.Medicoes[x - 1].DataHora = dtMedicao;
                            await this.CidadaoRepositorio.Atualizar(d);
                            records.Add(d);
                        }

                    }
                    return records;
                }
                // TextReader reader = new StreamReader();
            
            //csvReader.Context.RegisterClassMap<CidadaoMap>();
            
            
        }
        public static string DetectDelimiter(StreamReader reader)
        {
            // assume one of following delimiters
            var possibleDelimiters = new List<string> { ",", ";", "\t", "|" };

            var headerLine = reader.ReadLine();

            // reset the reader to initial position for outside reuse
            // Eg. Csv helper won't find header line, because it has been read in the Reader
            reader.BaseStream.Position = 0;
            reader.DiscardBufferedData();

            foreach (var possibleDelimiter in possibleDelimiters)
            {
                if (headerLine.Contains(possibleDelimiter))
                {
                    return possibleDelimiter;
                }
            }

            return possibleDelimiters[0];
        }
    }
}
