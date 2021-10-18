using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HASmart.Core.Entities;


namespace HASmart.Core.Repositories {
    public interface ICidadaoRepository {
        public Task<IEnumerable<Cidadao>> BuscarTodos(long de, long para);
        public Task<Cidadao> BuscarViaId(Guid id);
        public Task<Cidadao> BuscarViaCpf(string cpf);
        public Task<Cidadao> BuscarViaRg(string rg);
        public Task<bool> AlreadyExists(string cpf, string rg);
        public Task<Cidadao> Cadastrar(Cidadao c);
        public Task<Cidadao> Atualizar(Cidadao c);
        public Task<Relatorio> Relatorio(Relatorio r);
    }
}
