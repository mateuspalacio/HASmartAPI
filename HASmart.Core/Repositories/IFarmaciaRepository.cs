using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HASmart.Core.Entities;


namespace HASmart.Core.Repositories {
    public interface IFarmaciaRepository {
        
         public Task<Farmacia> Cadastrar(Farmacia c);

          public Task<Farmacia> BuscarViaId(Guid id);

        //public Task<Operador> BuscarOperador(long id);

        //public Task<Farmacia> Cadastrar(Farmacia c);

        //public Task<Farmacia> BuscarViaId(long id);

        public Task<Farmacia> BuscarViaCNPJ(string CNPJ);

        public Task<bool> AlreadyExists(string cnpj);

        
    }
}
