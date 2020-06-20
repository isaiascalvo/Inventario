using Data;
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
        Task<List<T>> GetAll();
        Task<List<T>> Find(Expression<Func<T, bool>> predicate);
        Task<T> GetById(Guid id, params Expression<Func<T, object>>[] includeProperties);
        Task<T> Add(T entity);
        Task<T> Update(T entity);
        Task<T> Delete(Guid id);
        IQueryable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties);
    }
}
