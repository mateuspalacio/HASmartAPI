using System;
using System.Threading.Tasks;
using HASmart.Core.Entities;

namespace HASmart.Core.Repositories
{
    public interface IMedicoRepository
    {
        public Task<Medico> BuscarViaId(Guid id);
        public Task<Medico> BuscarViaCrm(string crm);
        public Task<bool> AlreadyExists(string crm);
        public Task<Medico> Cadastrar(Medico m);
        public Task<Medico> Atualizar(Medico m);
        public Task<Medico> GetOperador(Medico o);

    }
}