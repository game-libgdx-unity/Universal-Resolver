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

public class ComponentAttribute : InjectAttribute, IInjectComponent
{
    public Component GetComponent(MonoBehaviour behaviour, Type type)
    {
        var componentFromGameObject = behaviour.GetComponent(type);
        if (componentFromGameObject != null)
        {
            return componentFromGameObject;
        }

        return behaviour.gameObject.AddComponent(type);
    }
}


public class ChildrenAttribute : InjectAttribute, IInjectComponent
{
    public Component GetComponent(MonoBehaviour behaviour, Type type)
    {
        throw new NotImplementedException();
    }
}

public class ParentsAttribute : InjectAttribute, IInjectComponent
{
    public Component GetComponent(MonoBehaviour behaviour, Type type)
    {
        throw new NotImplementedException();
    }
}

public class AncestorsAttribute : InjectAttribute, IInjectComponent
{
    public Component GetComponent(MonoBehaviour behaviour, Type type)
    {
        throw new NotImplementedException();
    }
}

public class DescendantsAttribute : InjectAttribute, IInjectComponent
{
    public Component GetComponent(MonoBehaviour behaviour, Type type)
    {
        throw new NotImplementedException();
    }
}


public class FindObjectOfTypeAttribute : InjectAttribute, IInjectComponent
{
    public Component GetComponent(MonoBehaviour behaviour, Type type)
    {
        return Object.FindObjectOfType(type) as MonoBehaviour;
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

        return GameObject.Find(Name).GetComponent(type);
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
            return findingObj.GetComponent(type);
        }

        //unable to find GameObjects By Tag
        return null;
    }
}