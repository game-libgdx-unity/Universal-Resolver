using System.Collections;
using System.Collections.Generic;

/* code template only */

public enum MultitonType
{
    ZERO,
    ONE,
    TWO
};

public class Multiton<K, T> : Singleton<Multiton<K, T>>, IEnumerable<T>
{
    private readonly Dictionary<K, T> Instances =
        new Dictionary<K, T>();

    public T this[K type]
    {
        get
        {
            if (!Instances.ContainsKey(type))
            {
                Instances.Add(type, default(T));
            }

            return Instances[type];
        }
        set
        {
            Instances[type] = value;
        }
    }

    public IEnumerator<T> GetEnumerator()
    {
        return Instances.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return Instances.Values.GetEnumerator();
    }
}

public class Singleton<T> where T : new()
{
    private static T instance = default(T);
    private static readonly object padlock = new object();

    public static T Instance
    {
        get
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new T();
                }
                return instance;
            }
        }
    }
}