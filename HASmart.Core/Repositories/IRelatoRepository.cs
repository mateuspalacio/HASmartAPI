using HASmart.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HASmart.Core.Repositories
{
    public interface IRelatoRepository
    {
        public Task<Relatorio> CriarRelato(Relatorio r);
        public Task<Relatorio> LerRelato(Guid id, Guid cidadaoId);
        public Task<List<Relatorio>> LerRelatos(Guid cidadaoId);
        public Task<Relatorio> AtualizarRelato(Relatorio r);
        public Task<Relatorio> ApagarRelato(Relatorio r);


    }
}
