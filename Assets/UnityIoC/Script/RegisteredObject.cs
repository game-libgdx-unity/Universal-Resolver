/**
 * Author:    Vinh Vu Thanh
 * This class is a part of Unity IoC project that can be downloaded free at 
 * https://github.com/game-libgdx-unity/UnityEngine.IoC
 * (c) Copyright by MrThanhVinh168@gmail.com    
 **/

using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UnityIoC
{
    public partial class AssemblyContext
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

            public LifeCycle LifeCycle { get; private set; }

            public Type InjectInto { get; private set; }

            public RegisteredObject(
                Type abstractType,
                Type concreteType,
                AssemblyContext assemblyContext,
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

            public object CreateInstance(AssemblyContext assemblyContext,
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


                if (Instance == null)
                {
                    if (GameObject != null || ImplementedType.IsSubclassOf(typeof(Component)))
                    {
                        //cache 
                        if (Instance == null)
                        {
                            //resolve by registeredObject's prefab
                            if (GameObject != null)
                            {
                                GameObject instance;

                                //don't create new Instance if the lifecycle is prefab
                                if ((preferredLifeCycle & LifeCycle.Prefab) == LifeCycle.Prefab)
                                {
                                    instance = GameObject;
                                }
                                else
                                {
                                    //create a new instance
                                    var monoBehaviour = resolveFrom as Component;
                                    if ((preferredLifeCycle & LifeCycle.Component) == LifeCycle.Component &&
                                        monoBehaviour != null)
                                    {
                                        instance = Object.Instantiate(GameObject, monoBehaviour.transform);
                                    }
                                    else
                                    {
                                        instance = Object.Instantiate(GameObject);
                                    }
                                }

                                Instance = instance.GetComponent(AbstractType);
                            }

                            //resolve by Prefab from resources or asset bundles
                            if (Instance == null)
                            {
                                Instance = TryGetGameObject(assemblyContext, ImplementedType, ImplementedType.Name,
                                    preferredLifeCycle, resolveFrom);
                            }

                            if (isTransient)
                            {
                                (Instance as Component).gameObject.SetActive(false);
                                assemblyContext.ProcessInjectAttribute((Instance as Component).gameObject);
                            }
                        }
                    }
                    else
                    {
                        Instance = Activator.CreateInstance(ImplementedType, args);
                        assemblyContext.ProcessInjectAttribute(Instance);
                    }
                }

                if (isTransient)
                {
                    object instance;

                    if (ImplementedType.IsSubclassOf(typeof(Component)))
                    {
                        var component = Instance as Component;

                        //instantiate
                        var monoBehaviour = resolveFrom as Component;

                        if ((preferredLifeCycle & LifeCycle.Component) == LifeCycle.Component && monoBehaviour != null)
                        {
                            instance = Object.Instantiate(component, monoBehaviour.transform);
                        }
                        else
                        {
                            instance = Object.Instantiate(component);
                        }

                        //activate
                        assemblyContext.ProcessInjectAttribute((instance as Component).gameObject);
                        (instance as Component).gameObject.SetActive(true);
                    }
                    else
                    {
                        //instance below should be cloned from Instance
                        instance = Activator.CreateInstance(ImplementedType, args);
                        assemblyContext.ProcessInjectAttribute(instance);
                    }

                    return instance;
                }

                if (isSingleton)
                {
                    return Instance;
                }

                Debug.LogError("RegisterObject failed to resolve type " + AbstractType.Name);
                return null;
            }


            private Component TryGetGameObject(AssemblyContext assemblyContext, Type concreteType,
                string TypeName, LifeCycle lifeCycle, object resolveFrom)
            {
                Component instance;

                //if resolve as singleton, try to get component from an existing one in current scene or from cached
                if ((lifeCycle == LifeCycle.Singleton ||
                     (lifeCycle & LifeCycle.Singleton) == LifeCycle.Singleton) &&
                    concreteType.IsSubclassOf(typeof(MonoBehaviour)))
                {
                    //try to get from caches first                    
                    if (assemblyContext.monoScripts.ContainsKey(concreteType))
                    {
                        instance = assemblyContext.monoScripts[concreteType];
                    }
                    else
                    {
                        //check if it's possible to resolve as Prefab
                        if (Instance == null)
                        {
                            if ((lifeCycle & LifeCycle.Prefab) == LifeCycle.Prefab && GameObject != null)
                            {
                                instance = GameObject.GetComponent(AbstractType);
                                if (instance)
                                {
                                    assemblyContext.monoScripts[concreteType] = instance as MonoBehaviour;
                                    Debug.Log("Found {0} component on gameObject {1} from Prefab",
                                        TypeName,
                                        instance.gameObject.name);
                                    Instance = instance;
                                    return instance;
                                }
                            }
                        }
                        
                        //try to find component from current scene then
                        if (Instance == null)
                        {
                            instance = Object.FindObjectOfType(concreteType) as MonoBehaviour;
                        }
                        else
                        {
                            instance = Instance as MonoBehaviour;
                        }

                        assemblyContext.monoScripts[concreteType] = instance as MonoBehaviour;
                    }

                    if (instance)
                    {
                        Debug.Log("Found {0} component on gameObject {1} as {2} from current scene",
                            TypeName,
                            instance.gameObject.name,
                            lifeCycle);
                        Instance = instance;
                        return instance;
                    }
                }

                //search for prefabs of this component type from resources path folders
                GameObject prefab = MyResources.Load<GameObject>(TypeName);
                if (!prefab) MyResources.Load<GameObject>(string.Format("bundles/{0}", TypeName));

                if (!prefab) prefab = Resources.Load<GameObject>(string.Format("prefabs/scenes/{0}", TypeName));
                if (!prefab) prefab = Resources.Load<GameObject>(string.Format("scenes/{0}", TypeName));
                if (!prefab) prefab = Resources.Load<GameObject>(string.Format("prefabs/{0}", TypeName));
                if (!prefab) prefab = Resources.Load<GameObject>(TypeName);

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

                assemblyContext.ProcessInjectAttribute(instance.gameObject);

                return instance;
            }

            public void Dispose()
            {
                var disposable = Instance as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
        }
    }
}