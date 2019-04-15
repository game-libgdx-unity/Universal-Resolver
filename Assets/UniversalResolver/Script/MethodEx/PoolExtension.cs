using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityIoC;

public static class PoolExtension
{
    public static T GetFromPool<T>(
        this List<T> objects,
        T prefab,
        Transform parentObject = null)
        where T : Component
    {
        if (objects.Count > 0)
        {
            for (int i = 0; i < objects.Count; i++)
            {
                if (!objects[i].gameObject.activeSelf)
                {
                    var obj = objects[i];
                    obj.gameObject.SetActive(true);
                    return obj;
                }
            }
        }

        T g = null;
        if (Context.Initialized)
        {
            g = Context.ResolveObject<T>(LifeCycle.Prefab);
        }
        else
        {
            g = Object.Instantiate(prefab) as T;
        }

        g.gameObject.SetActive(true);
        objects.Add(g);
        if (parentObject)
            g.transform.SetParent(parentObject, false);
        return g;
    }

    public static GameObject GetFromPool(this List<GameObject> objects, GameObject prefab,
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

    public static List<GameObject> Preload(this List<GameObject> output, int num, GameObject prefab,
        Transform parentObject = null)
    {
        if (output == null)
            output = new List<GameObject>(num);
        else
            output.Capacity = num;

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

            output.Add(g);
        }

        return output;
    }

    public static List<GameObject> Preload(int num, GameObject prefab, Transform parentObject = null)
    {
        var output = new List<GameObject>(num);
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

            output.Add(g);
        }

        return output;
    }
}