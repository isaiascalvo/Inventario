using Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.EFCore
{
    public abstract class GenericRepository<TEntity> : IGenericRepository<TEntity>
        where TEntity : class, IEntity
    {
        private readonly InventarioDbContext context;
        public GenericRepository(InventarioDbContext context)
        {
            this.context = context;
        }
        public async Task<TEntity> Add(TEntity entity)
        {
            await context.Set<TEntity>().AddAsync(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task<TEntity> Delete(Guid id)
        {
            var entity = await this.GetById(id);
            if (entity == null)
            {
                return entity;
            }
            entity.IsDeleted = true;
            entity.DeletedAt = DateTime.Now;
            context.Set<TEntity>().Update(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task<TEntity> GetById(Guid id, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = context.Set<TEntity>();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return await query.FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);
        }

        public async Task<List<TEntity>> GetAll()
        {
            try
            {
                return await context.Set<TEntity>().Where(e => !e.IsDeleted).ToListAsync();
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        public async Task<TEntity> Update(TEntity entity)
        {
            entity.LastModificationAt = DateTime.Now;
            context.Set<TEntity>().Update(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task<List<TEntity>> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return await context.Set<TEntity>().Where(predicate).Where(e => !e.IsDeleted).ToListAsync();
        }

        public IQueryable<TEntity> AllIncluding(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = context.Set<TEntity>();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query.Where(e => !e.IsDeleted);
        }
    }
}
