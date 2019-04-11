using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityIoC;

public class LoadBundleScene : MonoBehaviour
{
    public string SceneToLoad = "title";
    // Start is called before the first frame update
    void Start()
    {
        AssetBundleDownloader.Instance.EnableCacheMode = true;
        AssetBundleDownloader.Instance.Initialize(Application.streamingAssetsPath, Application.persistentDataPath);
        AssetBundleDownloader.Instance.StartDownloadRequiredBundles();
        AssetBundleDownloader.Instance.OnAssetBundleDownloader_LoadRequiredComplete += () =>
        {
            print("Done bundles");
            SceneManager.LoadScene(SceneToLoad);
        };
    }
}
