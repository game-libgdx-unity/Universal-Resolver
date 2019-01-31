using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SimpleIoc
{
    public class RegisteredObject
    {
        public Type TypeToResolve { get; private set; }

        public Type ConcreteType { get; private set; }

        public object Instance { get; private set; }

        public LifeCycle LifeCycle { get; private set; }

        public BindingAttribute BindingAttribute { get; private set; }

        public RegisteredObject(Type typeToResolve, Type concreteType, BindingAttribute bindingAttribute,
            LifeCycle lifeCycle = LifeCycle.Default)
        {
            TypeToResolve = typeToResolve;
            ConcreteType = concreteType;
            LifeCycle = lifeCycle;
            BindingAttribute = bindingAttribute;
        }

        public RegisteredObject(Type typeToResolve, object instance, BindingAttribute bindingAttribute,
            LifeCycle lifeCycle = LifeCycle.Default)
        {
            if (instance == null)
            {
                Debug.LogError("Instance must not be null");
                return;
            }

            TypeToResolve = typeToResolve;
            ConcreteType = instance.GetType();
            Instance = instance;
            LifeCycle = lifeCycle;
            BindingAttribute = bindingAttribute;
        }


        public object CreateInstance(Context context, LifeCycle preferredLifeCycle = LifeCycle.Default,
            params object[] args)
        {
            var objectLifeCycle = preferredLifeCycle == LifeCycle.Default ? LifeCycle : preferredLifeCycle;

            if (this.Instance == null || objectLifeCycle == LifeCycle.Transient ||
                objectLifeCycle == LifeCycle.Default)
            {
                object instance;
                if (TypeToResolve.IsSubclassOf(typeof(MonoBehaviour)))
                {
                    //search for templates from resources path folders
                    var prefab = MyResources.Load<GameObject>(string.Format("prefabs/scenes/{0}", ConcreteType));
                    if (!prefab) prefab = MyResources.Load<GameObject>(string.Format("scenes/{0}", ConcreteType));
                    if (!prefab) prefab = MyResources.Load<GameObject>(string.Format("prefabs/{0}", ConcreteType));
                    if (!prefab) prefab = MyResources.Load<GameObject>(ConcreteType.ToString());

                    if (prefab)
                    {
                        Debug.LogFormat("Found prefab for {0} .......", ConcreteType);
                        var prefabInstance = Object.Instantiate(prefab);

                        if (prefabInstance.GetComponent(ConcreteType) == null)
                        {
                            Debug.LogFormat("Found component {0} in prefab children", ConcreteType);
                            instance = prefabInstance.GetComponentInChildren(ConcreteType);
                        }
                        else
                        {
                            Debug.LogFormat("Found component {0} in the prefab", ConcreteType);
                            instance = prefabInstance.GetComponent(ConcreteType);
                        }
                    }
                    else
                    {
                        Debug.LogFormat("FNot found prefab for {0} in the prefab, created a new game object",
                            ConcreteType);
                        instance = new GameObject().AddComponent(ConcreteType);
                    }
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
    }
}