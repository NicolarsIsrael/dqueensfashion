using DQueensFashion.Core.Model;
using DQueensFashion.Data.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DQueensFashion.Data.Implementation
{

    public class CoreRepo<TEntity> : ICoreRepo<TEntity> where TEntity : Entity
    {
        protected readonly System.Data.Entity.DbContext _dbContext;

        public CoreRepo(System.Data.Entity.DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(TEntity entity)
        {
            if (entity is Entity)
            {
                (entity as Entity).DateCreated = DateTime.UtcNow;
                (entity as Entity).DateModified = DateTime.UtcNow;
                (entity as Entity).IsDeleted = false;
            }

            _dbContext.Set<TEntity>().Add(entity);
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            entities.ToList().ForEach(e => e.DateCreated = DateTime.Now);
            entities.ToList().ForEach(e => e.DateModified = DateTime.Now);
            entities.ToList().ForEach(e => e.IsDeleted = false);
            _dbContext.Set<TEntity>().AddRange(entities);
        }

        public TEntity Get(object id)
        {
            var entity = _dbContext.Set<TEntity>().Find(id);
            if (entity == null) return null;
            if (entity.IsDeleted == false)
                return entity;
            return null;
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _dbContext.Set<TEntity>().Where(t=>t.IsDeleted ==false).ToList();
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbContext.Set<TEntity>().Where(predicate).Where(t=>t.IsDeleted==false);
        }

        //public IEnumerable<TEntity> FindUsingDictionary(Dictionary<string, object> dictionary)
        //{

        //}

        public int Count()
        {
            return _dbContext.Set<TEntity>().Where(t=>t.IsDeleted==false).Count();
        }

        public void Remove(TEntity entity)
        {
            entity.IsDeleted = true;
            Update(entity);
            //_dbContext.Set<TEntity>().Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            //_dbContext.Set<TEntity>().RemoveRange(entities);
            foreach (var item in entities)
            {
                Remove(item);
            }
        }

        public void Update(TEntity entity)
        {
            if (typeof(TEntity) == typeof(Entity))
            {
                (entity as Entity).DateModified = DateTime.Now;
            }
            _dbContext.Entry(entity).State = System.Data.Entity.EntityState.Modified;
        }

        //public void SaveChanges()
        //{
        //    _dbContext.SaveChanges();
        //}

        public void Attach(TEntity entity)
        {
            _dbContext.Set<TEntity>().Attach(entity);
        }
        public void Include(string entityName)
        {
            _dbContext.Set<TEntity>().Include(entityName);
        }

    }

}
