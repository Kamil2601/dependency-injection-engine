using System;
using Container;

namespace Locator
{
    public delegate SimpleContainer ContainerProviderDelegate();

    public class ServiceLocator
    {
        private static SimpleContainer container;

        public static void SetContainerProvider(ContainerProviderDelegate ContainerProvider)
        {
            container = ContainerProvider();
            container.RegisterInstance<SimpleContainer>(container);
        }
        public static ServiceLocator Current
        {
            get
            {
                return new ServiceLocator();
            }
        }
        public T GetInstance<T>() //where T : class
        {
            return container.Resolve<T>();
        }
    }
}
