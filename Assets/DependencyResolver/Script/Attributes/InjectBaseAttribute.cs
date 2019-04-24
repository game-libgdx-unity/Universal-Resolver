/**
 * Author:    Vinh Vu Thanh
 * This class is a part of Universal Resolver project that can be downloaded free at 
 * https://github.com/game-libgdx-unity/UnityEngine.IoC
 * (c) Copyright by MrThanhVinh168@gmail.com
 **/

using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityIoC;
using Object = UnityEngine.Object;


//public class 

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Constructor |
                AttributeTargets.Field)]
public class InjectBaseAttribute : Attribute, IComponentResolvable, IComponentArrayResolvable
{
    public IContainer container { get; set; }
    public string Path { get; protected set; }
    public LifeCycle LifeCycle { get; private set; }

    public InjectBaseAttribute(LifeCycle lifeCycle = LifeCycle.Default, string path = null)
    {
        Path = path;
        LifeCycle = lifeCycle;
    }

    public InjectBaseAttribute() : this(LifeCycle.Default)
    {
    }


//    public object GetObject(MonoBehaviour behaviour, Type type)
//    {
//        var gameObject = GetGameObject(behaviour, Path);
//
//        //can't process if gameObject not found
//        if (gameObject == null)
//        {
//            return null;
//        }
//        
//        var componentFromGameObject = gameObject.GetComponent(type);
//
//        if (componentFromGameObject != null)
//        {
//            //as default or transition
//            if (LifeCycle == LifeCycle.Transient || (LifeCycle & LifeCycle.Transient) == LifeCycle.Transient ||
//                LifeCycle == LifeCycle.Default || (LifeCycle & LifeCycle.Default) == LifeCycle.Default)
//            {
//                var clone = Object.Instantiate(componentFromGameObject);
//                return clone;
//            }
//
//            //as singleton or component
//            return componentFromGameObject;
//        }
//
//        return null;
//    }

    public Component GetComponent(MonoBehaviour behaviour, Type type)
    {
        var gameObject = GetGameObject(behaviour, Path);

        //can't process if gameObject not found
        if (gameObject == null)
        {
            return null;
        }

        if (type.IsSubclassOf(typeof(Component))
            || type.IsInterface)
        {
            var componentFromGameObject = gameObject.GetComponent(type);

            if (componentFromGameObject != null)
            {
                //as default or transition
                if (LifeCycle == LifeCycle.Transient || (LifeCycle & LifeCycle.Transient) == LifeCycle.Transient ||
                    LifeCycle == LifeCycle.Default || (LifeCycle & LifeCycle.Default) == LifeCycle.Default)
                {
                    var clone = Object.Instantiate(componentFromGameObject);
                    return clone;
                }

                //as singleton or component
                return componentFromGameObject;
            }
        }

        return null;
    }

    public Component[] GetComponents(MonoBehaviour behaviour, Type type)
    {
        var gameObject = GetGameObject(behaviour, Path);

        //can't process if gameObject not found
        if (gameObject == null)
        {
            return null;
        }

        return gameObject.GetComponents(type);
    }

    protected virtual GameObject GetGameObject(MonoBehaviour behaviour, string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            //unsupported
            return behaviour.gameObject;
        }

        GameObject gameObject = null;
        Component componentFromGameObject = null;

        //seeking the component from root
        if (path.StartsWith("/"))
        {
            gameObject = GameObject.Find(path);
        }
        //searching from current gameObject
        else
        {
            var transform = behaviour.transform.Find(path);
            if (transform)
            {
                gameObject = transform.gameObject;
            }
        }

        //set the gameObject if the search failed
        if (gameObject == null)
        {
            MyDebug.Log("Can't find gameObject by path {0}, will use current gameObject", path);

            gameObject = behaviour.gameObject;
        }

        return gameObject;
    }
}

public class PrefabAttribute : InjectBaseAttribute
{
    public PrefabAttribute()
        : base(LifeCycle.Prefab, null)
    {
    }

    public PrefabAttribute(string path = null)
        : base(LifeCycle.Prefab, path)
    {
    }
}

public class TransientAttribute : InjectBaseAttribute
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

public class SingletonComponentAttribute : InjectBaseAttribute
{
    public SingletonComponentAttribute(string path = null)
        : base(LifeCycle.SingletonComponent, path)
    {
    }

    public SingletonComponentAttribute()
        : base(LifeCycle.SingletonComponent)
    {
    }
}