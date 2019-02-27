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
            public Type TypeToResolve { get; private set; }

            public Type ConcreteType { get; private set; }

            public object Instance { get; private set; }

            public LifeCycle LifeCycle { get; private set; }

            public Type InjectFromType { get; private set; }
            public BindingAttribute BindingAttribute { get; private set; }

            public RegisteredObject(Type typeToResolve, Type concreteType, AssemblyContext assemblyContext,
                LifeCycle lifeCycle = LifeCycle.Default) :
                this(typeToResolve, concreteType, null, assemblyContext, lifeCycle)
            {
            }

            public RegisteredObject(Type typeToResolve, object instance, AssemblyContext assemblyContext,
                LifeCycle lifeCycle = LifeCycle.Default) :
                this(typeToResolve, null, instance, assemblyContext, lifeCycle)
            {
            }

            private RegisteredObject(Type typeToResolve, Type concreteType, object instance, AssemblyContext assemblyContext,
                LifeCycle lifeCycle = LifeCycle.Default,
                Type injectFromType = null)
            {
                Instance = instance;
                TypeToResolve = typeToResolve;

                if (instance != null)
                {
                    if (typeToResolve == null)
                    {
                        typeToResolve = instance.GetType();
                    }

                    ConcreteType = instance.GetType();
                }
                else if (concreteType != null)
                {
                    ConcreteType = concreteType;
                }
                else
                {
                    throw new InvalidOperationException("either instance or concreteType must not be null");
                }


                LifeCycle = lifeCycle;
                InjectFromType = injectFromType;
                BindingAttribute = GetBinding(assemblyContext, typeToResolve, concreteType, lifeCycle);
            }

            private BindingAttribute GetBinding(AssemblyContext assemblyContext, Type typeToResolve, Type concreteType,
                LifeCycle lifeCycle)
            {
                return assemblyContext.bindingAttributes.FirstOrDefault(b =>
                    b.TypeToResolve == typeToResolve && b.ConcreteType == concreteType &&
                    b.LifeCycle.IsEqual(lifeCycle));
            }

            public object CreateInstance(AssemblyContext assemblyContext, LifeCycle preferredLifeCycle = LifeCycle.Default,
                object resolveFrom = null,
                params object[] args)
            {
                var objectLifeCycle = preferredLifeCycle == LifeCycle.Default ? LifeCycle : preferredLifeCycle;

                if (this.Instance == null || objectLifeCycle == LifeCycle.Transient ||
                    objectLifeCycle == LifeCycle.Default)
                {
                    object instance;
                    if (ConcreteType.IsSubclassOf(typeof(MonoBehaviour)))
                    {
                        GameObject prefab;
                        instance = TryGetPrefab(out prefab, assemblyContext, ConcreteType, ConcreteType.Name, preferredLifeCycle, resolveFrom);
                    }
                    else
                    {
                        instance = Activator.CreateInstance(ConcreteType, args);
                    }

                    assemblyContext.ProcessInjectAttribute(instance);

                    if (Instance != null) return instance;
                    
                    if (objectLifeCycle == LifeCycle.Singleton || (objectLifeCycle & LifeCycle.Singleton) == LifeCycle.Singleton)
                    {
                        Instance = instance;
                    }

                    return instance;
                }

                return Instance;
            }

            private object TryGetPrefab(out GameObject prefab,AssemblyContext assemblyContext, Type concreteType, string TypeName, LifeCycle lifeCycle, object resolveFrom)
            {
                Component instance;
                //search for templates from resources path folders
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
                    if ((lifeCycle & LifeCycle.Component) == LifeCycle.Component && monoBehaviour != null)
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