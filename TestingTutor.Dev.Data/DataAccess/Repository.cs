using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TestingTutor.Dev.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace TestingTutor.Dev.Data.DataAccess
{
    public class Repository<TEntity, T> : IRepository<TEntity, T> where TEntity : IdentityModel<T>
    {
        protected readonly ApplicationDbContext Context;
        protected readonly DbSet<TEntity> Entities;
        
        public Repository(ApplicationDbContext context)
        {
            Context = context;
            Entities = Context.Set<TEntity>();
        }
        

        public virtual Task<TEntity> Get(T id)
        {
            return Entities.FindAsync(id);
        }

        public virtual async Task<IEnumerable<TEntity>> GetAll()
        {
            return await Entities.ToListAsync();
        }

        public virtual async Task<IEnumerable<TEntity>> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return await Entities.Where(predicate).ToListAsync();
        }

        public virtual Task<TEntity> SingleOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return Entities.SingleOrDefaultAsync(predicate);
        }

        public virtual async Task Add(TEntity entity)
        {
            await Entities.AddAsync(entity);
            await Context.SaveChangesAsync();
        }

        public virtual async Task AddRange(IEnumerable<TEntity> entities)
        {
            await Entities.AddRangeAsync(entities);
            await Context.SaveChangesAsync();
        }

        public virtual async Task Remove(TEntity entity)
        {
            Entities.Remove(entity);
            await Context.SaveChangesAsync();
        }

        public virtual async Task RemoveRange(IEnumerable<TEntity> entities)
        {
            Entities.RemoveRange(entities);
            await Context.SaveChangesAsync();
        }

        public virtual async Task Update(TEntity entity)
        {
            Entities.Update(entity);
            await Context.SaveChangesAsync();
        }
    }
}
