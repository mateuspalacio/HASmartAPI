using CsvHelper.Configuration;


namespace HASmart.Core.Entities
{

    public sealed class CidadaoMap : ClassMap<Cidadao>
    {

        public CidadaoMap()
        {
            Map(m => m.Nome).Name("nome");
            Map(m => m.Cpf).Name("cpf");
            Map(m => m.Rg).Name("rg");
            Map(m => m.DadosPessoais.Email).Name("dadosPessoais.email");
            Map(m => m.DadosPessoais.Telefone).Name("dadosPessoais.telefone");
            Map(m => m.DadosPessoais.Genero).Name("dadosPessoais.genero");
            Map(m => m.DadosPessoais.Endereco.CEP).Name("dadosPessoais.endereço.cep");
            Map(m => m.DadosPessoais.Endereco.Rua).Name("dadosPessoais.endereço.rua");
            Map(m => m.DadosPessoais.Endereco.Numero).Name("dadosPessoais.endereço.numero");
            Map(m => m.DadosPessoais.Endereco.Complemento).Name("dadosPessoais.endereço.complemento");
            Map(m => m.DadosPessoais.Endereco.Cidade).Name("dadosPessoais.endereço.cidade");
            Map(m => m.DadosPessoais.Endereco.Estado).Name("dadosPessoais.endereço.estado");
            Map(m => m.IndicadorRiscoHAS.Altura).Name("indicadorRiscoHAS.altura");
            Map(m => m.IndicadorRiscoHAS.Diabetico).Name("indicadorRiscoHAS.diabetico");
            Map(m => m.IndicadorRiscoHAS.HistoricoAvc).Name("indicadorRiscoHAS.historicoAvc");
            Map(m => m.IndicadorRiscoHAS.DoencaRenalCronica).Name("indicadorRiscoHAS.doencaRenalCronica");
            Map(m => m.IndicadorRiscoHAS.InsuficienciaCardiaca).Name("indicadorRiscoHAS.insuficienciaCardiaca");
            Map(m => m.IndicadorRiscoHAS.DoencaArterialObstrutivaPeriferica).Name("indicadorRiscoHAS.doencaArterialObstrutivaPeriferica");
            Map(m => m.IndicadorRiscoHAS.RetinopatiaHipertensiva).Name("indicadorRiscoHAS.retinopatiaHipertensiva");
        }
    }
}