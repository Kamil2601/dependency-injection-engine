using NUnit.Framework;
using Container;
using TestClasses;

namespace Tests1
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Singleton()
        {
            SimpleContainer container = new SimpleContainer();
            container.RegisterType<A>(true);
            A obj1 = container.Resolve<A>();
            A obj2 = container.Resolve<A>();
            Assert.AreSame(obj1, obj2);
        }

        [Test]
        public void NotSingleton()
        {
            SimpleContainer container = new SimpleContainer();
            container.RegisterType<A>(false);
            A obj1 = container.Resolve<A>();
            A obj2 = container.Resolve<A>();
            Assert.AreNotSame(obj1, obj2);
        }

        [Test]
        public void NotImplementedInterface()
        {
            SimpleContainer container = new SimpleContainer();

            Assert.Catch( () => {
                container.Resolve<ITest>();
            });
        }

        [Test]
        public void ImplementedInterface()
        {
            SimpleContainer container = new SimpleContainer();
            container.RegisterType<ITest, B>(true);
            ITest obj = container.Resolve<ITest>();
            Assert.IsNotNull(obj);
            Assert.AreEqual(obj.Value(), 2);
        }

        [Test]
        public void RecursiveImplementation()
        {
            SimpleContainer container = new SimpleContainer();

            container.RegisterType<ITest, A>(false);
            
            ITest objA = container.Resolve<ITest>();

            Assert.IsNotNull(objA);
            Assert.AreEqual(objA.Value(), 1);

            container.RegisterType<A,B>(false);

            ITest objB = container.Resolve<ITest>();
            Assert.IsNotNull(objB);
            Assert.AreEqual(objB.Value(), 2);
        }

        // TESTY DO LISTY 10

        // Zadanie 1

        [Test]
        public void Instance()
        {
            SimpleContainer container = new SimpleContainer();
            ITest a1 = new A();
            container.RegisterInstance<ITest>(a1);
            ITest a2 = container.Resolve<ITest>();
            Assert.AreSame(a1, a2);
        }

        [Test]
        public void RegisterImplementByAfterInstance()
        {
            SimpleContainer container = new SimpleContainer();
            ITest instance = new A();
            container.RegisterInstance<ITest>(instance);
            ITest objA = container.Resolve<ITest>();
            Assert.AreEqual(objA.Value(), 1);

            container.RegisterType<ITest, B>(false);
            ITest objB = container.Resolve<ITest>();
            Assert.AreEqual(objB.Value(), 2);
            Assert.AreNotSame(objA, objB);
        }

        [Test]
        public void RegisterSingletonAfterInstance()
        {
            SimpleContainer container = new SimpleContainer();
            ITest instance = new A();
            container.RegisterInstance<ITest>(instance);
            ITest obj1 = container.Resolve<ITest>();
            container.RegisterType<ITest>(true);
            ITest obj2 = container.Resolve<ITest>();
            Assert.AreSame(obj1, obj2);
        }

        [Test]
        public void RegisterTypeAfterInstance()
        {
            SimpleContainer container = new SimpleContainer();
            ITest instance = new A();
            container.RegisterInstance<ITest>(instance);
            ITest obj1 = container.Resolve<ITest>();
            container.RegisterType<ITest, A>(false);
            ITest obj2 = container.Resolve<ITest>();
            Assert.AreNotSame(obj1, obj2);
            Assert.AreEqual(obj1.Value(), obj2.Value());
            container.RegisterType<ITest, B>(false);
            ITest obj3 = container.Resolve<ITest>();
            Assert.AreNotEqual(obj1.Value(), obj3.Value());
        }

        // Zadanie 2
        [Test]
        public void Injection()
        {
            SimpleContainer container = new SimpleContainer();
            X obj = container.Resolve<X>();
            Assert.IsNotNull(obj);
        }

        [Test]
        public void SingletonField()
        {
            SimpleContainer container = new SimpleContainer();
            container.RegisterType<Z>(true);
            Y obj1 = container.Resolve<Y>();
            Y obj2 = container.Resolve<Y>();
            Assert.AreSame(obj1.z, obj2.z);
        }

        [Test]
        public void InstanceField()
        {
            SimpleContainer container = new SimpleContainer();
            container.RegisterInstance<Z>(new Z());
            Y obj1 = container.Resolve<Y>();
            Y obj2 = container.Resolve<Y>();
            Assert.AreSame(obj1.z, obj2.z);
        }

        [Test]
        public void LoopException()
        {
            SimpleContainer container = new SimpleContainer();
            Assert.Catch( () => {
                container.Resolve<Loop>();
            });
        }

        [Test]
        public void StringInstance()
        {
            SimpleContainer container = new SimpleContainer();
            container.RegisterInstance<string>("Some string");
            WithString obj =  container.Resolve<WithString>();
            Assert.IsNotNull(obj);
        }

        [Test]
        public void MoreContructors()
        {
            SimpleContainer container = new SimpleContainer();
            TwoConstructors obj = container.Resolve<TwoConstructors>();
            Assert.True(obj.longerConstructor);
        }

        [Test]
        public void DependencyConstructorTest()
        {
            SimpleContainer container = new SimpleContainer();
            WithDependencyCostructor obj = container.Resolve<WithDependencyCostructor>();
            Assert.True(obj.depCons);
        }
    }
}