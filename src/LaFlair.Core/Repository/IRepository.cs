using System.Collections.Generic;
using System.Linq;
using NHibernate;

namespace LaFlair.Core.Repository
{
    public interface IRepository<T>
    {
        ISession Session { get; }

        T Get(object id);

        IQueryable<T> Query();

        void Refresh(T entity);
        void Refresh(IEnumerable<T> entities);

        void Save(T entity);
        void SaveAndFlush(T entity);

        void Save(IEnumerable<T> entities);
        void SaveAndFlush(IEnumerable<T> entities);

        void Delete(T entity);
        void DeleteAndFlush(T entity);

        void Delete(IEnumerable<T> entities);
        void DeleteAndFlush(IEnumerable<T> entities);

        void Flush();
        void Clear();
    }
}
