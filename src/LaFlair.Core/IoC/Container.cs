using Autofac;

namespace LaFlair.Core.IoC
{
    public class Container
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

        public static void Initialize(ContainerBuilder containerBuilder)
        {
            _container = containerBuilder.Build();
        }

        public static T Resolve<T>()
        {
            return _container.Resolve<T>();
        }
    }
}
