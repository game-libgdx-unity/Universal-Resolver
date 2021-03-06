﻿/**
 * Author:    Vinh Vu Thanh
 * This class is a part of Universal Resolver project that can be downloaded free at 
 * https://github.com/game-libgdx-unity/UnityEngine.IoC
 * (c) Copyright by MrThanhVinh168@gmail.com    
 **/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace UnityIoC
{
    public partial class Context
    {
        public static GameObject TempGameObject = null;

        public string[] assetPaths =
        {
            "{type}",
            "{scene}/{type}",
            "Prefabs/{type}",
            "Prefabs/{scene}/{type}",
        };

        public class RegisteredObject : IDisposable
        {
            private static Logger Debug = new Logger(typeof(RegisteredObject));
            
            public Type AbstractType { get; private set; }

            public Type ImplementedType { get; private set; }

            /// <summary>
            /// Prefab can be loaded instead of loading of ImplementType from bindingSetting files
            /// </summary>
            public GameObject Prefab { get; set; }

            public object Instance { get; private set; }

            public LifeCycle LifeCycle { get; set; }

            public Type InjectInto { get; private set; }
            
            public object InjectObject { get; private set; }
            
            public RegisteredObject(
                Type abstractType,
                Type concreteType,
                    Context context,
                LifeCycle lifeCycle = LifeCycle.Default) :
                this(abstractType, concreteType, null, lifeCycle, null)
            {
            }

            public RegisteredObject(
                Type abstractType,
                Type concreteType,
                object instance,
                LifeCycle lifeCycle = LifeCycle.Default,
                Type injectInto = null,
                object injectOject = null
            )
            {
                Instance = instance;
                AbstractType = abstractType;

                LifeCycle = lifeCycle;
                
                if (injectInto != null)
                {
                    Debug.Log("Inject into: " + InjectInto.Name);
                    InjectInto = injectInto;
                }

                if (InjectObject != null)
                {
                    Debug.Log("Inject into: " + injectOject);
                    InjectObject = injectOject;
                }

                if (instance != null)
                {
                    LifeCycle = LifeCycle.Singleton;
                    ImplementedType = instance.GetType();
                }
                else if (concreteType != null)
                {
                    ImplementedType = concreteType;
                }
                else
                {
                    throw new InvalidOperationException("either instance or concreteType must not be null");
                }

                Debug.Log("Life cycle: " + LifeCycle);
            }

            public object CreateInstance(Context context,
                LifeCycle preferredLifeCycle = LifeCycle.Default,
                object resolveFrom = null,
                params object[] args)
            {
                var objectLifeCycle = preferredLifeCycle == LifeCycle.Default ? LifeCycle : preferredLifeCycle;
                Debug.Log("Life cycle: " + objectLifeCycle);


                var isTransient = objectLifeCycle.IsEqual(LifeCycle.Transient) ||
                                  (objectLifeCycle & LifeCycle.Transient) == LifeCycle.Transient;

                var isSingleton = objectLifeCycle == LifeCycle.Singleton ||
                                  (objectLifeCycle & LifeCycle.Singleton) == LifeCycle.Singleton;

                var isPrefab = objectLifeCycle == LifeCycle.Prefab ||
                               (objectLifeCycle & LifeCycle.Prefab) == LifeCycle.Prefab;

                object instance = null;

                if (Instance == null || !isSingleton)
                {
                    if (ImplementedType.IsSubclassOf(typeof(Component)))
                    {
                        //resolve by registeredObject's prefab
                        if (Prefab != null)
                        {
                            var go = context.CreateInstance(Prefab);
                            go.transform.SetParent(Prefab.transform.parent);
                            instance = go.GetComponent(ImplementedType);
                            return instance;
                        }

                        //resolve by Prefab from resources or asset bundles
                        instance = TryGetComponent(context, ImplementedType,
                            preferredLifeCycle, resolveFrom);

                        var isUnityBehaviour = ImplementedType.Namespace != null &&
                                               ImplementedType.Namespace.StartsWith("UnityEngine");

                        if (instance != null && !isUnityBehaviour)
                        {
                            context.ProcessInjectAttribute((instance as Component).gameObject);
                        }
                    }
                    else
                    {
                        var defaultConstructor = ImplementedType.GetConstructors()
                            .FirstOrDefault(c => c.GetParameters().Length == 0);
                        if (defaultConstructor == null && args.Length == 0)
                        {
                            // if args are empty, cannot resolve with non-default Constructors
                            // this is most likely happened when you [inject] a field from Mono-behaviour
                            // that doesn't have the default constructor
                            return null;
                        }

                        instance = Activator.CreateInstance(ImplementedType, args);
                        context.ProcessInjectAttribute(instance);
                    }
                }

                //return if this is marked as prefab
                if (isPrefab || isTransient)
                {
//                    Instance = GameObject;
                    return instance;
                }

                //cache if this is marked as singleton
                if (isSingleton)
                {
                    Instance = instance;
                    return Instance;
                }

                return instance;
            }

            private Component TryGetComponent(Context context, Type concreteType,
                LifeCycle lifeCycle, object resolveFrom)
            {
                string TypeName = concreteType.Name;
                Component instance = null;

                //if resolve as singleton, try to get component from an existing one in current scene or from cached
                if ((lifeCycle == LifeCycle.Singleton ||
                     lifeCycle == LifeCycle.Prefab ||
                     (lifeCycle & LifeCycle.Singleton) == LifeCycle.Singleton ||
                     (lifeCycle & LifeCycle.Prefab) == LifeCycle.Prefab) &&
                    concreteType.IsSubclassOf(typeof(MonoBehaviour)))
                {
                    //try to get from caches first                    
                    if (context.monoScripts.ContainsKey(concreteType))
                    {
                        instance = context.monoScripts[concreteType];
                    }
                    else
                    {
                        //try to find component from current scene then
                        if (context.searchPrefabFromScene && Instance == null)
                        {
                            if (concreteType.IsSubclassOf(typeof(MonoBehaviour)) && AllBehaviours != null &&
                                AllBehaviours.Length > 0)
                            {
                                instance = AllBehaviours.FirstOrDefault(concreteType.IsInstanceOfType);
                            }
                        }
                        else
                        {
                            instance = Instance as MonoBehaviour;
                        }

                        if (instance)
                        {
                            context.monoScripts[concreteType] = instance as MonoBehaviour;
                        }
                    }

                    if (instance)
                    {
                        if (lifeCycle == LifeCycle.Prefab)
                        {
                            GameObject gameObject;
                            (gameObject = instance.gameObject).SetActive(false);
                            InjectIntoBindingData d = new InjectIntoBindingData();
                            d.AbstractType = AbstractType;
                            d.ImplementedType = ImplementedType;
                            d.LifeCycle = LifeCycle.Prefab;
                            context.container.Bind(d).Prefab = gameObject;
                        }

                        Debug.Log("Found {0} component on gameObject {1} as {2} from current scene",
                            TypeName,
                            instance.gameObject.name,
                            lifeCycle);
                        Instance = instance;
                        return instance;
                    }
                }

                //search for prefabs of this component type from asset paths
                GameObject prefab = null;

                if (lifeCycle == LifeCycle.Prefab && Prefab)
                {
                    prefab = Prefab;
                }
                else
                {
                    for (var i = 0; i < context.assetPaths.Length; i++)
                    {
                        var path = context.assetPaths[i]
                            .Replace("{scene}",
                                SceneManager.GetActiveScene().name)
                            .Replace("{type}", TypeName)
                            .Replace("{id}", String.Empty);

                        prefab = MyResources.Load<GameObject>(path);
                        if (prefab)
                        {
                            break;
                        }
                    }
                }

                if (prefab)
                {
                    //cache prefab from bundles/resources 
                    Prefab = prefab;

                    //create an instance from this prefab
                    Debug.Log("Found prefab for {0} .......", TypeName);
                    GameObject prefabInstance = Object.Instantiate(prefab);

                    if (prefabInstance.GetComponent(TypeName) == null)
                    {
                        Debug.Log("Found component {0} in prefab children", TypeName);
                        instance = prefabInstance.GetComponentInChildren(concreteType);
                    }
                    else
                    {
                        Debug.Log("Found component {0} in the prefab", TypeName);
                        instance = prefabInstance.GetComponent(TypeName);
                    }
                }
                else
                {
                    Debug.Log("Not found prefab for {0} in the prefab, created a new object",
                        TypeName);

                    var gameObject = resolveFrom as GameObject;
                    if (gameObject != null)
                    {
                        instance = gameObject.AddComponent(concreteType);
                    }
                    else
                    {
                        var monoBehaviour = resolveFrom is Component;
                        if (monoBehaviour && (lifeCycle & LifeCycle.Component) == LifeCycle.Component)
                        {
                            instance = ((Component) resolveFrom).gameObject.AddComponent(concreteType);
                        }
                        else
                        {
                            if (TempGameObject)
                            {
                                instance = TempGameObject.AddComponent(concreteType);
                                TempGameObject = null;
                            }
                            else
                            {
                                instance = new GameObject().AddComponent(concreteType);
                            }
                        }
                    }
                }

                return instance;
            }

            public void Dispose()
            {
                var disposable = Instance as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }

                Instance = null;
            }
        }

        private static IDictionary<Type, ICollection<ValidState>> validatorlist;

        public static bool ValidateData(Type type, ref object data, When when)
        {
            var result = true;
            var validStates = GetValidators(type);
            foreach (var validState in validStates)
            {
                if (validState.when.IsEqual(when))
                {
                    var validator = validState.predicate;
                    if (!validator(ref data))
                    {
                        result = false;
                        var exception = new InvalidDataException(validState.message);
                        exception.Data.Add("data", data.Clone());
                        onEventRaised.Value = exception;
                        break;
                    }
                }
            }

            return result;
        }

        public static bool RemoveConstraint(ValidState.Predicate validator, string msg = null)
        {
            var methodInfo = validator.Method;
            var dataType = methodInfo.GetParameters().FirstOrDefault()?.ParameterType;

            return RemoveConstraint(dataType, msg);
        }
    }
}