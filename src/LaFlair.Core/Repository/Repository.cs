using System;
using System.Collections.Generic;
using System.Linq;
using LaFlair.Core.IoC;
using NHibernate;
using NHibernate.Linq;

namespace LaFlair.Core.Repository
{
    internal class Repository<T> : IRepository<T> where T : class
    {
        public Repository()
            : this(ContainerFactory.Resolve<ISession>())
        {
            
        }

        public Repository(ISession session)
        {
            Session = session;
        }

        public virtual ISession Session { get; private set; }

        public virtual T Get(object id)
        {
            return Session.Get<T>(id);
        }

        public virtual IQueryable<T> Query()
        {
            return Session.Query<T>();
        }

        public virtual void Refresh(T entity)
        {
            Session.Refresh(entity);
        }

        public virtual void Refresh(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                var session = Session;
                session.Refresh(entity);
            }
        }

        public virtual void Save(T entity)
        {
            Transact(() => Session.SaveOrUpdate(entity));
        }

        public virtual void SaveAndFlush(T entity)
        {
            Save(entity);
            Flush();
        }

        public virtual void Save(IEnumerable<T> entities)
        {
            Transact(() =>
            {
                foreach (var entity in entities)
                {
                    Session.SaveOrUpdate(entity);
                }
            });
        }

        public virtual void SaveAndFlush(IEnumerable<T> entities)
        {
            Save(entities);
            Flush();
        }

        public virtual void Delete(T entity)
        {
            Transact(() => Session.Delete(entity));
        }

        public virtual void DeleteAndFlush(T entity)
        {
            Delete(entity);
            Flush();
        }

        public virtual void Delete(IEnumerable<T> entities)
        {
            Transact(() =>
            {
                foreach (var entity in entities)
                {
                    Session.Delete(entity);
                }
            });
        }

        public virtual void DeleteAndFlush(IEnumerable<T> entities)
        {
            Delete(entities);
            Flush();
        }

        public virtual void Flush()
        {
            Session.Flush();
        }

        public virtual void Clear()
        {
            Session.Clear();
        }

        private TResult Transact<TResult>(Func<TResult> func)
        {
            if (!Session.Transaction.IsActive)
            {
                // Wrap in a transaction
                TResult result;

                using (var tx = Session.BeginTransaction())
                {
                    result = func.Invoke();
                    tx.Commit();
                }

                return result;
            }

            // Don't wrap
            return func.Invoke();
        }

        private void Transact(Action action)
        {
            Transact(() =>
                         {
                             action.Invoke();
                             return false;
                         });
        }
    }
}
