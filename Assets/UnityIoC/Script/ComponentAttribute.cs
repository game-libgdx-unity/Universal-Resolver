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


public class ComponentAttribute : InjectAttribute, IComponentResolvable, IComponentArrayResolvable
{
    public ComponentAttribute()
        : base(LifeCycle.Component | LifeCycle.Default, null)
    {
    }

    public ComponentAttribute(string path = null)
        : base(LifeCycle.Component | LifeCycle.Default, path)
    {
    }

    public Component GetComponent(MonoBehaviour behaviour, Type type)
    {
        var gameObject = GetGameObject(behaviour);

        //can't process if gameObject not found
        if (gameObject != null)
        {
            var componentFromGameObject = gameObject.GetComponent(type);

            if (componentFromGameObject != null)
            {
                return componentFromGameObject;
            }
        }

        if (type.IsSubclassOf(typeof(MonoBehaviour)))
        {
            MyDebug.Log("Can't find type of {0}, create a new one on gameObject {1}", type,
                behaviour.gameObject.name);

            return behaviour.gameObject.AddComponent(type);
        }
        else
        {
            MyDebug.Log("Can't use [Component] for non-behaviour type of {0}", type);
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
        
        //Find the game object by searching in resources or asset bundles.
        if (gameObject == null)
        {
            gameObject = MyResources.Load<GameObject>(Path);
            if (gameObject)
            {
                MyDebug.Log("Found the {0} by searching in resources or asset bundles.", Path);
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

public class ChildrenAttribute : InjectAttribute, IComponentResolvable, IComponentArrayResolvable
{
    public Component GetComponent(MonoBehaviour behaviour, Type type)
    {
        var componentInChildren = behaviour.GetComponentInChildren(type);
        if (componentInChildren != null)
        {
            return componentInChildren;
        }

        if (type.IsSubclassOf(typeof(MonoBehaviour)))
        {
            MyDebug.Log("Can't find type of {0}, create a new one on gameObject {1}", type,
                behaviour.gameObject.name);
            return behaviour.gameObject.AddComponent(type);
        }

        MyDebug.Log("Unable to Get/Add non-monobehaviour {0} using on object {1}", type,
            behaviour.gameObject.name);
        return null;
    }

    public Component[] GetComponents(MonoBehaviour behaviour, Type type)
    {
        return behaviour.GetComponentsInChildren(type);
    }
}

public class ParentsAttribute : InjectAttribute, IComponentResolvable
{
    public Component GetComponent(MonoBehaviour behaviour, Type type)
    {
        var componentInParent = behaviour.GetComponentInParent(type);
        if (componentInParent != null)
        {
            return componentInParent;
        }

        if (type.IsSubclassOf(typeof(MonoBehaviour)))
        {
            MyDebug.Log("Can't find type of {0}, create a new one on gameObject {1}", type,
                behaviour.gameObject.name);
            return behaviour.gameObject.AddComponent(type);
        }

        MyDebug.Log("Unable to Get/Add non-monobehaviour {0} using on object {1}", type,
            behaviour.gameObject.name);
        return null;
    }
}

public class FindObjectOfTypeAttribute : InjectAttribute, IComponentResolvable
{
    public Component GetComponent(MonoBehaviour behaviour, Type type)
    {
        var component = Object.FindObjectOfType(type) as MonoBehaviour;
        if (!ReferenceEquals(component, null))
        {
            return component;
        }

        if (type.IsSubclassOf(typeof(MonoBehaviour)))
        {
            MyDebug.Log("Can't find type of {0}, create a new one on gameObject {1}", type,
                behaviour.gameObject.name);
            return behaviour.gameObject.AddComponent(type);
        }

        MyDebug.Log("Unable to Get/Add non-monobehaviour {0} using on object {1}", type,
            behaviour.gameObject.name);
        return null;
    }
}

public class FindGameObjectByNameAttribute : InjectAttribute, IComponentResolvable
{
    public FindGameObjectByNameAttribute(string name)
    {
        Name = name;
    }

    public string Name { get; set; }

    public Component GetComponent(MonoBehaviour behaviour, Type type)
    {
        if (string.IsNullOrEmpty(Name))
        {
            throw new MissingMemberException("You need to set value for Name property!");
        }

        var component = GameObject.Find(Name).GetComponent(type);
        if (!ReferenceEquals(component, null))
        {
            return component;
        }

        if (type.IsSubclassOf(typeof(MonoBehaviour)))
        {
            MyDebug.Log("Can't find type of {0}, create a new one on gameObject {1}", type,
                behaviour.gameObject.name);
            return behaviour.gameObject.AddComponent(type);
        }

        MyDebug.Log("Unable to Get/Add non-monobehaviour {0} using on object {1}", type,
            behaviour.gameObject.name);
        return null;
    }
}

public class FindGameObjectsByTagAttribute : InjectAttribute, IComponentResolvable
{
    public FindGameObjectsByTagAttribute(string tag)
    {
        Tag = tag;
    }

    public string Tag { get; set; }

    public Component GetComponent(MonoBehaviour behaviour, Type type)
    {
        var findingObj = GameObject.FindGameObjectsWithTag(Tag)
            .FirstOrDefault(go => go.GetComponent(type) != null);

        if (findingObj != null)
        {
            return findingObj.GetComponent(type) ?? behaviour.gameObject.AddComponent(type);
        }

        if (type.IsSubclassOf(typeof(MonoBehaviour)))
        {
            MyDebug.Log("Can't find type of {0}, create a new one on gameObject {1}", type,
                behaviour.gameObject.name);
            return behaviour.gameObject.AddComponent(type);
        }

        MyDebug.Log("Unable to Get/Add non-monobehaviour {0} using on object {1}", type,
            behaviour.gameObject.name);
        return null;
    }
}