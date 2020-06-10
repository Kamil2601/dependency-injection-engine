// Testy do listy 11

using NUnit.Framework;
using Container;
using TestClasses;

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
    }
}