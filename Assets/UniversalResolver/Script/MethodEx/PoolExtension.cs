using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityIoC;
using Object = UnityEngine.Object;

public static class PoolExtension
{
    public static T GetInstanceFromPool<T>(
        this List<T> objects,
        Transform parentObject = null)
        where T : Component
    {
        if (objects.Count > 0)
        {
//            objects.RemoveAll(o => o == null);
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
                    return obj;
                }
            }
        }

        T g = Context.Resolve<T>(LifeCycle.Transient);
        g.gameObject.SetActive(true);

        objects.Add(g);

        if (parentObject)
        {
            g.transform.SetParent(parentObject, false);
        }

        return g;
    }

    public static GameObject GetInstanceFromPool(this List<GameObject> objects, GameObject prefab,
        Transform parentObject = null)
    {
        if (objects.Count > 0)
        {
            for (int i = 0; i < objects.Count; i++)
            {
                if (!objects[i].activeSelf)
                {
                    GameObject obj = objects[i];
                    obj.SetActive(true);
                    return obj;
                }
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

    public static List<GameObject> Preload(this List<GameObject> input, int num, GameObject prefab,
        Transform parentObject = null)
    {
        if (input == null)
        {
            throw new InvalidOperationException("Input list must not be null!");
        }
        else
        {
            input.Capacity = num;
        }

        input.RemoveAll(o => !o);

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

    public static List<T> Preload<T>(this List<T> input, int num,
        Transform parentObject = null,
        object resolveFrom = null,
        params object[] parameters)
        where T : Component
    {
        if (input == null)
        {
            throw new InvalidOperationException("Input list must not be null!");
        }
        else
            input.Capacity = num;

        input.RemoveAll(o => !o);

        var i = 0;
        while (i++ < num)
        {
            T t = Context.Resolve<T>(parentObject, LifeCycle.Transient, resolveFrom, parameters);
            t.gameObject.SetActive(false);
            input.Add(t);
        }

        return input;
    }
}