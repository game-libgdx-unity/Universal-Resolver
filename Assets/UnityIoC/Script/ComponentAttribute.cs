/**
 * Author:    Vinh Vu Thanh
 * This class is a part of Unity IoC project that can be downloaded free at 
 * https://github.com/game-libgdx-unity/UnityEngine.IoC
 * (c) Copyright by MrThanhVinh168@gmail.com
 **/

using System;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;
public partial class ComponentAttribute : InjectAttribute, IInjectComponent
{
    public Component GetComponent(MonoBehaviour behaviour, Type type)
    {
        var componentFromGameObject = behaviour.GetComponent(type);
        if (componentFromGameObject != null)
        {
            return componentFromGameObject;
        }

        Debug.LogFormat("Can't find type of {0}, create a new one on gameObject {1}", type, behaviour.gameObject.name);
        return behaviour.gameObject.AddComponent(type);
    }
}


public class ChildrenAttribute : InjectAttribute, IInjectComponent
{
    public Component GetComponent(MonoBehaviour behaviour, Type type)
    {
        var componentInChildren = behaviour.GetComponentInChildren(type);
        if (componentInChildren != null)
        {
            return componentInChildren;
        }

        Debug.LogFormat("Can't find type of {0}, create a new one on gameObject {1}", type, behaviour.gameObject.name);
        return behaviour.gameObject.AddComponent(type);
    }
}

public class ParentsAttribute : InjectAttribute, IInjectComponent
{
    public Component GetComponent(MonoBehaviour behaviour, Type type)
    {
        var componentInParent = behaviour.GetComponentInParent(type);
        if (componentInParent != null)
        {
            return componentInParent;
        }

        Debug.LogFormat("Can't find type of {0}, create a new one on gameObject {1}", type, behaviour.gameObject.name);
        return behaviour.gameObject.AddComponent(type);
    }
}

public class FindObjectOfTypeAttribute : InjectAttribute, IInjectComponent
{
    public Component GetComponent(MonoBehaviour behaviour, Type type)
    {
        var component = Object.FindObjectOfType(type) as MonoBehaviour;
        if (!ReferenceEquals(component, null))
        {
            return component;
        }

        Debug.LogFormat("Can't find type of {0}, create a new one on gameObject {1}", type, behaviour.gameObject.name);
        return behaviour.gameObject.AddComponent(type);
    }
}

public class FindGameObjectByNameAttribute : InjectAttribute, IInjectComponent
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

        Debug.LogFormat("Can't find type of {0}, create a new one on gameObject {1}", type, behaviour.gameObject.name);
        return behaviour.gameObject.AddComponent(type);
    }
}

public class FindGameObjectsByTagAttribute : InjectAttribute, IInjectComponent
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

        Debug.LogFormat("Can't find type of {0}, create a new one on gameObject {1}", type, behaviour.gameObject.name);
        return behaviour.gameObject.AddComponent(type);
    }
}