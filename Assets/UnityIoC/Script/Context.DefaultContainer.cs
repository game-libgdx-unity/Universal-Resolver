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

            internal List<RegisteredObject> registeredObjects = new List<RegisteredObject>();

            private readonly Logger debug = new Logger(typeof(DefaultContainer));

            private AssemblyContext assemblyContext;

            public DefaultContainer(AssemblyContext assemblyContext)
            {
                this.assemblyContext = assemblyContext;
            }

            public void Dispose()
            {
                debug.Log("Disposing container...");
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
                    debug.Log("{0} already registered {1} time", typeToResolve.ToString(),
                        registeredObjects.Count(o => o.AbstractType == typeToResolve));
                }
                else
                {
                    debug.Log("new type registered: " + typeToResolve + " as " + lifeCycle);
                    registeredTypes.Add(typeToResolve);
                }

                debug.Log("Add registeredObject: " + concreteType + " for " + typeToResolve + " as " + lifeCycle);
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
                    debug.Log("Cannot register a Type twice");
                    return;
                }

                debug.Log("register type: " + typeof(TTypeToResolve));
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

                //unbind existing ones
                for (var i = 0; i < registeredObjects.Count; i++)
                {
                    var registeredObject = registeredObjects[i];

                    if (registeredObject.AbstractType != data.AbstractType)
                        continue;

                    if (data.EnableInjectInto)
                    {
                        if (registeredObject.InjectInto != null &&
                            data.InjectInto == registeredObject.InjectInto)
                        {
                            debug.Log("Unbind {0} registered for {1}", data.ImplementedType,
                                data.AbstractType);
                            registeredObjects.RemoveAt(i);
                            i--;
                        }
                    }
                    else if (data.InjectInto == null && registeredObject.InjectInto == null)
                    {
                        debug.Log("Unbind {0} registered for {1}", data.ImplementedType,
                            data.AbstractType);
                        registeredObjects.RemoveAt(i);
                        i--;
                    }
                }

                //add new registeredObject
                debug.Log("register type: " + data.AbstractType + " for " + data.ImplementedType + " when inject into "
                          + (data.InjectInto == null ? "none" : data.InjectInto.Name));

                //add registered Type
                if (!registeredTypes.Contains(data.AbstractType))
                {
                    registeredTypes.Add(data.AbstractType);
                }

                //add registered Object
                registeredObjects.Add(
                    new RegisteredObject(
                        data.AbstractType,
                        data.ImplementedType,
                        null,
                        data.LifeCycle,
                        data.InjectInto));
            }

            public IEnumerable<AssemblyContext.RegisteredObject> GetRegisteredObject(Type typeToResolve)
            {
                return registeredObjects.Where(r => r.AbstractType == typeToResolve);
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
                        debug.Log("Already registered type of " + typeToResolve + " will unbind them first");
                        registeredObjects.RemoveAll(r =>
                            r.ImplementedType == typeConcrete && r.AbstractType == typeToResolve);
                    }
                    else
                    {
                        debug.Log("register type: " + typeToResolve);
                        registeredTypes.Add(typeToResolve);
                    }

                    registeredObjects.Add(new RegisteredObject(typeToResolve, instance));
                }
                catch (Exception ex)
                {
                    debug.LogError(ex.Message);
                    debug.LogError(ex.StackTrace);
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
                return (TTypeToResolve) ResolveObject(typeof(TTypeToResolve), lifeCycle, null, parameters);
            }

            public object Resolve(Type typeToResolve, LifeCycle lifeCycle = LifeCycle.Default,
                params object[] parameters)
            {
                return ResolveObject(typeToResolve, lifeCycle, null, parameters);
            }

            public object ResolveObject(Type abstractType,
                LifeCycle preferredLifeCycle = LifeCycle.Default,
                object resolveFrom = null,
                params object[] parameters)
            {
                debug.Log("Start Resolve type: " + abstractType);
                debug.Log("preferredLifeCycle: " + preferredLifeCycle);
                Func<RegisteredObject, bool> filter = null;

                RegisteredObject registeredObject = null;

                if (resolveFrom == null || (preferredLifeCycle & LifeCycle.Component) == LifeCycle.Component)
                {
                    debug.Log("Try default process to resolve");

                    filter = o => o != null && o.AbstractType == abstractType && o.InjectInto == null;

                    registeredObject = registeredObjects.FirstOrDefault(filter);
                    if (registeredObject == null)
                    {
                        debug.Log(
                            "The type {0} has not been registered", abstractType.Name);

                        //if the typeToResolve is abstract, then we cannot resolve it, throw exceptions
                        if (abstractType.IsAbstract || abstractType.IsInterface)
                        {
                            throw new InvalidOperationException(
                                "Cannot resolve the abstract type " + abstractType.Name +
                                " with no respective registeredObject!");
                        }

                        debug.Log("trying to register {0} ", abstractType);

                        registeredObject = new RegisteredObject(
                            abstractType,
                            abstractType,
                            assemblyContext,
                            preferredLifeCycle);

                        registeredObjects.Add(registeredObject);
                    }

                    debug.Log("resolve with default approach");
                    var obj = GetInstance(registeredObject, preferredLifeCycle, resolveFrom, parameters);

                    return obj;
                }
                debug.Log("ResolveFrom is not null");
                debug.Log("Try high priority process for notnull InjectInto registeredObject");

                filter = o => o != null && o.AbstractType == abstractType && resolveFrom.GetType() == o.InjectInto;

                registeredObject = registeredObjects.FirstOrDefault(filter);
                if (registeredObject != null)
                {
                    //using binding attribute to filter registered objects
                    var injectFromType = registeredObject.InjectInto;
                    if (injectFromType != null)
                    {
                        debug.Log("Binding of " + registeredObject.ImplementedType + " has inject into: " +
                                  injectFromType);
                        debug.Log("resolve from: " + registeredObject.InjectInto +
                                  " by inject into from RegisteredObject");
                        debug.Log("resolve with high priority approach");
                        return GetInstance(registeredObject, preferredLifeCycle, resolveFrom, parameters);
                    }
                }
                debug.Log("High priority process is failed");
                debug.Log("Try lower priority process for null Inject Into registeredObject");

                filter = o => o != null && o.AbstractType == abstractType && null == o.InjectInto;

                registeredObject = registeredObjects.FirstOrDefault(filter);
                if (registeredObject != null)
                {
                    debug.Log("resolve with lower priority approach");
                    return GetInstance(registeredObject, preferredLifeCycle, resolveFrom, parameters);
                }
                debug.Log("Lower priority process is failed");
                debug.Log("Worst case happened: Resolve without referring the resolveFrom object");
                return ResolveObject(abstractType, preferredLifeCycle, null, parameters);
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

                    debug.Log("Successfully resolved " + registeredObject.AbstractType + " as " +
                              registeredObject.ImplementedType + " by " + objectLifeCycle + " from new object");
                    return obj;
                }

                debug.Log("Successfully resolved " + registeredObject.AbstractType + " as " +
                          registeredObject.ImplementedType + " by " + objectLifeCycle + " from cached instance");

                return registeredObject.Instance;
            }

            private IEnumerable<object> ResolveConstructorParameters(RegisteredObject registeredObject)
            {
                ConstructorInfo constructorInfo = registeredObject.ImplementedType.GetConstructors().FirstOrDefault();

                if (constructorInfo == null)
                {
                    debug.Log(
                        "No constructor to resolve object of {0} found, will use the default constructor",
                        registeredObject.ImplementedType);
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