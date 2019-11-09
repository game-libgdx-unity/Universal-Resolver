/**
 * Author:    Vinh Vu Thanh
 * This class is a part of Universal Resolver project that can be downloaded free at 
 * https://github.com/game-libgdx-unity/UnityEngine.IoC
 * (c) Copyright by MrThanhVinh168@gmail.com
 **/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Assertions;
using Object = UnityEngine.Object;
using Unity.Linq;

namespace UnityIoC
{
    public partial class Context
    {
        protected class CacheEqualityComparer : IEqualityComparer<ResolveInput>
        {
            public bool Equals(ResolveInput x, ResolveInput y)
            {
                return x.abstractType == y.abstractType &&
                       x.lifeCycle.IsEqual(y.lifeCycle) &&
                       x.resolveFromType == y.resolveFromType &&
                       x.injectIntoObject == y.injectIntoObject;
            }

            public int GetHashCode(ResolveInput obj)
            {
                var hash = 13;
                hash = (17 * hash) + obj.lifeCycle.GetHashCode();
                hash = (17 * hash) + obj.abstractType.GetHashCode();

                if (obj.resolveFromType != null)
                {
                    hash = (17 * hash) + obj.resolveFromType.GetHashCode();
                }

                if (obj.injectIntoObject != null)
                {
                    hash = (17 * hash) + obj.injectIntoObject.GetHashCode();
                }

                return hash;
            }
        }

        public struct ResolveInput
        {
            public Type abstractType { get; set; }
            public LifeCycle lifeCycle { get; set; }
            public Type resolveFromType { get; set; }
            public object injectIntoObject { get; set; }
            public object[] parameters { get; set; }
        }

        /** not very useful right now... ObjectContext can do what this class does better
        public class ObjectContextContainer : Container
        {
            public object ResolveFrom { get; set; }

            public ObjectContextContainer(
                Context context,
                object resolveFrom,
                BindingSetting bindingData = null
            )
                : base(context)
            {
                ResolveFrom = resolveFrom;
                context.LoadBindingSettingForType(resolveFrom.GetType(), bindingData);
            }

            public override object ResolveObject(Type abstractType, LifeCycle preferredLifeCycle = LifeCycle.Default, object resolveFrom = null,
                params object[] parameters)
            {
                if (ResolveFrom != null)
                {
                    resolveFrom = ResolveFrom;
                }
                
                return base.ResolveObject(abstractType, preferredLifeCycle, resolveFrom, parameters);
            }
            public object ResolveObject(Type abstractType, LifeCycle preferredLifeCycle = LifeCycle.Default,
                params object[] parameters)
            {
                return base.ResolveObject(abstractType, preferredLifeCycle, ResolveFrom, parameters);
            }
        }
        */

        public class Container : IContainer
        {
            private const BindingFlags BindingFlags = System.Reflection.BindingFlags.Instance |
                                                      System.Reflection.BindingFlags.NonPublic |
                                                      System.Reflection.BindingFlags.Public;

            public HashSet<Type> registeredTypes = new HashSet<Type>();

            /// <summary>
            /// cache of processings to find out the appropriate RegisteredObject  
            /// </summary>
            private Dictionary<ResolveInput, RegisteredObject> CachedResolveResults =
                new Dictionary<ResolveInput, RegisteredObject>(new CacheEqualityComparer());


            /// <summary>
            /// cache of bindings which are registered inside context.
            /// </summary>
            public List<RegisteredObject> registeredObjects = new List<RegisteredObject>();

            private readonly Logger debug = new Logger(typeof(Container));

            private Context context;

            public Container(Context context)
            {
                this.context = context;
            }

            public void Dispose()
            {
                debug.Log("Disposing container...");
                foreach (var registeredObject in registeredObjects)
                {
                    registeredObject.Dispose();
                }

                CachedResolveResults.Clear();
                registeredTypes.Clear();

                var prefabTypes = registeredObjects.Where(r => r.LifeCycle == LifeCycle.Prefab)
                    .Select(r => r.AbstractType);

                registeredTypes.RemoveWhere(t => !prefabTypes.Contains(t));
                registeredObjects.RemoveAll(r => r.LifeCycle != LifeCycle.Prefab);
            }

