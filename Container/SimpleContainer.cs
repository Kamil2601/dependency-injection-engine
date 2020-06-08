using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace Container
{
    class TypeInfo
    {
        public bool isSingleton;
        public Type implementBy;
        public Object instance;

        public TypeInfo(bool isSingleton, Type implementBy)
        {
            this.isSingleton = isSingleton;
            this.implementBy = implementBy;
            this.instance = null;
        }

        public TypeInfo(bool isSingleton, Type implementBy, Object instance)
        {
            this.isSingleton = isSingleton;
            this.implementBy = implementBy;
            this.instance = instance;
        }
    }

    public class SimpleContainer
    {
        Dictionary<Type, TypeInfo> registered = new Dictionary<Type, TypeInfo>();

        public void RegisterType<T>(bool singleton) where T : class
        {
            Type type= typeof(T);
            if (!registered.ContainsKey(type))
            {
                TypeInfo info = new TypeInfo(singleton, type);
                registered.Add(type, info);
            }
            else
            {
                registered[type].isSingleton = singleton;
                if (!singleton)
                {
                    registered[type].instance = null;
                }
            }
        }
        public void RegisterType<From, To>(bool singleton) where To : From
        {
            Type from = typeof(From);
            Type to = typeof(To);
            if (!registered.ContainsKey(from))
            {
                registered.Add(from, new TypeInfo(singleton, to));
            }
            else
            {
                registered[from].implementBy = to;
                registered[from].isSingleton = singleton;
            }
        }

        public void RegisterInstance<T>( T instance )
        {
            Type type = typeof(T);
            registered[type] = new TypeInfo(true, type, instance);
        }

        private Object CreateObject(Type type, HashSet<Type> dependencies)
        {

            if (dependencies.Contains(type))
            {
                throw new Exception(String.Format(
                    "Cycle in dependency tree! Repeated type: {0}",
                    type));
            }

            dependencies.Add(type);

            ConstructorInfo[] constructors = type.GetConstructors();

            Console.WriteLine(constructors.Count());

            ConstructorInfo constructor = constructors.Aggregate(
                (cons1, cons2) => 
                    cons1.GetParameters().Length >= cons2.GetParameters().Length ? cons1 : cons2
                );


            ParameterInfo[] parameters = constructor.GetParameters();


            var arguments = parameters.Select((param) =>
                Resolve(param.ParameterType, dependencies)
            ).ToArray();

            dependencies.Remove(type);

            return constructor.Invoke(arguments);
        }

        private Object Resolve(Type type, HashSet<Type> dependencies)
        {
            Console.WriteLine(String.Format("Resolve {0}",type));
            while (registered.ContainsKey(type) && registered[type].implementBy != type)
            {
                type = registered[type].implementBy;
            }

            if (type.IsInterface && registered[type].instance == null)
                throw new Exception(String.Format("Unknown implemenation of interface: {0}!", type));

            if (!registered.ContainsKey(type))
            {
                return CreateObject(type, dependencies);
            }
            else
            {
                var info = registered[type];
                if (info.isSingleton)
                {
                    Console.WriteLine("Singleton");
                    if (info.instance == null)
                    {
                        info.instance = CreateObject(type, dependencies);
                    }

                    return info.instance;
                }
                else
                {
                    return CreateObject(type, dependencies);
                }
            }
        }

        public T Resolve<T>()
        {
            Type type = typeof(T);
            return (T)Resolve(type, new HashSet<Type>());
        }
    }
}
