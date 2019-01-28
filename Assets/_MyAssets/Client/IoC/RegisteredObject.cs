using System;
using UnityEngine;

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
                var instance = TypeToResolve.IsSubclassOf(typeof(MonoBehaviour))
                    ? new GameObject().AddComponent(ConcreteType)
                    : Activator.CreateInstance(ConcreteType, args);

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