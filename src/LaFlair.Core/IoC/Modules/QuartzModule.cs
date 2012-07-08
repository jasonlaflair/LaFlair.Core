using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;
using Autofac;
using LaFlair.Core.Quartz;
using Quartz;
using Quartz.Impl;
using Module = Autofac.Module;

namespace LaFlair.Core.IoC.Modules
{
    public class QuartzModule : Module
    {
        private readonly IEnumerable<Assembly> _jobAssemblies;
        private readonly NameValueCollection _quartzProperties;

        public QuartzModule(Assembly jobAssembly, NameValueCollection quartzProperties = null)
        {
            _jobAssemblies = new List<Assembly> { jobAssembly };
            _quartzProperties = quartzProperties;
        }

        public QuartzModule(IEnumerable<Assembly> jobAssemblies, NameValueCollection quartzProperties = null)
        {
            _jobAssemblies = jobAssemblies;
            _quartzProperties = quartzProperties;
        }

        protected override void Load(ContainerBuilder builder)
        {
            var factory = _quartzProperties == null
                              ? new StdSchedulerFactory()
                              : new StdSchedulerFactory(_quartzProperties);

            builder.RegisterInstance(factory).As<ISchedulerFactory>().SingleInstance();
            builder.Register(x => x.Resolve<ISchedulerFactory>().GetScheduler())
                .As<IScheduler>()
                .OnActivated(x =>
                             {
                                 x.Instance.JobFactory = new AutofacJobFactory();
                             })
                .SingleInstance();

            foreach (var assm in _jobAssemblies)
            {
                builder.RegisterAssemblyTypes(assm)
                    .Where(x => x.IsAssignableTo<IJob>())
                    .AsSelf()
                    .AsImplementedInterfaces();
            }
        }
    }
}