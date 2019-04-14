/**
 * Author:    Vinh Vu Thanh
 * This class is a part of Universal Resolver project that can be downloaded free at 
 * https://github.com/game-libgdx-unity/UnityEngine.IoC
 * (c) Copyright by MrThanhVinh168@gmail.com
 **/

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        var gameObject = GetGameObject(behaviour, Path);

        if (gameObject != null)
        {
            if (type.IsSubclassOf(typeof(Component)))
            {
                var componentFromGameObject = gameObject.GetComponent(type);

                if (componentFromGameObject != null)
                {
                    return componentFromGameObject;
                }
            }
            //not supported: get non-component from gameObject
//            else
//            {
//                MyDebug.Log("Use IObjectObtainable for non-behaviour type of {0}", type);
//                var values = gameObject.GetComponents<IObjectObtainable>();
//                foreach (var value in values)
//                {
//                    var obj = value.TryObtain(type);
//                    if (obj != null && obj.GetType() == type)
//                    {
//                        return obj;
//                    }
//                }                
//            }

        }

        if (type.IsSubclassOf(typeof(MonoBehaviour)))
        {
            if (gameObject)
            {
                MyDebug.Log("Can't find type of {0}, create a new one on gameObject {1}", type,
                    gameObject.name);

                return gameObject.AddComponent(type);
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

    protected override GameObject GetGameObject(MonoBehaviour behaviour, string path)
    {
        if (string.IsNullOrEmpty(path))
        {
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

        //create game objects by the path in hierarchy 
        if (gameObject == null)
        {
            var nodes = path.Split('/').Where(s => !string.IsNullOrEmpty(s)).ToList();
            var rootObjects = SceneManager.GetActiveScene().GetRootGameObjects().ToList();

            if (nodes.Count > 0)
            {
                //find or create from root level transform
                if (path.StartsWith("/"))
                {
                    GameObject rootObj = rootObjects.FirstOrDefault(o => o.name == nodes[0]);

                    if (!rootObj)
                    {
                        rootObj = new GameObject(nodes[0]);
                    }

                    nodes.RemoveAt(0);
                    gameObject = CreateObjectOnHierarchy(nodes, rootObj.transform);
                }
                //find or create from current transform
                else
                {
                    gameObject = CreateObjectOnHierarchy(nodes, behaviour.transform);
                }
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

    protected GameObject CreateObjectOnHierarchy(IEnumerable<string> nodes, Transform currentT)
    {
        GameObject gameObject;
        foreach (var child in nodes)
        {
            var childT = currentT.Find(child);
            if (childT == null)
            {
                childT = new GameObject(child).transform;
                childT.SetParent(currentT);
            }

            currentT = childT;
        }

        gameObject = currentT.gameObject;
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