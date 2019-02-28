/**
 * Author:    Vinh Vu Thanh
 * This class is a part of Unity IoC project that can be downloaded free at 
 * https://github.com/game-libgdx-unity/UnityEngine.IoC
 * (c) Copyright by MrThanhVinh168@gmail.com
 **/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;

namespace UnityIoC
{
    public partial class AssemblyContext
    {
        public class DefaultContainer : IContainer
        {
            internal HashSet<Type> registeredTypes = new HashSet<Type>();
            internal IList<RegisteredObject> registeredObjects = new List<RegisteredObject>();

            private AssemblyContext assemblyContext;

            public DefaultContainer(AssemblyContext assemblyContext)
            {
                this.assemblyContext = assemblyContext;
            }

            public void Dispose()
            {
                Debug.Log("Disposing container...");
                foreach (var registeredObject in registeredObjects)
                {
                    registeredObject.Dispose();
                }

                registeredTypes.Clear();
                registeredObjects.Clear();
            }

            public void Bind<TTypeToResolve, TConcrete>()
            {
                Bind<TTypeToResolve, TConcrete>(LifeCycle.Transient);
            }

            public void Bind(Type typeToResolve, Type concreteType, LifeCycle lifeCycle = LifeCycle.Default)
            {
                if (concreteType.IsAbstract || concreteType.IsInterface)
                {
                    throw new InvalidOperationException("Cannot bind to concrete class by an abstract type " +
                                                        concreteType);
                }

                if (registeredTypes.Contains(typeToResolve))
                {
                    Debug.Log("{0} already registered {1} time", typeToResolve.ToString(),
                        registeredObjects.Count(o => o.TypeToResolve == typeToResolve));
                }
                else
                {
                    Debug.Log("new type registered: " + typeToResolve + " as " + lifeCycle);
                    registeredTypes.Add(typeToResolve);
                }

                Debug.Log("Add registeredObject: " + concreteType + " for " + typeToResolve + " as " + lifeCycle);
                registeredObjects.Add(new RegisteredObject(typeToResolve, concreteType, assemblyContext, lifeCycle));
            }

            public void Bind<TTypeToResolve, TConcrete>(LifeCycle lifeCycle)
            {
                if (typeof(TConcrete).IsAbstract || typeof(TConcrete).IsInterface)
                {
                    throw new InvalidOperationException("Cannot bind empty object of abstract type");
                }

                if (registeredTypes.Contains(typeof(TTypeToResolve)))
                {
                    Debug.Log("Cannot register a Type twice");
                    return;
                }

                Debug.Log("  register type: " + typeof(TTypeToResolve));
                registeredTypes.Add(typeof(TTypeToResolve));
                registeredObjects.Add(new RegisteredObject(typeof(TTypeToResolve), typeof(TConcrete), assemblyContext,
                    lifeCycle));
            }

            public void Bind(InjectIntoBindingData data)
            {
                if (data.ImplementedType.IsAbstract || data.ImplementedType.IsInterface)
                {
                    throw new InvalidOperationException("Cannot bind empty object of abstract type");
                }

                Debug.Log("  register type: " + data.AbstractType);
                registeredTypes.Add(data.AbstractType);
                registeredObjects.Add(
                    new RegisteredObject(
                        data.AbstractType,
                        data.ImplementedType,
                        null,
                        data.LifeCycle,
                        data.InjectInto));
            }

            public void Bind(Type typeToResolve, object instance)
            {
                Bind(typeToResolve, instance.GetType(), instance);
            }

            public void Bind(Type typeToResolve, Type typeConcrete, object instance)
            {
                if (instance == null && (typeConcrete.IsAbstract || typeConcrete.IsInterface))
                {
                    throw new InvalidOperationException("Cannot bind null object of abstract type");
                }

                try
                {
                    if (registeredTypes.Contains(typeToResolve))
                    {
                        Debug.Log("Already registered type of " + typeToResolve);
                    }
                    else
                    {
                        Debug.Log("  register type: " + typeToResolve);
                        registeredTypes.Add(typeToResolve);
                    }

                    registeredObjects.Add(new RegisteredObject(typeToResolve, instance, assemblyContext));
                }
                catch (Exception ex)
                {
                    Debug.LogError(ex.Message);
                    Debug.LogError(ex.StackTrace);
                }
            }

            public void Bind<TTypeToResolve>()
            {
                Bind<TTypeToResolve, TTypeToResolve>(LifeCycle.Transient);
            }

            public void Bind<TTypeToResolve>(object instance)
            {
                Bind(typeof(TTypeToResolve), instance.GetType(), instance);
            }

            public void Bind<TTypeToResolve>(LifeCycle lifeCycle)
            {
                Bind<TTypeToResolve, TTypeToResolve>(lifeCycle);
            }

            public TTypeToResolve Resolve<TTypeToResolve>(LifeCycle lifeCycle = LifeCycle.Default,
                params object[] parameters)
            {
                return (TTypeToResolve) ResolveObject(typeof(TTypeToResolve), null, lifeCycle, parameters);
            }

            public object Resolve(Type typeToResolve, LifeCycle lifeCycle = LifeCycle.Default,
                params object[] parameters)
            {
                return ResolveObject(typeToResolve, null, lifeCycle, parameters);
            }

