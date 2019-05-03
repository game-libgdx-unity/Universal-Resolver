using System;
using Object = UnityEngine.Object;

namespace UnityIoC
{
    /// <summary>
    /// Overloaded version of Resources, support loading from asset bundles or resources folder.
    /// </summary>
    public class MyResources
    {
        /// <summary>
        /// Load a general resource
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Object Load(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return null;
            }
            
            //load from Resources folder if running in Editor
#if !UNITY_EDITOR
            return UnityEngine.Resources.Load(path);
#endif

            //check if resources bundle is loaded, then load from the default bundle named "resources"
            var asset = AssetBundleDownloader.Instance.Get(path);
            if (asset != null)
            {
                return asset;
            }

            //if the bundle not found, go back to load from Resources folder
            return UnityEngine.Resources.Load(path);
        }

        /// <summary>
        /// /// Load a generic resource
        /// </summary>
        /// <param name="path"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Load<T>(string path) where T : Object
        {
            //load from Resources folder if running in Editor
#if UNITY_EDITOR
            return UnityEngine.Resources.Load<T>(path);
#endif

            //check if resources bundle is loaded, then load from the default bundle named "resources"
            var asset = AssetBundleDownloader.Instance.Get<T>(path);
            if (asset != null)
            {
                return asset;
            }

            //if the bundle not found, go back to load from Resources folder
            return UnityEngine.Resources.Load<T>(path);
        }

        /// <summary>
        /// /// Load a generic resource with parameterized string
        /// </summary>
        /// <param name="path"></param>
        /// <param name="string_params"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Load<T>(string path, params object[] string_params) where T : Object
        {
            return Load<T>(string.Format(path, string_params));
        }
    }
}