            public void Bind<TTypeToResolve, TConcrete>()
            {
                Bind<TTypeToResolve, TConcrete>(LifeCycle.Transient);
            }

            public RegisteredObject Bind(Type typeToResolve, Type concreteType, LifeCycle lifeCycle = LifeCycle.Default)
            {
                if (concreteType.IsAbstract)
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
                var registeredObject = new RegisteredObject(typeToResolve, concreteType, context, lifeCycle);
                registeredObjects.Add(registeredObject);
                return registeredObject;
            }

            public void Bind<TTypeToResolve, TConcrete>(LifeCycle lifeCycle)
            {
                if (typeof(TConcrete).IsAbstract)
                {
                    throw new InvalidOperationException("Cannot bind empty object of abstract type");
                }

                if (registeredTypes.Contains(typeof(TTypeToResolve)))
                {
                    Unbind<TTypeToResolve>();
                }

                debug.Log("register type: " + typeof(TTypeToResolve));
                registeredTypes.Add(typeof(TTypeToResolve));
                registeredObjects.Add(new RegisteredObject(typeof(TTypeToResolve), typeof(TConcrete), context,
                    lifeCycle));
            }

            public RegisteredObject Bind(InjectIntoBindingData data)
            {
                if (data.ImplementedType.IsAbstract)
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

                            RemoveRegisteredObjectsFromCache(registeredObject);
                            registeredObjects.RemoveAt(i);
                            registeredTypes.Remove(data.AbstractType);
                            i--;
                        }
                    }
                    else if (data.InjectInto == null && registeredObject.InjectInto == null)
                    {
                        debug.Log("Unbind {0} registered for {1}", data.ImplementedType,
                            data.AbstractType);

                        RemoveRegisteredObjectsFromCache(registeredObject);
                        registeredObjects.RemoveAt(i);
                        registeredTypes.Remove(data.AbstractType);
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
                var item = new RegisteredObject(
                    data.AbstractType,
                    data.ImplementedType,
                    null,
                    data.LifeCycle,
                    data.InjectInto);
                item.Prefab = data.Prefab;

                registeredObjects.Add(
                    item);

                return item;
            }

            public void Unbind(Type abstractType)
            {
                for (var i = 0; i < registeredObjects.Count; i++)
                {
                    var registeredObject = registeredObjects[i];

                    if (registeredObject.AbstractType != abstractType)
                        continue;

                    debug.Log("Unbind {0} registered for {1}", registeredObject.ImplementedType,
                        registeredObject.AbstractType);

                    RemoveRegisteredObjectsFromCache(registeredObject);
                    registeredObjects.RemoveAt(i);
                    registeredTypes.Remove(abstractType);
                    i--;
                }
            }

            private void RemoveRegisteredObjectsFromCache(RegisteredObject registeredObject)
            {
                foreach (var item in CachedResolveResults.Where(kvp => kvp.Value == registeredObject).ToList())
                {
                    CachedResolveResults.Remove(item.Key);
                }
            }

            public IEnumerable<RegisteredObject> GetRegisteredObject(Type typeToResolve)
            {
                return registeredObjects.Where(r => r.AbstractType == typeToResolve);
            }

            public void BindInstance(Type typeToResolve, object instance)
            {
                BindInstance(typeToResolve, instance.GetType(), instance);
            }

            public void BindInstance(
                Type typeToResolve,
                Type typeConcrete,
                object instance)
            {
                if (instance == null && (typeConcrete.IsAbstract))
                {
                    throw new InvalidOperationException("Cannot bind null object of abstract type");
                }

