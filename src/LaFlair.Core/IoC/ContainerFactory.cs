using System;
using Autofac;
using Autofac.Core;

namespace LaFlair.Core.IoC
{
    public class ContainerFactory
    {
        private static IContainer _container;

        public static void Reset()
        {
            if (_container == null)
            {
                return;
            }

            _container.Dispose();
            _container = null;
        }

        public static void Initialize(params IModule[] modules)
        {
            var containerBuilder = new ContainerBuilder();

            foreach (var module in modules)
            {
                containerBuilder.RegisterModule(module);
            }

            _container = containerBuilder.Build();
        }

        public static T Resolve<T>()
        {
            return _container.Resolve<T>();
        }

        public static object Resolve(Type type)
        {
            return _container.Resolve(type);
        }

        public static ILifetimeScope BeginLifetimeScope()
        {
            return _container.BeginLifetimeScope();
        }
    }
}
