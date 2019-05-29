using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace UnityIoC
{
    public class AssetBundleDownloader : SingletonBehaviour<AssetBundleDownloader>
    {
        [Serializable]
        public class BundleInfo
        {
            public string Name;
            public bool IsRequired;
        }

        public event Action<string> OnAssetBundleDownloader_DecryptedCompleted;
        public event Action<Exception> OnAssetBundleDownloader_LoadFail;
        public event Action OnAssetBundleDownloader_LoadRequiredComplete;

        [SerializeField] public string HostUrl;
        [SerializeField] public string FilePath;
        [SerializeField] protected bool enableCacheMode = true;

        public BundleInfo[] BundleInfos;

        public AssetBundle this[string bundleName]
        {
            get
            {
                if (bundles.ContainsKey(bundleName))
                {
                    return bundles[bundleName];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (bundles.ContainsKey(bundleName))
                {
                    bundles[bundleName] = value;
                }
                else
                {
                    bundles.Add(bundleName, value);
                }
            }
        }


        public bool EnableCacheMode
        {
            get { return enableCacheMode; }
            set { enableCacheMode = value; }
        }

        protected Dictionary<string, AssetBundle> bundles = new Dictionary<string, AssetBundle>();

        protected int requiredBundleLoadCount;

        void Start()
        {
            Initialize(HostUrl, FilePath);
            OnAssetBundleDownloader_DecryptedCompleted += HandleAssetBundleDownloaderDecryptedCompleted;
        }

        private void OnDestroy()
        {
            UnLoadAllBundles(false);
        }


        public void Initialize(string downloadUrl, string filePath)
        {
            HostUrl = String.IsNullOrEmpty(HostUrl) ? Application.streamingAssetsPath : downloadUrl;
            FilePath = String.IsNullOrEmpty(filePath) ? Application.persistentDataPath : filePath;
        }

        private void HandleAssetBundleDownloaderDecryptedCompleted(string assetBundle)
        {
            var requiredBundleCount = BundleInfos.Count(bi => bi.IsRequired);
            if (requiredBundleLoadCount < requiredBundleCount)
            {
                requiredBundleLoadCount++;
                if (requiredBundleLoadCount == requiredBundleCount)
                {
                    if (OnAssetBundleDownloader_LoadRequiredComplete != null)
                    {
                        OnAssetBundleDownloader_LoadRequiredComplete();
                    }
                }
            }
        }

        /// <summary>
        /// Start download all asset bundles
        /// Only call after register success
        /// </summary>
        public void StartDownloadRequiredBundles()
        {
            StartCoroutine(DownloadRoutine(BundleInfos
                .Where(bi => bi.IsRequired)
                .Select(bi => bi.Name)
                .ToArray()));
        }

        public void StartDownloadOptionalBundles()
        {
            StartCoroutine(DownloadRoutine(BundleInfos
                .Where(bi => !bi.IsRequired)
                .Select(bi => bi.Name)
                .ToArray()));
        }

        protected IEnumerator DownloadRoutine(params string[] bundleNames)
        {
            yield return null;

            string filePath;
            byte[] data;
            for (int index = 0; index < bundleNames.Length; index++)
            {
                var bundleName = bundleNames[index];
                filePath = FilePath + "/" + bundleName;
                Debug.Log("File Path: " + filePath);
                // Load from cache
                if (enableCacheMode && File.Exists(filePath))
                {
                    Debug.Log("File Path Exist!!!!!");
                    using (var reader = new BinaryReader(File.Open(filePath, FileMode.Open)))
                    {
                        data = reader.ReadBytes((int) reader.BaseStream.Length);
                        reader.Close();
                    }
                }
                // Download and save to cache
                else
                {
                    string fullPath = "";
                    if (Application.isEditor || String.IsNullOrEmpty(HostUrl))
                    {
                        fullPath = Application.streamingAssetsPath + "/" + bundleName;
                    }
                    else
                    {
                        fullPath = HostUrl + "/" + bundleName;
                    }

#if UNITY_EDITOR

#if !UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN

                    //if the file url is from local streamingAssets path:
                    if (HostUrl == Application.streamingAssetsPath)
                    {
                        fullPath = "file://" + fullPath;
                    }
#endif

                    if (HostUrl == Application.streamingAssetsPath && !File.Exists(fullPath))
                    {
                        string msg = string.Format("Can't find the asset bundle {0}", bundleName);
                        Debug.LogFormat(msg);
                        if (OnAssetBundleDownloader_LoadFail != null)
                        {
                            OnAssetBundleDownloader_LoadFail(new FileNotFoundException(msg));
                        }

                        HandleAssetBundleDownloaderDecryptedCompleted(bundleName);
                        continue;
                    }
#endif

                    print("Downloading assets from " + fullPath);

                    var wwwEncryptedBundle = new WWW(fullPath);

                    yield return wwwEncryptedBundle;
                    if (wwwEncryptedBundle.error != null)
                    {
                        if (OnAssetBundleDownloader_LoadFail != null)
                        {
                            Debug.Log("Download error: " + wwwEncryptedBundle.error);
                            OnAssetBundleDownloader_LoadFail(new Exception(wwwEncryptedBundle.error));
                        }

                        continue;
                    }

                    data = wwwEncryptedBundle.bytes;

                    print("Download is complete");

                    if (enableCacheMode)
                    {
                        using (var stream = File.Open(filePath, FileMode.OpenOrCreate))
                        {
                            stream.Write(data, 0, data.Length);
                            stream.Close();
                        }
                    }

                    //dispose unused stream
                    wwwEncryptedBundle.Dispose();
                }
#if !UNITY_WEBGL || UNITY_EDITOR
                //decrypt asset bundles in a new thread
                new Thread(new ParameterizedThreadStart(DecryptBundle))
                    .Start(new KeyValuePair<string, byte[]>(bundleName, data));
#else
			DecryptBundle(new KeyValuePair<string, byte[]>(bundleNames[index], data));
#endif
            }
        }

        /// <summary>
        /// Decrypt asset bundle
        /// </summary>
        /// <param name="bundlePairObject"></param>
        protected void DecryptBundle(object bundlePairObject)
        {
            var bundlePair = (KeyValuePair<string, byte[]>) bundlePairObject;
            var bundleName = bundlePair.Key;
            var byteData = bundlePair.Value;
            try
            {
                UIThreadInvoker.Instance.Invoke(() => StartCoroutine(CreateBundleInMemory(bundleName, byteData)));
            }
            catch (Exception ex)
            {
                if (OnAssetBundleDownloader_LoadFail != null)
                {
                    OnAssetBundleDownloader_LoadFail(ex);
                }
            }
        }

        /// <summary>
        /// Create bundles from Memory
        /// </summary>
        /// <param name="bundleName"></param>
        /// <param name="rawData"></param>
        /// <returns></returns>
        protected IEnumerator CreateBundleInMemory(string bundleName, byte[] rawData)
        {
            AssetBundleCreateRequest assetBundleCreateRequest = AssetBundle.LoadFromMemoryAsync(rawData);

            yield return assetBundleCreateRequest;

            this[bundleName] = assetBundleCreateRequest.assetBundle;

            yield return null;

            if (OnAssetBundleDownloader_DecryptedCompleted != null)
            {
                OnAssetBundleDownloader_DecryptedCompleted(bundleName);
            }
        }

        /// <summary>
        /// Unload bundles
        /// </summary>
        /// <param name="unloadAll"></param>
        public void UnLoadAllBundles(bool unloadAll)
        {
            foreach (var pair in bundles)
            {
                if (pair.Value != null)
                {
                    pair.Value.Unload(unloadAll);
                }
            }
        }

        /// <summary>
        /// get an asset by a path
        /// Load from default resource bundle or by template: "bundle name/asset_key"
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public Object Get(string path)
        {
            return Get<Object>(path);
        }

        /// <summary>
        /// Get bundle by a path
        /// Load from default resource bundle or by template: "bundle name/asset_key"
        /// </summary>
        /// <param name="path"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Get<T>(string path) where T : Object
        {
            //try load by template: "bundle name/asset_key"
            if (path.Contains("/"))
            {
                var bundleName = path.Substring(0, path.IndexOf('/'));
                var assetPath = path.Substring(path.IndexOf('/') + 1); //+1 for ignoring the '/'
                var asset = Get<T>(bundleName, assetPath);
                if (asset && asset.GetType().IsInstanceOfType(typeof(T)))
                {
                    return asset as T;
                }
            }
            
            //load from default bundle
            if (bundles.ContainsKey(Context.Setting.DefaultBundleName))
            {
                var defaultBundle = bundles[Context.Setting.DefaultBundleName];
                if (defaultBundle.Contains(path))
                {
                    var asset = defaultBundle.LoadAsset(path);
                    if (asset && asset.GetType().IsInstanceOfType(typeof(T)))
                    {
                        return asset as T;
                    }
                }
            }

            return default(T);
        }

        public Object Get<T>(string bundleName, string path) where T : Object
        {
            if (bundles.ContainsKey(bundleName))
            {
                var bundle = bundles[bundleName];
                if (bundle.Contains(path))
                {
                    return bundle.LoadAsset(path);
                }
            }

            return default(T);
        }

        public bool GetFromSceneBundle<T>(string path) where T : Object
        {
            return Get<T>(SceneManager.GetActiveScene().name, path);
        }
    }
}