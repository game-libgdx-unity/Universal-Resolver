using System.Collections.Generic;

/// <summary>
/// Pool only works for Component & GameObject
/// C# objects aren't supported yet.
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
                list = new List<T>();
            }
            return list;
        }
    }
}