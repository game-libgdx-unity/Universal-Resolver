using System.Collections.Generic;

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