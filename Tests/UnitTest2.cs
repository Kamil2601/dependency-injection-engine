// Testy do listy 11

using NUnit.Framework;
using Container;
using TestClasses;
using Locator;
using Factory;

namespace Tests2
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        // Zadanie 1

        [Test]
        public void DependencyMethodTest1()
        {
            SimpleContainer container = new SimpleContainer();
            WithDependencyMethod obj = container.Resolve<WithDependencyMethod>();
            Assert.True(obj.test);
        }

        [Test]
        public void DependencyMethodTest2()
        {
            SimpleContainer container = new SimpleContainer();

            Assert.Catch( () => {
                container.Resolve<Loop3>();
            });
        }

        // Zadanie 2

        [Test]
        public void BuildUpTest1()
        {
            SimpleContainer container = new SimpleContainer();
            E obj = new E();
            container.BuildUp<E>(obj);
            Assert.IsNotNull(obj.z);
        }

        [Test]
        public void BuildUpTest2()
        {
            SimpleContainer container = new SimpleContainer();
            E obj = new E();
            Z dep = new Z();
            container.RegisterInstance<Z>(dep);
            container.BuildUp<E>(obj);
            Assert.IsNotNull(obj.z);
            Assert.AreSame(dep, obj.z);
        }

        [Test]
        public void BuildUpTest3()
        {
            SimpleContainer container = new SimpleContainer();
            Loop3 obj = new Loop3();

            Assert.Catch( () => {
                container.BuildUp<Loop3>(obj);
            });
        }

        [Test]
        public void BuildUpTest4()
        {
            SimpleContainer container = new SimpleContainer();
            container.RegisterType<Z>(true);
            F obj = new F();
            container.BuildUp<F>(obj);
            Assert.IsNotNull(obj.z1);
            Assert.IsNotNull(obj.z2);
            Assert.AreSame(obj.z1, obj.z2);

            container.RegisterType<Z>(false);

            container.BuildUp<F>(obj);
            Assert.IsNotNull(obj.z1);
            Assert.IsNotNull(obj.z2);
            Assert.AreNotSame(obj.z1, obj.z2);
        }

        [Test]
        public void LocatorTest1()
        {
            SimpleContainer c = new SimpleContainer();
            ContainerProviderDelegate containerProvider = () => c;
            ServiceLocator.SetContainerProvider(containerProvider);
            SimpleContainer c2 = ServiceLocator.Current.GetInstance<SimpleContainer>();
            Assert.AreSame(c,c2);
        }

        [Test]
        public void LocatorTest2()
        {
            SimpleContainer c = new SimpleContainer();
            ContainerProviderDelegate containerProvider = () => c;
            ServiceLocator.SetContainerProvider(containerProvider);
            SimpleContainer c2 = ServiceLocator.Current.GetInstance<SimpleContainer>();
            Z obj = new Z();
            c.RegisterInstance<Z>(obj);
            Z obj2 = ServiceLocator.Current.GetInstance<Z>();
            Assert.AreSame(obj, obj2);
        }

        

        [Test]
        public void FactoryTest()
        {
            SimpleContainer container = new SimpleContainer();
            container.RegisterInstance<string>("Something");
            NotifierFactory.SetProvider(() => container.Resolve<Notifier>());
            DoTest();
        }

        private void DoTest()
        {
            NotifierFactory factory = new NotifierFactory();
            INotifier notifier = factory.CreateNotifier();
            string msg = notifier.Notify();
            Assert.AreEqual(msg, "Message: Something");
        }
    }
}