            public object ResolveObject(Type typeToResolve, object resolveFrom = null,
                LifeCycle preferredLifeCycle = LifeCycle.Default,
                params object[] parameters)
            {
                Func<RegisteredObject, bool> filter = null;

                if (assemblyContext.requirePreRegistered)
                    filter = o => o.TypeToResolve == typeToResolve;
                else
                    filter = o => o.TypeToResolve == typeToResolve ||
                                  o.ConcreteType == typeToResolve ||
                                  o.ConcreteType.IsSubclassOf(typeToResolve) ||
                                  o.ConcreteType.GetInterfaces().Contains(typeToResolve);


                var numOfRegisters = registeredObjects.Count(filter);

                if (resolveFrom == null || numOfRegisters <= 1)
                {
                    var registeredObject = registeredObjects.FirstOrDefault(filter);
                    if (registeredObject == null)
                    {
                        Debug.Log(
                            "The type {0} has not been registered", typeToResolve.Name);

                        //if the typeToResolve is abstract, then we cannot resolve it, throw exceptions
                        if (typeToResolve.IsAbstract || typeToResolve.IsInterface)
                        {
                            throw new InvalidOperationException(
                                "Cannot resolve the abstract type " + typeToResolve.Name +
                                " with no respective registeredObject!");
                        }

                        Debug.Log("trying to register {0} ", typeToResolve);

                        registeredObject = new RegisteredObject(typeToResolve, typeToResolve, assemblyContext,
                            preferredLifeCycle);
                        registeredObjects.Add(registeredObject);
                    }

                    var obj = GetInstance(registeredObject, preferredLifeCycle, resolveFrom, parameters);

                    return obj;
                }

                //high priority process for notnull InjectInto binding attributes
                foreach (var registeredObject in registeredObjects.Where(filter))
                {
                    //using binding attribute to filter registered objects
                    var injectFromType = registeredObject.InjectFromType;
                    if (injectFromType == null)
                    {
                        //no binding found
                        continue;
                    }

                    Debug.Log("Binding of " + registeredObject.ConcreteType + " has inject into: " + injectFromType);
                    var typeResolveFrom = resolveFrom.GetType();

                    //get the correct registeredObject to create an instance
                    if (injectFromType == typeResolveFrom)
                    {
                        Debug.Log("resolve from: " + typeResolveFrom + " by binding attributes");
                        return GetInstance(registeredObject, preferredLifeCycle, resolveFrom, parameters);
                    }
                }

                //lowest priority process for null InjectIntoType or binding attributes
                foreach (var registeredObject in registeredObjects.Where(filter))
                {
                    if (registeredObject.InjectFromType == null)
                    {
                        return GetInstance(registeredObject, preferredLifeCycle, resolveFrom, parameters);
                    }
                }

                Debug.Log("Resolve without resolveFrom object");
                return ResolveObject(typeToResolve, null, preferredLifeCycle, parameters);
            }
            

            public bool IsRegistered(Type type)
            {
                return registeredTypes.FirstOrDefault(t => t == type) != null;
            }

            public RegisteredObject GetRegisteredObject(Type typeToResolve)
            {
                return registeredObjects.FirstOrDefault(t => t.TypeToResolve == typeToResolve);
            }

            private object GetInstance(RegisteredObject registeredObject,
                LifeCycle preferredLifeCycle = LifeCycle.Default,
                object resolveFrom = null,
                params object[] parameters)
            {
                var objectLifeCycle = preferredLifeCycle != LifeCycle.Default
                    ? preferredLifeCycle
                    : registeredObject.LifeCycle;


                object obj = null;

                if (registeredObject.Instance == null ||
                    objectLifeCycle == LifeCycle.Default ||
                    objectLifeCycle == LifeCycle.Transient)
                {
                    object[] paramArray = null;

                    if (parameters == null || parameters.Length == 0)
                    {
                        var param = ResolveConstructorParameters(registeredObject);
                        paramArray = param == null ? null : param.ToArray();
                    }

                    obj = registeredObject.CreateInstance(assemblyContext, objectLifeCycle, resolveFrom, paramArray);

                    Debug.Log("Successfully resolved " + registeredObject.TypeToResolve + " as " +
                              registeredObject.ConcreteType + " by " + objectLifeCycle + " from new object");
                    return obj;
                }
                else
                {
                    Debug.Log("Successfully resolved " + registeredObject.TypeToResolve + " as " +
                              registeredObject.ConcreteType + " by " + objectLifeCycle + " from cached instance");

                    return registeredObject.Instance;
                }
            }

            private IEnumerable<object> ResolveConstructorParameters(RegisteredObject registeredObject)
            {
                ConstructorInfo constructorInfo = registeredObject.ConcreteType.GetConstructors().FirstOrDefault();

                if (constructorInfo == null)
                {
                    Debug.Log(
                        "No constructor to resolve object of {0} found, will use the default constructor",
                        registeredObject.ConcreteType);
                    yield break;
                }

                foreach (var parameter in constructorInfo.GetParameters())
                {
                    yield return ResolveObject(parameter.ParameterType);
                }
            }
        }
    }
}