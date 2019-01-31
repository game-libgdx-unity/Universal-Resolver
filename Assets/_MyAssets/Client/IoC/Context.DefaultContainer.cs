using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;

namespace SimpleIoc
{
    public partial class Context
    {
        private class DefaultContainer : IContainer
        {
            private HashSet<Type> registeredTypes = new HashSet<Type>();
            private IList<RegisteredObject> registeredObjects = new List<RegisteredObject>();

            private Context context;
            private Type injectInto;

            public DefaultContainer(Context context, Type injectInto)
            {
                this.context = context;
                this.injectInto = injectInto;
            }

            public void Unload()
            {
                Debug.Log("Disposing container...");
                registeredTypes = new HashSet<Type>();
                registeredObjects = new List<RegisteredObject>();
            }

            public void Bind<TTypeToResolve, TConcrete>()
            {
                Bind<TTypeToResolve, TConcrete>(LifeCycle.Transient);
            }

            public void Bind(Type typeToResolve, Type concreteType, LifeCycle lifeCycle = LifeCycle.Default)
            {
                if (concreteType.IsAbstract || concreteType.IsInterface)
                {
                    throw new InvalidOperationException("Cannot bind to empty object of abstract type");
                }

                if (registeredTypes.Contains(typeToResolve))
                {
                    Debug.LogFormat("{0} already registered {1} time", typeToResolve.ToString(),
                        registeredObjects.FirstOrDefault(o => o.TypeToResolve == typeToResolve));
                }
                else
                {
                    Debug.Log("registered type: " + concreteType + " for " + typeToResolve + " as " + lifeCycle);
                    registeredTypes.Add(typeToResolve);
                }

                registeredObjects.Add(new RegisteredObject(typeToResolve, concreteType, context.Binding, lifeCycle));
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
                registeredObjects.Add(new RegisteredObject(typeof(TTypeToResolve), typeof(TConcrete), context.Binding,
                    lifeCycle));
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

                    registeredObjects.Add(new RegisteredObject(typeToResolve, instance, context.Binding));
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

            public object ResolveObject(Type typeToResolve, object injectInto = null,
                LifeCycle preferredLifeCycle = LifeCycle.Default,
                params object[] parameters)
            {
                Func<RegisteredObject, bool> filter = null;

                if (context.requirePreRegistered)
                    filter = o => o.TypeToResolve == typeToResolve ||
                                  o.ConcreteType == typeToResolve;
                else
                    filter = o => o.TypeToResolve == typeToResolve ||
                                  o.ConcreteType == typeToResolve ||
                                  o.ConcreteType.IsSubclassOf(typeToResolve) ||
                                  o.ConcreteType.GetInterfaces().Contains(typeToResolve);


                var numOfRegisters = registeredObjects.Count(filter);

                if (injectInto == null || numOfRegisters <= 1)
                {
                    var registeredObject = registeredObjects.FirstOrDefault(filter);
                    if (registeredObject == null)
                    {
                        Debug.LogFormat(
                            "The type {0} has not been registered", typeToResolve.Name);

                        //if the typeToResolve is abstract, then we cannot resolve it, throw exceptions
                        if (typeToResolve.IsAbstract || typeToResolve.IsInterface)
                        {
                            throw new InvalidOperationException("Cannot resolve the typeToResolve which is abstract");
                        }
                        
                        Debug.LogFormat("trying to register {0} ", typeToResolve);

                        registeredObject = new RegisteredObject(typeToResolve, typeToResolve, context.Binding,
                            preferredLifeCycle);
                        registeredObjects.Add(registeredObject);
                    }

                    var obj = GetInstance(registeredObject, preferredLifeCycle, parameters);

                    return obj;
                }

                var typeInjectInto = injectInto.GetType();

                foreach (var registeredObject in registeredObjects.Where(filter))
                {
                    var binding = registeredObject.BindingAttribute;
                    if (binding == null)
                    {
                        Debug.LogError("You need binding attribute to resolve a type with multiply implements of " +
                                       registeredObject.ConcreteType);
                        return null;
                    }

                    if (binding.InjectInto == null) //cannot process with empty inject into.
                    {
                        continue;
                    }

                    if (binding.InjectInto.FirstOrDefault(t => t == typeInjectInto) != null)
                    {
                        return GetInstance(registeredObject, preferredLifeCycle, parameters);
                    }

                    return ResolveObject(typeToResolve, null, preferredLifeCycle, parameters);
                }

                Debug.LogError("Unable to resolve due to too many implemented type for " + typeToResolve);
                return null;
            }

            private object GetInstance(RegisteredObject registeredObject,
                LifeCycle preferredLifeCycle = LifeCycle.Default,
                params object[] parameters)
            {
                var objectLifeCycle = preferredLifeCycle != LifeCycle.Default
                    ? preferredLifeCycle
                    : registeredObject.LifeCycle;
                Debug.Log("Resolved " + registeredObject.TypeToResolve + " as " +
                          registeredObject.ConcreteType + " by " + objectLifeCycle);

                object obj = null;

                if (registeredObject.Instance == null ||
                    objectLifeCycle == LifeCycle.Default ||
                    objectLifeCycle == LifeCycle.Transient)
                {
                    object[] paramArray = null;

                    if (parameters == null || parameters.Length == 0)
                    {
                        var param = ResolveConstructorParameters(registeredObject).ToArray();
                        paramArray = param.ToArray();
                    }

                    obj = registeredObject.CreateInstance(context, objectLifeCycle, paramArray);
                }
                else
                {
                    return registeredObject.Instance;
                }

                return obj;
            }

            private IEnumerable<object> ResolveConstructorParameters(RegisteredObject registeredObject)
            {
                ConstructorInfo constructorInfo = registeredObject.ConcreteType.GetConstructors().FirstOrDefault();

                if (constructorInfo == null)
                {
                    Debug.LogFormat(
                        "No constructor to resolve object of {0} found, using default constructor",
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