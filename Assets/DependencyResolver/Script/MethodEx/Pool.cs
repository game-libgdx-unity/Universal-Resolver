using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityIoC;

/// <summary>
/// Pool only works for IPoolable, Component or GameObject
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
                if (Context.Setting.UseSetForCollection)
                {
                    list = new SortedSet<T>();
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

    public static IEnumerable<T> GetCollection()
    {
        //support for IPoolable
        if (typeof(IPoolable).IsAssignableFrom(typeof(T)))
        {
            return List.Where(item => ((IPoolable)item).Alive);
        }
        else
        {
            return List;
        }
    }


    public static void AddItem(T item)
    {
        List.Add(item);
    }

    public static void RemoveItem(T item)
    {
        //support for IPoolable
        if (typeof(IPoolable).IsAssignableFrom(typeof(T)))
        {
            //don't remove from Pool<T>, just set alive = false
            ((IPoolable) item).Alive = false;
        }
        else
        {
            List.Remove(item);
        }
    }

    public static void Clear()
    {
        //support for IPoolable
        if (typeof(IPoolable).IsAssignableFrom(typeof(T)))
        {
            foreach (IPoolable item in List)
            {
                item.Alive = false;       
            }
        }
        else
        {
            if (list != null)
            {
                list.Clear();
            }
        }
    }
}

public class Pool
{
    public static HashSet<Type> Types = new HashSet<Type>();
    public static Dictionary<Type, object> GetListCache = new Dictionary<Type, object>();

    public static object GetList(Type type)
    {
        if (type == null)
        {
            return null;
        }

        if (GetListCache.ContainsKey(type))
        {
            return GetListCache[type];
        }

        Type generic = typeof(Pool<>);
        Type constructed = generic.MakeGenericType(type);
        var list = constructed.GetProperty("List", BindingFlags.Public | BindingFlags.Static).GetValue(null);
        GetListCache[type] = list;

        return list;
    }

    public static void Add(object item)
    {
        Type generic = typeof(Pool<>);
        Type[] typeArgs = {item.GetType()};
        Type constructed = generic.MakeGenericType(typeArgs);
        var method = constructed.GetMethod("AddItem", BindingFlags.Static | BindingFlags.Public);
        method.Invoke(null, new[] {item});
    }

    public static void Remove(object item)
    {
        Type generic = typeof(Pool<>);
        Type[] typeArgs = {item.GetType()};
        Type constructed = generic.MakeGenericType(typeArgs);
        var method = constructed.GetMethod("RemoveItem", BindingFlags.Static | BindingFlags.Public);
        method.Invoke(null, new[] {item});
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