                try
                {
                    if (registeredTypes.Contains(typeToResolve))
                    {
                        debug.Log("Already registered type of " + typeToResolve + " will unbind them first");

//                        RemoveRegisteredObjectFromCache(registeredObject);

                        registeredTypes.Remove(typeToResolve);
                        registeredObjects.RemoveAll(r =>
                            r.ImplementedType == typeConcrete && r.AbstractType == typeToResolve);
                    }
                    else
                    {
                        debug.Log("register type: " + typeToResolve);
                        registeredTypes.Add(typeToResolve);
                    }

                    registeredObjects.Add(new RegisteredObject(typeToResolve, typeConcrete, instance, LifeCycle.Singleton));
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

            public void Bind<TTypeToResolve>(
                object instance)
            {
                BindInstance(typeof(TTypeToResolve), instance.GetType(), instance);
            }

            public void Bind<TTypeToResolve>(
                LifeCycle lifeCycle)
            {
                Bind<TTypeToResolve, TTypeToResolve>(lifeCycle);
            }

            public TTypeToResolve ResolveObject<TTypeToResolve>(
                LifeCycle lifeCycle = LifeCycle.Default,
                params object[] parameters)
            {
                return (TTypeToResolve) ResolveObject(typeof(TTypeToResolve), lifeCycle, null, parameters);
            }

            public object ResolveObject(
                Type typeToResolve,
                LifeCycle lifeCycle = LifeCycle.Default,
                params object[] parameters
            )
            {
                return ResolveObject(typeToResolve, lifeCycle, null, parameters);
            }

            public virtual object ResolveObject(
                Type abstractType,
                LifeCycle preferredLifeCycle = LifeCycle.Default,
                object resolveFromObjectParam = null,
                params object[] parameters
            )
            {
                Type resolveFromType = resolveFromObjectParam as Type;

                object resolveFromObject = resolveFromType == null ? resolveFromObjectParam : null;
                
                ResolveInput resolveInput = new ResolveInput();

                //quick return for some particular types & cases.
                if (abstractType.IsValueType)
                {
                    if (parameters != null && parameters.Length == 1 && parameters[0].GetType() == abstractType)
                        return parameters[0];

                    return Activator.CreateInstance(abstractType);
                }

                if (abstractType == typeof(string))
                {
                    if (parameters != null && parameters.Length == 1 && parameters[0] is string)
                        return parameters[0];

                    return String.Empty;
                }

                if (abstractType.IsSubclassOf(typeof(ScriptableObject)) && parameters != null &&
                    parameters.Length == 1 && parameters[0] is string)
                {
                    return Object.Instantiate(MyResources.Load((string) parameters[0]));
                }

                //process for other reference types
                //only cache for non-array types
                if (!abstractType.IsArray)
                {
                    resolveInput.abstractType = abstractType;
                    resolveInput.lifeCycle = preferredLifeCycle;
                    resolveInput.resolveFromType = resolveFromType;
                    resolveInput.injectIntoObject = resolveFromObject;
                    
                    //try to get the result from internal cache
                    if (CachedResolveResults.ContainsKey(resolveInput))
                    {
                        debug.Log("Resolved from internal cache for type {0} as {1}", abstractType, preferredLifeCycle);
                        var cacheResult = CachedResolveResults[resolveInput];
                        var obj = GetInstance(cacheResult, preferredLifeCycle, resolveFromType, parameters);
                        return obj;
                    }
                }

                //start resolving without load from internal cache
                debug.Log("Start Resolve type: " + abstractType);
                debug.Log("preferredLifeCycle: " + preferredLifeCycle);
                if (resolveFromType != null)
                {
                    debug.Log("Resolve from: " + resolveFromType.Name);
                }

                Func<RegisteredObject, bool> filter = null;
                RegisteredObject registeredObject = null;

                if (resolveFromType == null && resolveFromObject == null)
                {
                    debug.Log("Try default process to resolve");
                    var instance = CreateInstanceFromPrefab(abstractType, preferredLifeCycle, resolveInput);
                    if (instance != null)
                    {
                        return instance;
                    }
                    
                    if (registeredObject == null)
                    {
                        filter = o => abstractType.IsAssignableFrom(o.AbstractType) && o.InjectInto == null && o.InjectObject == null; 
                        registeredObject = registeredObjects.FirstOrDefault(filter);
                    }
                    
                    if (registeredObject == null)
                    {
                        debug.Log(
                            "The type {0} has not been registered", abstractType.Name);

                        if (context.searchPrefabFromScene && abstractType.IsAbstract)
                        {
                            //search the abstractType from all root gameObjects and its descendants
                            foreach (var child in AllRootgameObjects)
                            {
                                var component = child.GetComponent(abstractType);

                                if (component != null)
                                {
                                    registeredObject = new RegisteredObject(
                                        abstractType,
                                        abstractType,
                                        context,
                                        preferredLifeCycle);

                                    registeredObject.Prefab = component.gameObject;
                                    registeredObjects.Add(registeredObject);

                                    registeredTypes.Add(abstractType);

                                    return component;
                                }
                            }

                            foreach (var go in AllRootgameObjects)
                            {
                                foreach (var child in go.Descendants())
                                {
                                    var component = child.GetComponent(abstractType);

                                    if (component != null)
                                    {
                                        registeredObject = new RegisteredObject(
                                            abstractType,
                                            abstractType,
                                            context,
                                            preferredLifeCycle);

                                        registeredObject.Prefab = component.gameObject;
                                        registeredObjects.Add(registeredObject);

                                        registeredTypes.Add(abstractType);

                                        return component;
                                    }
                                }
                            }

                            //if the typeToResolve is abstract and can't find from scene,
                            //then we cannot resolve it, throw exceptions
                            throw new InvalidOperationException(
                                "Cannot resolve the abstract type " + abstractType.Name +
                                " with no respective registeredObject!");
                        }

                        debug.Log("trying to register {0} ", abstractType);

                        registeredObject = new RegisteredObject(
                            abstractType,
                            abstractType,
                            context,
                            preferredLifeCycle);


                        if (context.searchPrefabFromScene &&
                            abstractType.IsSubclassOf(typeof(Component)) &&
                            !context.monoScripts.ContainsKey(abstractType))
                        {
                            Component findObjOnScene = null;

                            if (abstractType.IsSubclassOf(typeof(MonoBehaviour)) && AllBehaviours != null &&
                                AllBehaviours.Length > 0)
                            {
                                findObjOnScene =
                                    AllBehaviours.FirstOrDefault(b => abstractType.IsAssignableFrom(b.GetType()));
                            }

                            if (findObjOnScene == null)
                                findObjOnScene = Object.FindObjectOfType(abstractType) as Component;

                            if (findObjOnScene != null)
                            {
                                var asComp = findObjOnScene as Component;

                                if (asComp)
                                {
                                    //register GameObject as prefab
                                    registeredObject.Prefab = asComp.gameObject;
                                    registeredObject.LifeCycle = LifeCycle.Prefab;
                                    //inactive prefab
                                    asComp.gameObject.SetActive(false);
                                }
                            }

                            context.monoScripts[abstractType] = findObjOnScene;
                        }

                        registeredObjects.Add(registeredObject);
                    }

                    debug.Log("resolved with default approach");
                    var obj = GetInstance(registeredObject, preferredLifeCycle, resolveFromType, parameters);
                    //store as cached
                    if (!abstractType.IsArray)
                    {
                        CachedResolveResults[resolveInput] = registeredObject;
                    }

                    return obj;
                }

                ////////////////////////////////////////////////
                debug.Log("Try high priority process for notnull InjectInto registeredObject");

                if (resolveFromType != null)
                    filter = o =>
                        o.AbstractType == abstractType && o.InjectInto != null && resolveFromType == o.InjectInto;
                else
                    filter = o =>
                        o.AbstractType == abstractType && o.InjectObject != null && resolveFromObject == o.InjectObject;

                registeredObject = registeredObjects.FirstOrDefault(filter);
                if (registeredObject != null)
                {
                    //using binding attribute to filter registered objects
                    debug.Log("Binding of " + registeredObject.ImplementedType + " has inject into: " +
                              registeredObject.InjectInto);
                    debug.Log("resolve from: " + registeredObject.InjectInto +
                              " by inject into from RegisteredObject");
                    debug.Log("resolved by high priority approach");
                    var obj = GetInstance(registeredObject, preferredLifeCycle, resolveFromType, parameters);

                    //store as cached
                    //only cache for non-array types
                    if (!abstractType.IsArray)
                    {
                        CachedResolveResults[resolveInput] = registeredObject;
                    }

                    return obj;
                }

                debug.Log("High priority process is failed");
                debug.Log("Try lower priority process for null Inject Into registeredObject");

                filter = o => o.AbstractType == abstractType && o.InjectInto == null;

                registeredObject = registeredObjects.FirstOrDefault(filter);
                if (registeredObject != null)
                {
                    debug.Log("resolved with lower priority approach");
                    var obj = GetInstance(registeredObject, preferredLifeCycle, resolveFromType, parameters);

                    //store as cached
                    //only cache for non-array types
                    if (!abstractType.IsArray)
                    {
                        CachedResolveResults[resolveInput] = registeredObject;
                    }

                    return obj;
                }


                //Here is the recursive method, so can't cache from this part
                debug.Log("Lower priority process is failed");
                debug.Log("Lowest priority process: Create a new registeredObject to resolve");
                debug.Log("trying to register {0} ", abstractType);

                registeredObject = new RegisteredObject(
                    abstractType,
                    abstractType,
                    context,
                    preferredLifeCycle);

                registeredObjects.Add(registeredObject);

                var resolveObject = GetInstance(registeredObject, preferredLifeCycle, resolveFromType, parameters);
                //store as cached
                if (!abstractType.IsArray)
                {
                    CachedResolveResults[resolveInput] = registeredObject;
                }

                return resolveObject;
            }

            private object CreateInstanceFromPrefab(Type abstractType, LifeCycle preferredLifeCycle,
                ResolveInput resolveInput)
            {
                object resolveObject = null;
                RegisteredObject registeredObject;
                if (abstractType.IsSubclassOf(typeof(Component)) &&
                    (preferredLifeCycle == LifeCycle.Default || preferredLifeCycle == LifeCycle.Transient))
                {
                    //try to look for the component from prefab
                    registeredObject =
                        registeredObjects.FirstOrDefault(r => r.Prefab && r.AbstractType == abstractType);
                    if (registeredObject != null)
                    {
                        //store as cached
                        if (!abstractType.IsArray)
                        {
                            CachedResolveResults[resolveInput] = registeredObject;
                        }

                        resolveObject = context.CreateInstance(registeredObject.Prefab)
                            .GetComponent(registeredObject.ImplementedType);
                    }
                }

                return resolveObject;
            }

            private object GetInstance(RegisteredObject registeredObject,
                LifeCycle preferredLifeCycle = LifeCycle.Default,
                object resolveFrom = null,
                params object[] parameters)
            {
                var objectLifeCycle = preferredLifeCycle != LifeCycle.Default
                    ? preferredLifeCycle
                    : registeredObject.LifeCycle;

                if (registeredObject.Instance == null ||
                    objectLifeCycle == LifeCycle.Default ||
                    objectLifeCycle == LifeCycle.Transient)
                {
                    object[] paramArray = null;

                    if (parameters == null || parameters.Length == 0)
                    {
                        paramArray = ResolveConstructorParameters(registeredObject.ImplementedType).ToArray();
                    }
                    else if (parameters.GetType().GetElementType() == typeof(Type))
                    {
                        var elementTypes = parameters.Select(p => p as Type).ToArray();
                        paramArray = ResolveConstructorParameters(registeredObject.ImplementedType, elementTypes)
                            .ToArray();
                    }
                    else
                    {
                        paramArray = parameters;
                    }

                    var obj = registeredObject.CreateInstance(
                        context,
                        objectLifeCycle,
                        resolveFrom,
                        paramArray
                    );

                    debug.Log("Successfully resolved " + registeredObject.AbstractType + " as " +
                              registeredObject.ImplementedType + " by " + objectLifeCycle);
                    return obj;
                }

                debug.Log("Successfully resolved " + registeredObject.AbstractType + " as " +
                          registeredObject.ImplementedType + " by " + objectLifeCycle + " from cached instance");

                return registeredObject.Instance;
            }

            private IEnumerable<object> ResolveConstructorParameters(Type type,
                Type[] elementTypes = null)
            {
                ConstructorInfo constructorInfo = null;

                if (elementTypes == null || elementTypes.Length == 0)
                {
                    constructorInfo = type
                        .GetConstructors(BindingFlags)
                        .FirstOrDefault();
                }
                else
                {
                    constructorInfo = type
                        .GetConstructor(elementTypes);
                }

                if (constructorInfo == null)
                {
                    debug.Log(
                        "No constructor to resolve object of {0} found, will use the default constructor",
                        type);
                    yield break;
                }

                foreach (var parameter in constructorInfo.GetParameters())
                {
                    yield return ResolveObject(parameter.ParameterType, LifeCycle.Default, type, null);
                }
            }
        }
    }
}