//using System;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;
//
//namespace SimpleIoc
//{
//    public partial class Context
//    {
//        private class DictionaryContainer : IContainer
//        {
//            private readonly Dictionary<Type, RegisteredObject> cachedObjects = new Dictionary<Type, RegisteredObject>();
//
//            public void Dispose()
//            {
//                cachedObjects.Clear();
//            }
//
//            public void Bind<TTypeToResolve, TConcrete>()
//            {
//                Bind<TTypeToResolve, TConcrete>(LifeCycle.Transient);
//            }
//
//            public void Bind<TTypeToResolve, TConcrete>(LifeCycle lifeCycle)
//            {
//                var typeToResolve = typeof(TTypeToResolve);
//                var concreteType = typeof(TConcrete);
//
//                if (cachedObjects.ContainsKey(typeToResolve))
//                {
//                    Debug.Log("Cannot register a Type twice");
//                    return;
//                }
//
//                Debug.Log("  register type: " + typeToResolve);
//
//                var registerObj = new RegisteredObject(typeToResolve, concreteType, lifeCycle);
//                cachedObjects.Add(typeToResolve, registerObj);
//            }
//
//            public void Bind(object instance)
//            {
//                Bind(instance.GetType(), instance);
//            }
//
//            public void Bind(Type typeToResolve, object instance)
//            {
//                try
//                {
//                    if (cachedObjects.ContainsKey(typeToResolve))
//                    {
//                        Debug.Log("Cannot register a Type twice");
//                        return;
//                    }
//
//                    Debug.Log("  register type: " + typeToResolve);
//                    var registerObj = new RegisteredObject(typeToResolve, instance);
//                    cachedObjects.Add(typeToResolve, registerObj);
//                }
//                catch (Exception ex)
//                {
//                    Debug.LogError(ex.Message);
//                    Debug.LogError(ex.StackTrace);
//                }
//            }
//
//            public void Bind<TTypeToResolve>()
//            {
//                Bind<TTypeToResolve, TTypeToResolve>(LifeCycle.Transient);
//            }
//
//            public void Bind<TTypeToResolve>(LifeCycle lifeCycle)
//            {
//                Bind<TTypeToResolve, TTypeToResolve>(lifeCycle);
//            }
//
//            public TTypeToResolve Resolve<TTypeToResolve>()
//            {
//                return (TTypeToResolve) ResolveObject(typeof(TTypeToResolve));
//            }
//
//            public object Resolve(Type typeToResolve, params object[] parameters)
//            {
//                return ResolveObject(typeToResolve, parameters);
//            }
//
//            private object ResolveObject(Type typeToResolve, params object[] parameters)
//            {
//                if (!cachedObjects.ContainsKey(typeToResolve))
//                {
//                    throw new TypeNotRegisteredException(string.Format(
//                        "The type {0} has not been registered", typeToResolve.Name));
//                }
//
//                return cachedObjects[typeToResolve];
//            }
//
//            private object GetInstance(RegisteredObject registeredObject, params object[] parameters)
//            {
//                if (registeredObject.Instance == null ||
//                    registeredObject.LifeCycle == LifeCycle.Transient)
//                {
//                    if (parameters == null || parameters.Length == 0)
//                    {
//                        parameters = ResolveConstructorParameters(registeredObject).ToArray();
//                    }
//                    registeredObject.CreateInstance(parameters.ToArray());
//                }
//
//                return registeredObject.Instance;
//            }
//
//            private IEnumerable<object> ResolveConstructorParameters(RegisteredObject registeredObject)
//            {
//                var constructorInfo = registeredObject.ConcreteType.GetConstructors().First();
//                foreach (var parameter in constructorInfo.GetParameters())
//                {
//                    yield return ResolveObject(parameter.ParameterType);
//                }
//            }
//        }
//    }
//}