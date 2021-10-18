using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using HASmart.Core.Architecture;
using HASmart.Core.Exceptions;
using HASmart.Core.Repositories;
using Microsoft.EntityFrameworkCore;


namespace HASmart.Infrastructure.EFDataAccess.Repositories {
    /// <summary>
    /// Exemplo de como implementar os métodos mais comuns
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ReferenceRepository<T> where T : Entity {
        public DbContext Context { get; }
        public DbSet<T> Set { get; }

        public ReferenceRepository(DbContext context) {
            this.Context = context;
            this.Set = context.Set<T>();
        }


        public virtual async Task<T> Create(T t) {
            this.Set.Add(t);
            await this.Context.SaveChangesAsync();
            return t;
        }

        public virtual async Task<IEnumerable<T>> RetrieveAll() {
            return await this.Set.ToListAsync();
        }

        public virtual async Task<IEnumerable<T>> Retrieve(long from, long to) {
            return await this.Set.Skip((int)from).Take((int)(to - from)).ToListAsync();
        }

        public virtual async Task<T> RetrieveOneById(Guid id) {
            T t = await this.Set.FirstOrDefaultAsync(x => x.Id == id);
            return t ?? throw new EntityNotFoundException(typeof(T));
        }

        public virtual async Task<T> RetrieveOneByPredicate(Expression<Func<T, bool>> predicate) {
            T t =  await this.Set.FirstOrDefaultAsync(predicate);
            return t ?? throw new EntityNotFoundException(typeof(T));
        }

        public virtual async Task<IEnumerable<T>> RetrieveManyByPredicate(Expression<Func<T, bool>> predicate) {
            return await this.Set.Where(predicate).ToListAsync();
        }

        public virtual async Task<bool> ExistsByPredicate(Expression<Func<T, bool>> predicate) {
            return await this.Set.AnyAsync(predicate);
        }

        public virtual async Task<T> Update(T t) {
            this.Context.Entry(t).State = EntityState.Modified;

            try {
                await this.Context.SaveChangesAsync();
                return t;
            } catch (DbUpdateConcurrencyException) {
                if (! await this.Set.AnyAsync(e => e.Id == t.Id)) {
                    throw new EntityNotFoundException(typeof(T));
                } else {
                    throw new EntityConcurrencyException(typeof(T));
                }
            }
        }

        public virtual async Task Delete(Guid id) {
            T entity = await this.RetrieveOneById(id);
            this.Set.Remove(entity);
            try {
                await this.Context.SaveChangesAsync();
            } catch {
                throw new EntityNotFoundException(typeof(T));
            }
        }
    }
}
