

using System;
using Container;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public class DependencyConstructor : Attribute { }

public class DependencyMethod : Attribute { }

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

    class Program
    {
        static void Main(string[] args)
        {
            Type type = typeof(A);
            foreach (var constructor in type.GetConstructors())
            {
                var test =  constructor.CustomAttributes.Any(
                    (attr) => attr.AttributeType == typeof(DependencyConstructor)
                );

                Console.WriteLine(test);
            }
        }
    }
}
