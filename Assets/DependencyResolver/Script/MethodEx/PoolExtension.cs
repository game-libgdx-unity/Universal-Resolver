using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityIoC;
using Object = UnityEngine.Object;

public static class PoolExtension
{
    public static T GetInstanceFromPool<T>(
        this ICollection<T> objects,
        Transform parentObject = null,
        object resolveFrom = null,
        params object[] parameters)
        where T : Component
    {
        foreach (var comp in objects)
        {
            if (!comp)
            {
//                objects.Remove(comp);
                continue;
            }

            if (!comp.gameObject.activeSelf)
            {
                comp.gameObject.SetActive(true);
                if (parentObject)
                {
                    comp.transform.SetParent(parentObject, false);
                }

                return comp;
            }
        }


        T g = Context.Resolve<T>(parentObject, LifeCycle.Prefab, resolveFrom, parameters);
        g.gameObject.SetActive(true);

        if (!ReferenceEquals(objects, Pool<T>.List))
        {
            objects.Add(g);
        }

        if (parentObject)
        {
            g.transform.SetParent(parentObject, false);
        }

        return g;
    }

    public static T GetInstanceFromPool<T>(
        this HashSet<T> objects,
        Transform parentObject = null,
        object resolveFrom = null,
        params object[] parameters)
        where T : Component
    {
        if (objects.Count > 0)
        {
            objects.RemoveWhere(o => o == null);

            foreach (var o in objects)
            {
                if (!o)
                {
                    continue;
                }

                if (!o.gameObject.activeSelf)
                {
                    var obj = o;
                    obj.gameObject.SetActive(true);

                    if (obj != null)
                    {
                        if (parentObject)
                        {
                            obj.transform.SetParent(parentObject, false);
                        }

                        //trigger the subject
                        Context.onResolved.Value = obj;
                    }


                    return obj;
                }
            }
        }

        T g = Context.Resolve<T>(parentObject, LifeCycle.Prefab, resolveFrom, parameters);
        g.gameObject.SetActive(true);

        objects.Add(g);

        if (parentObject)
        {
            g.transform.SetParent(parentObject, false);
        }

        return g;
    }

    public static T GetObjectFromPool<T>(
        this List<T> objects,
        Type resolveFrom = null,
        params object[] parameters)
        where T : IPoolable
    {
        if (objects.Count > 0)
        {
            objects.RemoveAll(o => o == null);
            for (int i = 0; i < objects.Count; i++)
            {
                if (objects[i] == null)
                {
                    objects[i] = Context.GetDefaultInstance(typeof(T))
                        .ResolveObject<T>(LifeCycle.Transient, resolveFrom, parameters);
                }

                if (!objects[i].Alive)
                {
                    var obj = objects[i];
                    obj.Alive = true;

                    return obj;
                }
            }
        }

        //not found in pool, create a new one
        T g = Context.GetDefaultInstance(typeof(T)).ResolveObject<T>(LifeCycle.Transient, resolveFrom, parameters);
        g.Alive = true;
        objects.Add(g);

        return g;
    }

    public static GameObject GetInstanceFromPool(this ICollection<GameObject> objects, GameObject prefab,
        Transform parentObject = null)
    {
        foreach (var gameObject in objects)
        {
            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(true);
                if (parentObject)
                {
                    gameObject.transform.SetParent(parentObject, false);
                }

                return gameObject;
            }
        }

        GameObject g = null;
        if (Context.Initialized)
        {
            g = Context.Instantiate(prefab, parentObject);
        }
        else
        {
            g = Object.Instantiate(prefab) as GameObject;
        }

        g.SetActive(true);
        objects.Add(g);
        if (parentObject)
            g.transform.SetParent(parentObject, false);
        return g;
    }

    public static ICollection<GameObject> Preload(this ICollection<GameObject> input, int num, GameObject prefab,
        Transform parentObject = null)
    {
        if (input == null)
        {
            throw new InvalidOperationException("Input list must not be null!");
        }

        var i = 0;
        while (i++ < num)
        {
            GameObject g = null;
            if (Context.Initialized)
            {
                g = Context.Instantiate(prefab, parentObject);
            }
            else
            {
                g = Object.Instantiate(prefab) as GameObject;
            }

            g.SetActive(false);
            if (parentObject)
                g.transform.SetParent(parentObject, false);

            input.Add(g);
        }

        return input;
    }

    public static ICollection<T> Preload<T>(this ICollection<T> input, int num,
        Transform parentObject = null,
        object resolveFrom = null,
        params object[] parameters)
        where T : Component
    {
        if (input == null)
        {
            throw new InvalidOperationException("Input list must not be null!");
        }

        var i = 0;
        while (i++ < num)
        {
            T t = Context.Resolve<T>(parentObject, LifeCycle.Prefab, resolveFrom, parameters);
            t.gameObject.SetActive(false);
            input.Add(t);
        }

        return input;
    }

    public static HashSet<T> Preload<T>(this HashSet<T> input, int num,
        Transform parentObject = null,
        object resolveFrom = null,
        params object[] parameters)
        where T : Component
    {
        if (input == null)
        {
            throw new InvalidOperationException("Input list must not be null!");
        }

        input.RemoveWhere(o => !o);

        var i = 0;
        while (i++ < num)
        {
            T t = Context.Resolve<T>(parentObject, LifeCycle.Prefab, resolveFrom, parameters);
            t.gameObject.SetActive(false);
            input.Add(t);
        }

        return input;
    }

    public static List<T> Preload<T>(this List<T> input, int num, Func<T> CreateObject)
        where T : Component
    {
        if (input == null)
        {
            throw new InvalidOperationException("Input list must not be null!");
        }
        else if (input.Count < num)
        {
            input.Capacity = num;
        }

        input.RemoveAll(o => !o);

        var i = 0;
        while (i++ < num)
        {
            T t = CreateObject();
            t.gameObject.SetActive(false);
            input.Add(t);
        }

        return input;
    }

    public static List<T> Preload<T>(this List<T> input, int num,
        object resolveFrom = null,
        params object[] parameters)
        where T : IPoolable
    {
        if (input == null)
        {
            throw new InvalidOperationException("Input list must not be null!");
        }
        else if (input.Count < num)
        {
            input.Capacity = num;
        }

        input.RemoveAll(o => o == null);

        var i = 0;
        while (i++ < num)
        {
            T t = Context.Resolve<T>(LifeCycle.Transient, resolveFrom, parameters);
            t.Alive = false;
            input.Add(t);
        }

        return input;
    }
}