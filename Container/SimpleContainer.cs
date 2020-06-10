using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using Attributes;

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
            Type type = typeof(T);
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

        public void RegisterInstance<T>(T instance)
        {
            Type type = typeof(T);
            registered[type] = new TypeInfo(true, type, instance);
        }

        private bool HasAttribute<T>(MethodBase method) where T : Attribute
        {
            foreach(var attr in method.CustomAttributes)
            {
                if (attr.AttributeType == typeof(T))
                {
                    return true;
                }
            }

            return false;
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

            ConstructorInfo constructor = constructors.Aggregate(
                (cons1, cons2) =>
                {
                    bool isDep1 = HasAttribute<DependencyConstructor>(cons1);
                    bool isDep2 = HasAttribute<DependencyConstructor>(cons2);
                    if (isDep1 && isDep2)
                    {
                        throw new Exception(String.Format(
                        "Several constructors with [DependencyConstructor] attribute in class", type));
                    }
                    else if (isDep1)
                        return cons1;
                    else if (isDep2)
                        return cons2;
                    else if (cons1.GetParameters().Length >= cons2.GetParameters().Length)
                        return cons1;
                    else
                        return cons2;
                }
                );

            ParameterInfo[] parameters = constructor.GetParameters();

            var arguments = parameters.Select((param) =>
                Resolve(param.ParameterType, dependencies)
            ).ToArray();

            dependencies.Remove(type);

            return constructor.Invoke(arguments);
        }

        private void InvokeMethods(Object obj, Type type, HashSet<Type> dependencies)
        {
            dependencies.Add(type);
            Console.WriteLine("InvokeMethods");
            Console.WriteLine(type);
            MethodInfo[] methods = type.GetMethods();
            foreach (MethodInfo method in methods)
            {
                
                if (HasAttribute<DependencyMethod>(method) &&
                    method.ReturnType == typeof(void) &&
                    method.GetParameters().Count() > 0)
                {
                    Console.WriteLine(method.Name);
                    ParameterInfo[] parameters = method.GetParameters();

                    var arguments = parameters.Select((param) =>
                        Resolve(param.ParameterType, dependencies)
                        ).ToArray();

                    method.Invoke(obj, arguments);
                }
            }

            dependencies.Remove(type);
        }

        private Object Resolve(Type type, HashSet<Type> dependencies)
        {
            Object result;
            while (registered.ContainsKey(type) && registered[type].implementBy != type)
            {
                type = registered[type].implementBy;
            }

            if (type.IsInterface && registered[type].instance == null)
                throw new Exception(String.Format("Unknown implemenation of interface: {0}!", type));

            if (!registered.ContainsKey(type))
            {
                result = CreateObject(type, dependencies);
            }
            else
            {
                var info = registered[type];
                if (info.isSingleton)
                {
                    if (info.instance == null)
                    {
                        info.instance = CreateObject(type, dependencies);
                    }

                    result = info.instance;
                }
                else
                {
                    result = CreateObject(type, dependencies);
                }
            }

            InvokeMethods(result, type, dependencies);

            return result;
        }

        public T Resolve<T>()
        {
            Type type = typeof(T);
            return (T)Resolve(type, new HashSet<Type>());
        }
    }
}
