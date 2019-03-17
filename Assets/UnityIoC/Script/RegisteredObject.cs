/**
 * Author:    Vinh Vu Thanh
 * This class is a part of Unity IoC project that can be downloaded free at 
 * https://github.com/game-libgdx-unity/UnityEngine.IoC
 * (c) Copyright by MrThanhVinh168@gmail.com    
 **/

using System;
using System.Linq;
using UnityEngine;
using UnityIoC;
using Object = UnityEngine.Object;

namespace UnityIoC
{
    public partial class AssemblyContext
    {
        public class RegisteredObject : IDisposable
        {
            private readonly Logger Debug = new Logger(typeof(RegisteredObject));
            public Type AbstractType { get; private set; }

            public Type ImplementedType { get; private set; }

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

                if (this.Instance == null || objectLifeCycle == LifeCycle.Transient ||
                    objectLifeCycle == LifeCycle.Default)
                {
                    object instance;
                    if (ImplementedType.IsSubclassOf(typeof(MonoBehaviour)))
                    {
                        instance = TryGetGameObject(assemblyContext, ImplementedType, ImplementedType.Name,
                            preferredLifeCycle, resolveFrom);
                    }
                    else
                    {
                        instance = Activator.CreateInstance(ImplementedType, args);
                    }

                    assemblyContext.ProcessInjectAttribute(instance);

                    if (Instance != null) return instance;

                    if (objectLifeCycle == LifeCycle.Singleton ||
                        (objectLifeCycle & LifeCycle.Singleton) == LifeCycle.Singleton)
                    {
                        Instance = instance;
                    }

                    return instance;
                }

                return Instance;
            }

            private object TryGetGameObject(AssemblyContext assemblyContext, Type concreteType,
                string TypeName, LifeCycle lifeCycle, object resolveFrom)
            {
                GameObject prefab = null;
                Component instance;

//                //try to get component from an existing one in current scene
//                if (Instance == null)
//                {
//                    instance = Object.FindObjectOfType(concreteType) as Component;
//                    Instance = instance;
//                }
//                else
//                {
//                    instance = Instance as Component;
//                }
//
//                if (instance)
//                {
//                    if (lifeCycle == LifeCycle.Singleton || (lifeCycle & LifeCycle.Singleton) == LifeCycle.Singleton)
//                    {
//                        Debug.Log("Found {0} component on gameObject {1} as {2} from current scene",
//                            TypeName,
//                            instance.gameObject.name,
//                            lifeCycle);
//
//                        return instance;
//                    }
//
//                    if (lifeCycle == LifeCycle.Transient || (lifeCycle & LifeCycle.Transient) == LifeCycle.Transient || 
//                        lifeCycle == LifeCycle.Default || (lifeCycle & LifeCycle.Default) == LifeCycle.Default)
//                    {
//                        Debug.Log("Found {0} component on gameObject {1} as {2} from current scene",
//                            TypeName,
//                            instance.gameObject.name,
//                            lifeCycle);
//                        
//                        var cloneObj = Object.Instantiate(instance);
//                        return cloneObj;
//                    }
//                }

                //search for prefabs of this component type from resources path folders
                prefab = Resources.Load<GameObject>(string.Format("prefabs/scenes/{0}", TypeName));
                if (!prefab) prefab = Resources.Load<GameObject>(string.Format("scenes/{0}", TypeName));
                if (!prefab) prefab = Resources.Load<GameObject>(string.Format("prefabs/{0}", TypeName));
                if (!prefab) prefab = Resources.Load<GameObject>(TypeName.ToString());

                if (prefab)
                {
                    Debug.Log("Found prefab for {0} .......", TypeName);
                    GameObject prefabInstance = null;
                    var monoBehaviour = resolveFrom as Component;

                    if ((lifeCycle & LifeCycle.Component) == LifeCycle.Component && monoBehaviour != null)
                    {
                        prefabInstance = Object.Instantiate(prefab, monoBehaviour.transform);
                    }
                    else
                    {
                        prefabInstance = Object.Instantiate(prefab);
                    }

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
            }
        }
    }
}