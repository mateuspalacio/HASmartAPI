using HASmart.Core.Entities;
using HASmart.Core.Exceptions;
using HASmart.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HASmart.Infrastructure.EFDataAccess.Repositories
{
    public class RelatoRepository : IRelatoRepository
    {
        private AppDBContext Context { get; }
        public RelatoRepository(AppDBContext context)
        {
            Context = context;
        }

        public async Task<Relatorio> CriarRelato(Relatorio r)
        {
            try
            {
                Context.Relatorios.Add(r);
                r.Cidadao = await Context.Cidadaos.FirstOrDefaultAsync(c => c.Id == r.CidadaoId);
                await this.Context.SaveChangesAsync();
                return r;
            }
            catch
            {
                if (!await this.Context.Cidadaos.AnyAsync(e => e.Id == r.CidadaoId))
                {
                    throw new EntityNotFoundException(typeof(Cidadao));
                }
                else
                {
                    throw new EntityConcurrencyException(typeof(Cidadao));
                }
            }
        }

        public async Task<Relatorio> AtualizarRelato(Relatorio r)
        {
            try
            {
                Relatorio toUpdate = await Context.Relatorios.FirstOrDefaultAsync(x => x.Id == r.Id && x.CidadaoId == r.CidadaoId);

                if (toUpdate is not null)
                {
                    toUpdate.RelatorioCidadao = r.RelatorioCidadao;
                    toUpdate.DataRelatorio = r.DataRelatorio;
                    toUpdate.RelatorNome = r.RelatorNome;
                    toUpdate.TipoContato = r.TipoContato;
                    toUpdate.Success = r.Success;
                    await this.Context.SaveChangesAsync();
                }

                return toUpdate;
            }
            catch (Exception)
            {
                if (!await Context.Cidadaos.AnyAsync(e => e.Id == r.CidadaoId))
                {
                    throw new EntityNotFoundException(typeof(Cidadao));
                }
                else
                {
                    throw new EntityConcurrencyException(typeof(Cidadao));
                }
            }
        }

        public async Task<Relatorio> ApagarRelato(Relatorio r)
        {
            try
            {
                Context.Relatorios.Remove(r);
                await Context.SaveChangesAsync();
                return r;
            }
            catch (Exception)
            {
                if (!await this.Context.Cidadaos.AnyAsync(e => e.Id == r.CidadaoId))
                {
                    throw new EntityNotFoundException(typeof(Cidadao));
                }
                else
                {
                    throw new EntityConcurrencyException(typeof(Cidadao));
                }
            }
        }

        public async Task<Relatorio> LerRelato(Guid id, Guid cidadaoId)
        {
            try
            {
                var r = await Context.Relatorios.FirstOrDefaultAsync(c => c.Id == id && c.CidadaoId == cidadaoId);
                return r;
            }
            catch
            {
                if (!await this.Context.Cidadaos.AnyAsync(e => e.Id == cidadaoId))
                {
                    throw new EntityNotFoundException(typeof(Cidadao));
                }
                else
                {
                    throw new EntityConcurrencyException(typeof(Cidadao));
                }
            }
        }

        public async Task<List<Relatorio>> LerRelatos(Guid cidadaoId)
        {
            if (Context.Cidadaos.Any(c => c.Id == cidadaoId))
            {
                var reports = await Context.Relatorios.Where(x => x.CidadaoId == cidadaoId).ToListAsync();
                return reports;
            }
            //Cidadao c = await this.Context.Cidadaos.Include(x => x.Medicoes).FirstOrDefaultAsync(x => x.Id == id);
            throw new EntityNotFoundException(typeof(Cidadao));
        }
        public async Task<List<Relatorio>> LerAllRelatos()
        {
            try
            {
                var reports = await Context.Relatorios.ToListAsync();
                return reports;
            } catch
            {
                throw new EntityNotFoundException(typeof(Relatorio));

            }

            //Cidadao c = await this.Context.Cidadaos.Include(x => x.Medicoes).FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<List<Relatorio>> LerRelatosParaAnonimo(string anonimo)
        {
            if (Context.Cidadaos.Any(c => c.AnonimoNome.Contains(anonimo)))
            {
                var reports = new List<Relatorio>();
                var cidadaos = await Context.Cidadaos.Where(c => c.AnonimoNome.Contains(anonimo)).ToListAsync();
                foreach (var c in cidadaos)
                {
                    var returns = await Context.Relatorios.Where(x => x.CidadaoId == c.Id).ToListAsync();
                    reports.AddRange(returns);
                }
                return reports;
            }
            //Cidadao c = await this.Context.Cidadaos.Include(x => x.Medicoes).FirstOrDefaultAsync(x => x.Id == id);
            throw new EntityNotFoundException(typeof(Cidadao));
        }
    }
}
