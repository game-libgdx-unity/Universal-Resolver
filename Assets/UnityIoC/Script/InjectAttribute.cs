/**
 * Author:    Vinh Vu Thanh
 * This class is a part of Unity IoC project that can be downloaded free at 
 * https://github.com/game-libgdx-unity/UnityEngine.IoC
 * (c) Copyright by MrThanhVinh168@gmail.com
 **/

using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityIoC;
using Object = UnityEngine.Object;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Constructor |
                AttributeTargets.Field)]
public class InjectAttribute : Attribute, IComponentResolvable, IComponentArrayResolvable
{
    public IContainer container { get; set; }
    public string Path { get; protected set; }
    public LifeCycle LifeCycle { get; private set; }

    public InjectAttribute(LifeCycle lifeCycle = LifeCycle.Default, string path = null)
    {
        Path = path;
        LifeCycle = lifeCycle;
    }

    public InjectAttribute() : this(LifeCycle.Default)
    {
    }

    public Component GetComponent(MonoBehaviour behaviour, Type type)
    {
        var gameObject = GetGameObject(behaviour);

        //can't process if gameObject not found
        if (gameObject == null)
        {
            return null;
        }

        var componentFromGameObject = gameObject.GetComponent(type);

        if (componentFromGameObject != null)
        {
            //as default or transition
            if (LifeCycle == LifeCycle.Transient || (LifeCycle & LifeCycle.Transient) == LifeCycle.Transient ||
                LifeCycle == LifeCycle.Default || (LifeCycle & LifeCycle.Default) == LifeCycle.Default)
            {
//                var clone = componentFromGameObject.CopyComponent(new GameObject());
                var clone = Object.Instantiate(componentFromGameObject);
                return clone;
            }

            //as singleton or component
            return componentFromGameObject;
        }

        return null;
    }

    public Component[] GetComponents(MonoBehaviour behaviour, Type type)
    {
        var gameObject = GetGameObject(behaviour);

        //can't process if gameObject not found
        if (gameObject == null)
        {
            return null;
        }

        return gameObject.GetComponents(type);
    }

    private GameObject GetGameObject(MonoBehaviour behaviour)
    {
        if (string.IsNullOrEmpty(Path))
        {
            //unsupported
            return null;
        }

        GameObject gameObject = null;
        Component componentFromGameObject = null;

        //seeking the component from root
        if (Path.StartsWith("/"))
        {
            gameObject = GameObject.Find(Path);
        }
        //searching from current gameObject
        else
        {
            var transform = behaviour.transform.Find(Path);
            if (transform)
            {
                gameObject = transform.gameObject;
            }
        }

        //set the gameObject if the search failed
        if (gameObject == null)
        {
            MyDebug.Log("Can't find gameObject by path {0}, will use current gameObject", Path);

            gameObject = behaviour.gameObject;
        }

        return gameObject;
    }
}

public class SingletonAttribute : InjectAttribute
{
    public SingletonAttribute(string path = null)
        : base(LifeCycle.Singleton, path)
    {
    }

    public SingletonAttribute()
        : base(LifeCycle.Singleton)
    {
    }
}

public class TransientAttribute : InjectAttribute
{
    public TransientAttribute(string path = null)
        : base(LifeCycle.Transient, path)
    {
    }

    public TransientAttribute()
        : base(LifeCycle.Transient)
    {
    }
}

public class ComponentSingletonAttribute : InjectAttribute
{
    public ComponentSingletonAttribute(string path = null)
        : base(LifeCycle.SingletonComponent, path)
    {
    }
    public ComponentSingletonAttribute()
        : base(LifeCycle.SingletonComponent)
    {
    }
}