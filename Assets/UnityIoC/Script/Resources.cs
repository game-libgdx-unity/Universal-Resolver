using UnityEngine;

namespace UnityIoC
{
    /// <summary>
    /// Overload version of Resources, planned to support for load from asset bundles or resources folder.
    /// </summary>
    public class Resources
    {
        public static T Load<T>(string path) where T : Object
        {
            return UnityEngine.Resources.Load<T>(path);
        }
        public static T Load<T>(string path, params object[] string_params) where T : Object
        {
            return UnityEngine.Resources.Load<T>(string.Format(path, string_params));
        }
    }
}