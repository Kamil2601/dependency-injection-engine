using Attributes;

namespace TestClasses
{
    interface ITest
    {
        int Value();
    }
    class A : ITest
    {
        public int x;

        public A()
        {
            x = 1;
        }

        public virtual int Value()
        {
            return 1;
        }
    }

    class B : A
    {
        public B()
        {
            x = 2;
        }

        public override int Value()
        {
            return 2;
        }
    }

    class X
    {
        public Y y;
        public X(Y y)
        {
            this.y = y;
        }
    }

    class Y
    {
        public Z z;
        public Y(Z z)
        {
            this.z = z;
        }
    }

    class Z
    {

    }

    class Loop
    {
        public Loop(Loop loop)
        {

        }
    }

    class WithString
    {
        public WithString(string s)
        {

        }
    }

    class TwoConstructors
    {
        public bool longerConstructor;
        public TwoConstructors(Z z, Z z2)
        {
            longerConstructor = true;
        }

        public TwoConstructors(Z z)
        {
            longerConstructor = false;
        }
    }

    class WithDependencyCostructor
    {
        public bool depCons;

        [DependencyConstructor]
        public WithDependencyCostructor()
        {
            depCons = true;
        }

        public WithDependencyCostructor(Z z)
        {
            depCons = false;
        }
    }

    class WithDependencyMethod
    {
        public bool test;

        public WithDependencyMethod()
        {
            test = false;
        }

        [DependencyMethod]
        public void SomeMethod(Z z)
        {
            test = true;
        }
    }

    class Loop3
    {
        Loop3 loop;

        public Loop3()
        {

        }

        [DependencyMethod]
        public void SomeMethod(Loop3 loop)
        {
            this.loop = loop;
        }
    }

    class E
    {
        public Z z;
        public E()
        {

        }

        [DependencyMethod]
        public void AssignZ(Z z)
        {
            this.z = z;
        }
    }

    class F
    {
        public Z z1, z2;

        public F()
        {

        }

        [DependencyMethod]
        public void Assignz1(Z z)
        {
            z1 = z;
        }

        [DependencyMethod]
        public void Assignz2(Z z)
        {
            z2 = z;
        }
    }
}