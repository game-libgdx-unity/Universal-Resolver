using System;
using System.Collections.Generic;
using UnityEngine;
using UnityIoC;

/// <summary>
/// Pool only works for Component or GameObject
/// C# objects haven't been supported yet.
/// </summary>
/// <typeparam name="T">Unity Component or GameObject</typeparam>
public class Pool<T>
{
    private static List<T> list;

    public static List<T> List
    {
        get
        {
            if (list == null)
            {
                poolTypes.Add(typeof(T));
                list = new List<T>();
            }

            return list;
        }
    }

    static HashSet<Type> poolTypes = new HashSet<Type>();

    public static void Clear()
    {
        foreach (var type in poolTypes)
        {
            //todo: try to get Pool<type>.List then clear it.
        }

        poolTypes.Clear();
    }
}

public class ViewPool
{
    private Dictionary<Type, List<Component>> caches = new Dictionary<Type, List<Component>>();

    public List<Component> GetPools(Type type)
    {
        if (!caches.ContainsKey(type))
        {
            caches[type] = new List<Component>();
        }

        return caches[type];
    }

    public void Clear()
    {
        caches.Clear();
    }

    public Component GetObject(
        Type type,
        Func<Component> CreateObject)
    {
        var objects = GetPools(type);

        if (objects.Count > 0)
        {
            objects.RemoveAll(o => o == null);
            for (int i = 0; i < objects.Count; i++)
            {
                if (!objects[i])
                {
                    continue;
                }

                if (!objects[i].gameObject.activeSelf)
                {
                    var obj = objects[i];
                    obj.gameObject.SetActive(true);
                    obj.transform.SetAsLastSibling();
                    if (obj != null)
                    {
                        //trigger the subject
                        Context.OnResolved.Value = obj;
                    }


                    return obj;
                }
            }
        }

        var g = CreateObject();
        g.gameObject.SetActive(true);
        objects.Add(g);
        return g;
    }

    public void Preload(
        int preload,
        Type type,
        Func<Component> CreateObject)
    {
        var pool = GetPools(type);
        pool.Preload(preload, CreateObject);
    }
}

// obsoleted code 
///// <summary>
///// Pool only works for Component or GameObject
///// C# objects haven't been supported yet.
///// </summary>
///// <typeparam name="T">Unity Component or GameObject</typeparam>
//public class DataBinding<T>
//{
//    private static Dictionary<IDataBinding<T>, IObserver<T>> dictionary;
//
//    public static Dictionary<IDataBinding<T>, IObserver<T>> Dictionary
//    {
//        get
//        {
//            if (dictionary == null)
//            {
//                keyTypes.Add(typeof(T));
//                dictionary = new Dictionary<IDataBinding<T>, IObserver<T>>();
//            }
//            return dictionary;
//        }
//    }
//
//    static HashSet<Type> keyTypes = new HashSet<Type>();
//    public static void Clear()
//    {
//        foreach (var type in keyTypes)
//        {
//            //try to get DataBinding<type>.List then clear it.
//        }
//        
//        keyTypes.Clear();
//    }
//}