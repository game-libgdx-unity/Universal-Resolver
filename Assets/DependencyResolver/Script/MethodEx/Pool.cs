using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityIoC;

/// <summary>
/// Pool only works for Component or GameObject
/// C# objects haven't been supported yet.
/// </summary>
/// <typeparam name="T">Unity Component or GameObject</typeparam>
public class Pool<T>
{
    private static ICollection<T> list;

    public static ICollection<T> List
    {
        get
        {
            if (list == null)
            {
                if (Pool.UseSetInsteadOfList)
                {
                    list = new HashSet<T>();
                }
                else
                {
                    list = new List<T>();
                }
            }

            Pool.Types.Add(typeof(T));
            return list;
        }
    }

    public static void AddItem(T item)
    {
        List.Add(item);
    }

    public static void RemoveItem(T item)
    {
        List.Remove(item);
    }

    public static void Clear()
    {
        if (list != null)
        {
            list.Clear();
        }
    }
}

public class Pool
{
    public static bool UseSetInsteadOfList = false;

    public static HashSet<Type> Types = new HashSet<Type>();

    public static void Add(object item)
    {
        Type generic = typeof(Pool<>);
        Type[] typeArgs = {item.GetType()};
        Type constructed = generic.MakeGenericType(typeArgs);
        var addMethod = constructed.GetMethod("AddItem", BindingFlags.Static | BindingFlags.Public);
        addMethod.Invoke(null, new[] {item});
    }

    public static void Clear()
    {
        foreach (var type in Types)
        {
            Type generic = typeof(Pool<>);
            Type[] typeArgs = {type};
            Type constructed = generic.MakeGenericType(typeArgs);
            var clearMethod = constructed.GetMethod("Clear", BindingFlags.Static | BindingFlags.Public);
            clearMethod.Invoke(null, null);
        }

        Types.Clear();
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
            for (int i = 0; i < objects.Count; i++)
            {
                if (!objects[i])
                {
                    objects[i] = CreateObject();
                }

                if (!objects[i].gameObject.activeSelf)
                {
                    var obj = objects[i];
                    obj.gameObject.SetActive(true);
                    obj.transform.SetAsLastSibling();
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