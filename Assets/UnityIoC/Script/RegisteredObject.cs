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
    public partial class Context
    {
        public class RegisteredObject : IDisposable
        {
            public Type TypeToResolve { get; private set; }

            public Type ConcreteType { get; private set; }

            public object Instance { get; private set; }

            public LifeCycle LifeCycle { get; private set; }

            public BindingAttribute BindingAttribute { get; private set; }

            public RegisteredObject(Type typeToResolve, Type concreteType, Context context,
                LifeCycle lifeCycle = LifeCycle.Default) :
                this(typeToResolve, concreteType, null, context, lifeCycle)
            {
            }

            public RegisteredObject(Type typeToResolve, object instance, Context context,
                LifeCycle lifeCycle = LifeCycle.Default) :
                this(typeToResolve, null, instance, context, lifeCycle)
            {
            }

            private RegisteredObject(Type typeToResolve, Type concreteType, object instance, Context context,
                LifeCycle lifeCycle = LifeCycle.Default)
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
                BindingAttribute = GetBinding(context, typeToResolve, concreteType, lifeCycle);
            }

            private BindingAttribute GetBinding(Context context, Type typeToResolve, Type concreteType,
                LifeCycle lifeCycle)
            {
                return context.bindingAttributes.FirstOrDefault(b =>
                    b.TypeToResolve == typeToResolve && b.ConcreteType == concreteType &&
                    b.LifeCycle.IsEqual(lifeCycle));
            }

            public object CreateInstance(Context context, LifeCycle preferredLifeCycle = LifeCycle.Default,
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
                        instance = TryFetPrefab(out prefab, ConcreteType, ConcreteType.Name);
                    }
                    else
                    {
                        instance = Activator.CreateInstance(ConcreteType, args);
                    }

                    context.ProcessInjectAttribute(instance);

                    if (this.Instance == null && objectLifeCycle == LifeCycle.Singleton)
                    {
                        this.Instance = instance;
                    }

                    return instance;
                }

                return Instance;
            }

            private object TryFetPrefab(out GameObject prefab, Type concreteType, string TypeName)
            {
                object instance;
//search for templates from resources path folders
                prefab = MyResources.Load<GameObject>(string.Format("prefabs/scenes/{0}", TypeName));
                if (!prefab) prefab = MyResources.Load<GameObject>(string.Format("scenes/{0}", TypeName));
                if (!prefab) prefab = MyResources.Load<GameObject>(string.Format("prefabs/{0}", TypeName));
                if (!prefab) prefab = MyResources.Load<GameObject>(TypeName.ToString());

                if (prefab)
                {
                    Debug.LogFormat("Found prefab for {0} .......", TypeName);
                    var prefabInstance = Object.Instantiate(prefab);

                    if (prefabInstance.GetComponent(TypeName) == null)
                    {
                        Debug.LogFormat("Found component {0} in prefab children", TypeName);
                        instance = prefabInstance.GetComponentInChildren(concreteType);
                    }
                    else
                    {
                        Debug.LogFormat("Found component {0} in the prefab", TypeName);
                        instance = prefabInstance.GetComponent(TypeName);
                    }
                }
                else
                {
                    Debug.LogFormat("FNot found prefab for {0} in the prefab, created a new game object",
                        TypeName);
                    instance = new GameObject().AddComponent(concreteType);
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