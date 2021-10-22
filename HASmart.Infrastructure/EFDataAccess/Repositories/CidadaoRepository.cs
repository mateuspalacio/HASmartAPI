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


namespace HASmart.Infrastructure.EFDataAccess.Repositories {
    public class CidadaoRepository : ICidadaoRepository {
        private AppDBContext Context  { get; }
        public CidadaoRepository(AppDBContext context) { 
            this.Context = context;
        }


        public async Task<IEnumerable<Cidadao>> BuscarTodos(long de, long para) {
            return await this.Context.Cidadaos.Skip((int)de).Take((int)(para - de)).Include(x => x.Medicoes).ToListAsync();
        }

        //Retirado .Include(x => x.Dispencacoes) dos 3 metodos abaixo
        public async Task<Cidadao> BuscarViaId(Guid id) {
            if (Context.Cidadaos.Any(c => c.Id == id))
            {
                var user = await Context.Cidadaos
                 .FirstOrDefaultAsync(u => u.Id == id);

                user.DadosPessoais = await Context.DadosPessoais
                    .FirstOrDefaultAsync(up => up.CidadaoId == id);

                user.DadosPessoais.Endereco = await Context.Enderecos
                    .FirstOrDefaultAsync(add => add.DadosPessoais_Id == user.DadosPessoais.DadosPessoaisId);

                user.IndicadorRiscoHAS = await Context.IndicadorRiscos
                    .FirstOrDefaultAsync(ris => ris.Cidadao_Id == id);

                user.Medicoes = await Context.Medicoes
                    .Where(x => x.CidadaoId == id).ToListAsync();
                foreach (var med in user.Medicoes)
                {
                    med.Afericoes = await Context.Afericao
                        .Where(afe => afe.MedicaoId == med.Id).ToListAsync();

                    med.Medicamentos = await Context.Medicamentos
                        .Where(md => md.MedicaoId == med.Id).ToListAsync();
                }
                //user.medicoAtual = await Context.Medicos.Where(r => r.cidadaosAtuais.FirstOrDefault().Id == user.Id).FirstOrDefaultAsync();
                foreach (var medic in Context.Medicos)
                {
                    if (medic.cidadaosAtuais != null)
                    {
                        foreach (var cit in medic.cidadaosAtuais)
                        {
                            if (cit.Id == id)
                            {
                                user.medicoAtual = medic;
                            }
                        }
                    }
                }
                user.Relatorios = await Context.Relatorios
                    .Where(rel => rel.CidadaoId == user.Id).ToListAsync();
                return user;
            }
            //Cidadao c = await this.Context.Cidadaos.Include(x => x.Medicoes).FirstOrDefaultAsync(x => x.Id == id);
            throw new EntityNotFoundException(typeof(Cidadao));
        }

        public async Task<Cidadao> BuscarViaCpf(string cpf) {
           
            if(Context.Cidadaos.Any(c => c.Cpf == cpf))
            {
                var user = await Context.Cidadaos
                 .FirstOrDefaultAsync(u => u.Cpf == cpf);

                user.DadosPessoais = await Context.DadosPessoais
                    .FirstOrDefaultAsync(up => up.CidadaoId == user.Id);

                user.DadosPessoais.Endereco = await Context.Enderecos
                    .FirstOrDefaultAsync(add => add.DadosPessoais_Id == user.DadosPessoais.DadosPessoaisId);

                user.IndicadorRiscoHAS = await Context.IndicadorRiscos
                    .FirstOrDefaultAsync(ris => ris.Cidadao_Id == user.Id);

                user.Medicoes = await Context.Medicoes
                    .Where(med => med.CidadaoId == user.Id).ToListAsync();

                foreach (var med in user.Medicoes)
                {
                    med.Afericoes = await Context.Afericao
                        .Where(afe => afe.MedicaoId == med.Id).ToListAsync();

                    med.Medicamentos = await Context.Medicamentos
                        .Where(md => md.MedicaoId == med.Id).ToListAsync();
                }
                foreach (var medic in Context.Medicos)
                {
                    if (medic.cidadaosAtuais != null)
                    {
                        foreach (var cit in medic.cidadaosAtuais)
                        {
                            if (cit.Id == user.Id)
                            {
                                user.medicoAtual = medic;
                            }
                        }
                    }

                }
                user.Relatorios = await Context.Relatorios
                    .Where(rel => rel.CidadaoId == user.Id).ToListAsync();
                return user;
            }
            
            //user.medicoAtual = await Context.Medicos.Select(md => md.cidadaosAtuais);
            //user.medicoAtual = await Context.Medicos.FirstOrDefaultAsync(r => r.cidadaosAtuais.FirstOrDefault().Id == user.Id);


            //Cidadao c = await this.Context.Cidadaos.Include(x => x.Medicoes).FirstOrDefaultAsync(x => x.Id == id);
            throw new EntityNotFoundException(typeof(Cidadao));
        }

