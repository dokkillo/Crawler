using Spider.DLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Spider.DAL
{
    public class Repository<T> : IRepository<T> where T : class, IEntity
    {
        readonly DBContext _ctx;

        public Repository(DBContext context)
        {
            _ctx = context;
        }

        public void Delete(T TEntity)
        {
            _ctx.Set<T>().Remove(TEntity);
            _ctx.SaveChanges();
        }

        public IEnumerable<T> FindAll()
        {
            IQueryable<T> query = _ctx.Set<T>();
            return query;
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            IQueryable<T> query = _ctx.Set<T>().Where(predicate);
            return query;
        }

        public T FindOne(Expression<Func<T, bool>> predicate)
        {
            return _ctx.Set<T>().Where(predicate).FirstOrDefault();
        }


        public void Save(T TEntity)
        {
            _ctx.Set<T>().Add(TEntity);
            _ctx.SaveChanges();
        }

        public void Edit(T TEntity)
        {
            Delete(TEntity);
            Save(TEntity);
        }
    }
}
