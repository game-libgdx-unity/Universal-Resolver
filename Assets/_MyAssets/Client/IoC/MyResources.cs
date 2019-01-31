using UnityEngine;

namespace UnityIoC
{
    public class MyResources
    {
        public static T Load<T>(string path) where T : Object
        {
            return Resources.Load<T>(path);
        }
    }

    public class MyDebug
    {
        
    }
}