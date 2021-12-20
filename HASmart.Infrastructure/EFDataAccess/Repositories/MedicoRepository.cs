using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using HASmart.Core.Architecture;
using HASmart.Core.Entities;
using HASmart.Core.Exceptions;
using HASmart.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HASmart.Infrastructure.EFDataAccess.Repositories
{
    public class MedicoRepository : IMedicoRepository
    {
        private AppDBContext Context  { get; }
        public MedicoRepository(AppDBContext context) { 
            this.Context = context;
        }

        public async Task<Medico> BuscarViaId(Guid id) {
            Medico m = await this.Context.Medicos.FirstOrDefaultAsync(x => x.Id == id);
            return m ?? throw new EntityNotFoundException(typeof(Medico));
        }   
        public async Task<Medico> BuscarViaCrm(string crm) {
            Medico m = await this.Context.Medicos.FirstOrDefaultAsync(x => x.Crm == crm);
            return m ?? throw new EntityNotFoundException(typeof(Medico));
        }

        public async Task<bool> AlreadyExists(string crm) {
            return await this.Context.Medicos.AnyAsync(x => x.Crm == crm);
        }

        public async Task<Medico> Cadastrar(Medico m) {
            this.Context.Medicos.Add(m);
            await this.Context.SaveChangesAsync();
            return m;
        }

        public async Task<Medico> Atualizar(Medico m) {
            this.Context.Entry(m).State = EntityState.Modified;
            
            try {
                await this.Context.SaveChangesAsync();
                return m;
            } catch (Exception) {
                if (! await this.Context.Medicos.AnyAsync(e => e.Id == m.Id)) {
                    throw new EntityNotFoundException(typeof(Medico));
                } else {
                    throw new EntityConcurrencyException(typeof(Medico));
                }
            }
        }
        public async Task<Medico> GetOperador(Medico r)
        {
            Medico o = await Context.Medicos.FirstOrDefaultAsync(x => x.Nome == r.Nome && x.Senha == r.Senha);
            if(o is null)
                throw new EntityNotFoundException(typeof(Medico));
            o.cidadaosAtuais = await Context.Cidadaos.Where(x => x.medicoAtual.Id == o.Id).ToListAsync();
            foreach (var cit in o.cidadaosAtuais)
            {
                cit.IndicadorRiscoHAS = await Context.IndicadorRiscos
                .FirstOrDefaultAsync(ris => ris.Cidadao_Id == cit.Id);

                cit.Medicoes = await Context.Medicoes
                    .Where(med => med.CidadaoId == cit.Id).ToListAsync();

                foreach (var med in cit.Medicoes)
                {
                    med.Afericoes = await Context.Afericao
                    .Where(afe => afe.MedicaoId == med.Id).ToListAsync();

                    med.Medicamentos = await Context.Medicamentos
                        .Where(md => md.MedicaoId == med.Id).ToListAsync();
                }
                
            }
            return o ?? throw new EntityNotFoundException(typeof(Medico));
        }
        public async Task<Medico> UpdateOperador(Medico r)
        {
            Medico o = await Context.Medicos.FirstOrDefaultAsync(x => x.Nome == r.Nome && x.Crm == r.Crm);
            if (o == null)
                throw new EntityNotFoundException(typeof(Medico));
            Context.Medicos.Update(o);
            await Context.SaveChangesAsync();
            return o ?? throw new EntityNotFoundException(typeof(Medico));
        }

        public async Task<Medico> AddCrm(Guid id, string username, string crm)
        {
            Medico o = await Context.Medicos.FirstOrDefaultAsync(x => x.Id == id && x.Nome == username);
            if (o == null)
                throw new EntityNotFoundException(typeof(Medico));
            if (o.Crm == crm)
                return o;
            o.Crm = crm;
            await Context.SaveChangesAsync();
            return o ?? throw new EntityNotFoundException(typeof(Medico));
        }
        public async Task<Medico> AddEmail(Guid id, string username, string email)
        {
            Medico o = await Context.Medicos.FirstOrDefaultAsync(x => x.Id == id && x.Nome == username);
            if (o == null)
                throw new EntityNotFoundException(typeof(Medico));
            if (o.Email == email)
                return o;
            o.Email = email;
            await Context.SaveChangesAsync();
            return o ?? throw new EntityNotFoundException(typeof(Medico));
        }
    }
}