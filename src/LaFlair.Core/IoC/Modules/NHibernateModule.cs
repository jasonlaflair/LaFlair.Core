using System.Reflection;
using Autofac;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using LaFlair.Core.Repository;
using NHibernate;
using Module = Autofac.Module;

namespace LaFlair.Core.IoC.Modules
{
    public class NHibernateModule : Module
    {
        private readonly Assembly _mappingAssembly;
        private readonly IPersistenceConfigurer _persistenceConfigurer;

        public NHibernateModule(IPersistenceConfigurer persistenceConfigurer, Assembly mappingAssembly)
        {
            _persistenceConfigurer = persistenceConfigurer;
            _mappingAssembly = mappingAssembly;
        }

        protected override void Load(ContainerBuilder builder)
        {
            var config = Fluently.Configure()
                .Database(_persistenceConfigurer)
                .Mappings(x => x.FluentMappings.AddFromAssembly(_mappingAssembly).Conventions.AddAssembly(_mappingAssembly))
                .BuildConfiguration();

            var sessionFactory = config.BuildSessionFactory();

            builder.RegisterInstance(config).As<NHibernate.Cfg.Configuration>().SingleInstance();
            builder.RegisterInstance(sessionFactory).As<ISessionFactory>().SingleInstance();
            builder.Register(x => x.Resolve<ISessionFactory>().OpenSession()).As<ISession>()
                .OnActivated(x => x.Instance.FlushMode = FlushMode.Never)
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>));
        }
    }
}
