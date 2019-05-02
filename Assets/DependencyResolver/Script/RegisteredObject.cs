﻿/**
 * Author:    Vinh Vu Thanh
 * This class is a part of Universal Resolver project that can be downloaded free at 
 * https://github.com/game-libgdx-unity/UnityEngine.IoC
 * (c) Copyright by MrThanhVinh168@gmail.com    
 **/

using System;
using System.Linq;
using Unity.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UnityIoC
{
    public partial class Context
    {
        public class RegisteredObject : IDisposable
        {
            private static Logger Debug = new Logger(typeof(RegisteredObject));
            public Type AbstractType { get; private set; }

            public Type ImplementedType { get; private set; }

            /// <summary>
            /// Prefab can be loaded instead of loading of ImplementType from bindingSetting files
            /// </summary>
            public GameObject GameObject { get; set; }

            public object Instance { get; private set; }

            public LifeCycle LifeCycle { get; set; }

            public Type InjectInto { get; private set; }

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
                object instance) :
                this(abstractType, instance.GetType(), instance)
            {
            }

            public RegisteredObject(
                Type abstractType,
                Type concreteType,
                object instance,
                LifeCycle lifeCycle = LifeCycle.Default,
                Type injectInto = null
            )
            {
                Instance = instance;
                AbstractType = abstractType;

                LifeCycle = lifeCycle;
                InjectInto = injectInto;

                if (InjectInto != null)
                {
                    Debug.Log("Inject into: " + InjectInto.Name);
                }

                if (instance != null)
                {
                    if (abstractType == null)
                    {
                        abstractType = instance.GetType();
                    }

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
                        if (GameObject != null)
                        {
                            var go = context.CreateInstance(GameObject);
                            go.transform.SetParent(GameObject.transform.parent);
                            instance = go.GetComponent(ImplementedType);
                            return instance;
                        }

                        //resolve by Prefab from resources or asset bundles
                        instance = TryGetGameObject(context, ImplementedType, ImplementedType.Name,
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
                        
                        var defaultConstructor = ImplementedType.GetConstructors().FirstOrDefault(c=>c.GetParameters().Length == 0);
                        if (defaultConstructor == null && args.Length == 0)
                        {
                            // if args are empty, cannot resolve with non-default Constructors
                            // this is most likely happened when you [inject] a field from Mono-behaviour
                            // that doesn't have a default constructor
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

            private Component TryGetGameObject(Context context, Type concreteType,
                string TypeName, LifeCycle lifeCycle, object resolveFrom)
            {
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
                        if (Instance == null)
                        {
                            if (concreteType.IsSubclassOf(typeof(MonoBehaviour)) && Context.Behaviours != null &&
                                Context.Behaviours.Length > 0)
                            {
                                instance =
                                    Context.Behaviours.FirstOrDefault(
                                        b => concreteType.IsAssignableFrom(b.GetType()));
                            }

                            if (instance == null)
                                instance = Object.FindObjectOfType(concreteType) as MonoBehaviour;
                        }
                        else
                        {
                            instance = Instance as MonoBehaviour;
                        }

                        context.monoScripts[concreteType] = instance as MonoBehaviour;
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
                            context.container.Bind(d).GameObject = gameObject;
                        }
                        
                        Debug.Log("Found {0} component on gameObject {1} as {2} from current scene",
                            TypeName,
                            instance.gameObject.name,
                            lifeCycle);
                        Instance = instance;
                        return instance;
                    }
                }

                //search for prefabs of this component type from resources path folders
                GameObject prefab;

                if (lifeCycle == LifeCycle.Prefab && GameObject)
                {
                    prefab = GameObject;
                }
                else
                {
                    prefab = MyResources.Load<GameObject>(TypeName);
                    if (!prefab) MyResources.Load<GameObject>(string.Format("bundles/{0}", TypeName));
                    if (!prefab) prefab = Resources.Load<GameObject>(string.Format("prefabs/scenes/{0}", TypeName));
                    if (!prefab) prefab = Resources.Load<GameObject>(string.Format("scenes/{0}", TypeName));
                    if (!prefab) prefab = Resources.Load<GameObject>(string.Format("prefabs/{0}", TypeName));
                    if (!prefab) prefab = Resources.Load<GameObject>(TypeName);
                }

                if (prefab)
                {
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
                    Debug.Log("Not found prefab for {0} in the prefab, created a new game object",
                        TypeName);

                    var monoBehaviour = resolveFrom as Component;
                    if (monoBehaviour != null && (lifeCycle & LifeCycle.Component) == LifeCycle.Component)
                    {
                        instance = monoBehaviour.gameObject.AddComponent(concreteType);
                    }
                    else
                    {
                        instance = new GameObject().AddComponent(concreteType);
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
    }
}