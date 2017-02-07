using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Spider.DAL
{
    interface IRepository<T>
    {
        T FindOne(Expression<Func<T, bool>> predicate);
        void Save(T TEntity);
        void Delete(T TEntity);

        void Edit(T TEntity);

        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);

        IEnumerable<T> FindAll();



    }
}
