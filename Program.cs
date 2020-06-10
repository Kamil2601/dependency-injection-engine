

using System;
using Container;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Attributes;

namespace DependencyInjectionEngine
{


    class B
    {

    }
    class A
    {
        [DependencyConstructor]
        public A()
        {

        }

        public A(B x)
        {

        }

        public A(B x, B y)
        {

        }
    }

    class Loop
    {
        public B TheB { get; set; }
        public Loop(Loop2 loop)
        {

        }
    }

    class Loop2
    {
        public Loop2(Loop loop)
        {

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

    class Program
    {
        static void Main(string[] args)
        {
            SimpleContainer container = new SimpleContainer();
            container.Resolve<Loop3>();
        }
    }
}
