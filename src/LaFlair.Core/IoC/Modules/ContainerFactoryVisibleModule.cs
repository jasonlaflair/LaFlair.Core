using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Module = Autofac.Module;

namespace LaFlair.Core.IoC.Modules
{
    public class ContainerFactoryVisibleModule : Module
    {
        private readonly IEnumerable<Assembly> _assemblies;

        public ContainerFactoryVisibleModule(IEnumerable<Assembly> assemblies)
        {
            _assemblies = assemblies;
        }

        protected override void Load(ContainerBuilder builder)
        {
            foreach (var assm in _assemblies)
            {
                builder.RegisterAssemblyTypes(assm)
                    .Where(t => t.GetCustomAttributes(typeof (ContainerFactoryVisibleAttribute), false).Any())
                    .AsSelf()
                    .AsImplementedInterfaces();
            }
        }
    }
}
