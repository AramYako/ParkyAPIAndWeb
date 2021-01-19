using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ParkyApi.Models.Repository.IRepository
{
    public interface IRepository<TEntity> where TEntity: class
    {

        TEntity Get(int id);
        IEnumerable<TEntity> GetAll();

        bool Remove(TEntity entity);
        bool RemoveRange(IEnumerable<TEntity> entities);

        Task<bool> Exists(int id);

        bool Add(TEntity entity);
        bool AddRange(IEnumerable<TEntity> entities);

        bool Update(TEntity entity);

        bool Save();
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate = null);
    }
}