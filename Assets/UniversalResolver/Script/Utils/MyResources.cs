using UnityEngine;

namespace UnityIoC
{
    /// <summary>
    /// Overload version of Resources, planned to support for load from asset bundles or resources folder.
    /// </summary>
    public class MyResources
    {
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

        public static T Load<T>(string path, params object[] string_params) where T : Object
        {
            return Load<T>(string.Format(path, string_params));
        }
    }
}