        public async Task<Cidadao> BuscarViaRg(string rg) {
            if (Context.Cidadaos.Any(c => c.Rg == rg))
            {
                var user = await Context.Cidadaos
                 .FirstOrDefaultAsync(u => u.Rg == rg);

                user.DadosPessoais = await Context.DadosPessoais
                    .FirstOrDefaultAsync(up => up.CidadaoId == user.Id);

                user.DadosPessoais.Endereco = await Context.Enderecos
                    .FirstOrDefaultAsync(add => add.DadosPessoais_Id == user.DadosPessoais.DadosPessoaisId);

                user.IndicadorRiscoHAS = await Context.IndicadorRiscos
                    .FirstOrDefaultAsync(ris => ris.Cidadao_Id == user.Id);
                user.Medicoes = await Context.Medicoes
                   .Where(med => med.CidadaoId == user.Id).ToListAsync();

                foreach (var med in user.Medicoes)
                {
                    med.Afericoes = await Context.Afericao
                        .Where(afe => afe.MedicaoId == med.Id).ToListAsync();

                    med.Medicamentos = await Context.Medicamentos
                        .Where(md => md.MedicaoId == med.Id).ToListAsync();
                }
                foreach (var medic in Context.Medicos)
                {
                    if (medic.cidadaosAtuais != null)
                    {
                        foreach (var cit in medic.cidadaosAtuais)
                        {
                            if (cit.Id == user.Id)
                            {
                                user.medicoAtual = medic;
                            }
                        }
                    }
                }
                user.Relatorios = await Context.Relatorios
                    .Where(rel => rel.CidadaoId == user.Id).ToListAsync();
                return user;
            }
            //Cidadao c = await this.Context.Cidadaos.Include(x => x.Medicoes).FirstOrDefaultAsync(x => x.Id == id);
            throw new EntityNotFoundException(typeof(Cidadao));
        }

        public async Task<bool> AlreadyExists(string cpf, string rg) {
            return await this.Context.Cidadaos.AnyAsync(x => x.Cpf == cpf || x.Rg == rg);
        }

        public async Task<Cidadao> Cadastrar(Cidadao c) {
            this.Context.Cidadaos.Add(c);
            await this.Context.SaveChangesAsync();
            return c;
        }

        public async Task<Cidadao> Atualizar(Cidadao c) {
            this.Context.Entry(c).State = EntityState.Modified;
            
            try {
                await this.Context.SaveChangesAsync();
                return c;
            } catch (Exception) {
                if (! await this.Context.Cidadaos.AnyAsync(e => e.Id == c.Id)) {
                    throw new EntityNotFoundException(typeof(Cidadao));
                } else {
                    throw new EntityConcurrencyException(typeof(Cidadao));
                }
            }
        }
        public async Task<Cidadao> ApagarCidadao(Cidadao cidadao)
        {
            try
            {
                Context.Cidadaos.Remove(cidadao);
                await Context.SaveChangesAsync();
                return cidadao;
            }
            catch (Exception)
            {
                if (!await this.Context.Cidadaos.AnyAsync(e => e.Id == cidadao.Id))
                {
                    throw new EntityNotFoundException(typeof(Cidadao));
                }
                else
                {
                    throw new EntityConcurrencyException(typeof(Cidadao));
                }
            }
        }
        public async Task<List<Cidadao>> BuscarPorNome(string name)
        {
            try
            {
                var result = await Context.Cidadaos.Where(data => data.Nome.ToLower().Contains(name.ToLower())).ToListAsync();
                return result;
            }
            catch (Exception)
            {
                if (!await this.Context.Cidadaos.AnyAsync(e => e.Nome == name))
                {
                    throw new EntityNotFoundException(typeof(Cidadao));
                }
                else
                {
                    throw new EntityConcurrencyException(typeof(Cidadao));
                }
            }
        }

        public async Task<List<Cidadao>> BuscarPorNomeAnonimo(string name)
        {
            try
            {
                var result = await Context.Cidadaos.Where(data => data.AnonimoNome.ToLower().Contains(name.ToLower())).ToListAsync();
                return result;
            }
            catch (Exception)
            {
                if (!await this.Context.Cidadaos.AnyAsync(e => e.Nome == name))
                {
                    throw new EntityNotFoundException(typeof(Cidadao));
                }
                else
                {
                    throw new EntityConcurrencyException(typeof(Cidadao));
                }
            }
        }

    }
}
