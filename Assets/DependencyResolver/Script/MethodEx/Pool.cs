using System;
using System.Collections.Generic;
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