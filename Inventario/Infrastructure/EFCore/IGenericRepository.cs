using Data;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.EFCore
{
    public interface IGenericRepository<T> where T : class, IEntity
    {
        Task<List<T>> GetAll(params Func<IQueryable<T>, IIncludableQueryable<T, object>>[] includeProperties);
        Task<List<T>> Find(Expression<Func<T, bool>> predicate, params Func<IQueryable<T>, IIncludableQueryable<T, object>>[] includeProperties);
        Task<T> GetById(Guid id, params Func<IQueryable<T>, IIncludableQueryable<T, object>>[] includeProperties);
        Task<T> Add(T entity);
        Task<T> Update(T entity);
        Task<T> Delete(Guid userId, Guid id);
        IQueryable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties);
        Task<List<T>> FindDeleted(Expression<Func<T, bool>> predicate);
        Task CommitAsync();
    }
}
