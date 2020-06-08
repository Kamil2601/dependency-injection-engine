using System;
using Container;
using System.Collections.Generic;
using System.Linq;

namespace DependencyInjectionEngine
{
    class B
    {

    }
    class A
    {
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

    class Program
    {
        static void Main(string[] args)
        {
            SimpleContainer container = new SimpleContainer();
            container.Resolve<Loop>();
        }
    }
}
