using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ParkyApi.Data;
using ParkyApi.Models.Repository.IRepository;

namespace ParkyApi.Models.Repository2
{
    public class Repository<TEntity> : IRepository.IRepository<TEntity> where TEntity : class
    {
        protected readonly ParkyDbContext db;
        protected DbSet<TEntity> _dbSet;

        public Repository(ParkyDbContext db)
        {
            this.db = db;
            this._dbSet = this.db.Set<TEntity>();
        }

        public TEntity Get(int id)
        {
            using (var dbCtx = new ParkyDbContext(this.db._option))
            {
                this._dbSet = dbCtx.Set<TEntity>();
                return _dbSet.Find(id);
            }
        }
        public IEnumerable<TEntity> GetAll()
        {
            using (var dbCtx = new ParkyDbContext(this.db._option))
            {
                return _dbSet.ToList();
            }
        }

        public bool Add(TEntity entity)
        {
            using (var dbCtx = new ParkyDbContext(db._option))
            {
                this._dbSet.AddAsync(entity);

                if (Save())
                    return true;
                else
                    return false;
            }
        }
        public bool AddRange(IEnumerable<TEntity> entities)
        {
            using (var dbCtx = new ParkyDbContext(db._option))
            {
                this._dbSet.AddRangeAsync(entities);

                if (Save())
                    return true;
                else
                    return false;
            }
        }

        public bool Remove(TEntity entity)
        {
            using (var dbCtx = new ParkyDbContext(db._option))
            {
                _dbSet.Remove(entity);
                return Save();
            }
        }

        public bool RemoveRange(IEnumerable<TEntity> entities)
        {
            using (var dbCtx = new ParkyDbContext(db._option))
            {
                _dbSet.RemoveRange(entities);
                return Save();
            }
        }


        public bool Update(TEntity entity)
        {
            using (var dbCtx = new ParkyDbContext(db._option))
            {
                dbCtx.Update(entity);

                return Save(dbCtx);
            }
        }
   



        public IEnumerable<TEntity> Find(
            Expression<Func<TEntity, bool>> predicate = null)
        {
            return this._dbSet.Where(predicate);
        }

        public bool Save()
        {
            return this.db.SaveChanges() >= 0 ? true : false;
        }
        public bool Save(DbContext db)
        {
            return db.SaveChanges() >= 0 ? true : false;
        }

        public bool Exists(int id)
        {
            using (var dbCtx = new ParkyDbContext(this.db._option))
            {
                return this._dbSet.Find(id) != null ? true : false; 
            }
        }
    }
}
