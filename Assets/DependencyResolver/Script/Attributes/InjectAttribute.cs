using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Linq;
using UnityEngine;
using UnityIoC;

/// <summary>
/// General solutions for injecting most of types, also support getting from context caches
/// </summary>
public class InjectAttribute : ComponentAttribute
{
    public InjectAttribute() : base(LifeCycle.Inject, null)
    {
    }

    public InjectAttribute(string path = null) : base(LifeCycle.Inject, path)
    {
    }


    public override Component GetComponent(MonoBehaviour behaviour, Type type)
    {
        if (type.IsInterface || type.IsSubclassOf(typeof(Component)))
        {
            //try to get the component from self or descendants
            var go = GetGameObject(behaviour, Path);

            var componentFromGameObject = go.GetComponent(type);

            if (componentFromGameObject != null)
            {
                return componentFromGameObject;
            }

            var descendants = go.Descendants();
            foreach (var gameObject in descendants)
            {
                var component = gameObject.GetComponent(type);
                if (component != null)
                {
                    return component;
                }
            }
            
            var ancestors = go.Ancestors();
            foreach (var gameObject in ancestors)
            {
                var arrayComponents = gameObject.GetComponents(type);
                if (arrayComponents.Length > 0)
                {
                    foreach (var component in arrayComponents)
                    {
                        return component;
                    }
                }
            }
            
            if (!string.IsNullOrEmpty(Path) && type.IsSubclassOf(typeof(MonoBehaviour)))
            {
                return go.AddComponent(type);
            }
        }

        return null;
    }

    public override Component[] GetComponents(MonoBehaviour behaviour, Type type)
    {
        if (type.IsInterface || type.IsSubclassOf(typeof(Component)))
        {
            var go = GetGameObject(behaviour, Path);

            var components = go.GetComponents(type);

            if (components != null)
            {
                return components;
            }

            var listComponents = components.ToList();

            var descendants = go.Descendants();
            foreach (var gameObject in descendants)
            {
                var arrayComponents = gameObject.GetComponents(type);
                if (arrayComponents.Length > 0)
                {
                    foreach (var component in arrayComponents)
                    {
                        listComponents.Add(component);
                    }
                }
            }

            if (listComponents.Count > 0)
            {
                return listComponents.ToArray();
            }

            var ancestors = go.Ancestors();
            foreach (var gameObject in ancestors)
            {
                var arrayComponents = gameObject.GetComponents(type);
                if (arrayComponents.Length > 0)
                {
                    foreach (var component in arrayComponents)
                    {
                        listComponents.Add(component);
                    }
                }
            }

            if (listComponents.Count > 0)
            {
                return listComponents.ToArray();
            }

            return components;
        }

        return null;
    }
}