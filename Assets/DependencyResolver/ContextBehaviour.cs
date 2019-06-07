using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityIoC;
using Object = UnityEngine.Object;

public class ContextBehaviour : MonoBehaviour
{
    /// <summary>
    /// Context will be created and initilized with this setting
    /// </summary>
    public BaseBindingSetting customSetting;

    /// <summary>
    /// Allow Context to log its actions when it registers or resolves objects.
    /// </summary>
    public bool enableLogging = false;

    /// <summary>
    /// This is the default name of the default assembly that unity generated to compile your code
    /// </summary>
    public string DefaultBundleName = "resources";

    /// <summary>
    /// While running in editor, always load from resources before searching in asset bundles
    /// </summary>
    public bool EditorLoadFromResource = true;

    /// <summary>
    /// Get views from pools rather than creating a brand new one. Default is true.
    /// </summary>
    public bool CreateViewsFromPools = true;

    /// <summary>
    /// if true, when a new scene is unloaded, call the Dispose method. Default is false.
    /// </summary>
    public bool AutoDisposeOnUnload;

    /// <summary>
    /// if true, when the default instance get initialized, it will process all mono-behaviours in current active scenes. Default is true.
    /// </summary>
    public bool AutoLoadSetting = true;

    /// <summary>
    /// If true, Pool will use HashSet as the collection instead of using List
    /// </summary>
    public bool UseSetForPoolCollection = false;

    /// <summary>
    /// If true, Context will be created automatically by default assembly
    /// </summary>
    public bool AutoCreateContext = true;

    /// <summary>
    /// Path to load a resource locally. You can use {type}, {scene}, {id} to modify the path 
    /// </summary>
    public string[] assetPaths =
    {
        "{type}",
        "{scene}/{type}",
        "Prefabs/{type}",
        "Prefabs/{scene}/{type}",
    };

    public BindingInScene[] bindings;

    void Awake()
    {
        UniversalResolverDebug.EnableLogging = enableLogging;

        if (!string.IsNullOrEmpty(DefaultBundleName))
        {
            Context.Setting.DefaultBundleName = DefaultBundleName;
        }

        Context.Setting.CreateViewFromPool = CreateViewsFromPools;
        Context.Setting.AutoBindDefaultSetting = AutoLoadSetting;
        Context.Setting.AutoDisposeWhenSceneChanged = AutoDisposeOnUnload;
        Context.Setting.UseSetForCollection = UseSetForPoolCollection;
        Context.Setting.EditorLoadFromResource = EditorLoadFromResource;


        if (customSetting != null || bindings.Length > 0 ||
            assetPaths.Length > 0 || AutoCreateContext)
        {
            Debug.Log("Context is created automatically!");

            Context context = null;

            if (customSetting)
            {
                context = new Context(customSetting)
                {
                    assetPaths = assetPaths
                };
            }
            else
            {
                context = new Context(this, AutoLoadSetting)
                {
                    assetPaths = assetPaths
                };
            }

            Context.DefaultInstance = context;
            
            if (bindings.Length > 0)
            {
                context.DefaultContainer.registeredTypes.Add(typeof(GameObject));

                foreach (var binding in bindings)
                {
                    Context.RegisteredObject registeredObject = new Context.RegisteredObject(typeof(GameObject),
                        typeof(GameObject), binding.GameObject, LifeCycle.Prefab,
                        context.GetTypeFromCurrentAssembly(binding.TypeObjHolder.name));

                    context.DefaultContainer.registeredObjects.Add(registeredObject);
                }
            }

            //force auto process if settings require it to run.
            if (customSetting && !customSetting.autoProcessSceneObjects && AutoLoadSetting)
            {
                context.ProcessInjectAttributeForMonoBehaviours();
            }

            return;
        }

        Debug.Log("Context isn't created automatically due to no usages found");
    }

    private void OnDestroy()
    {
        Context.Reset();
    }
}

[Serializable]
public struct BindingInScene
{
    public Object TypeObjHolder;
    public GameObject GameObject;
}

//        static object lockObj = new object();
//        Test multithread code in unity
//        Debug.Log("Starting....");
//
//        Task.Run(()=>
//        {
//            Monitor.Enter(lockObj);
//
//            Thread.Sleep(1000);
//            
//            Monitor.Exit(lockObj);
//        });
//
//        
//        Thread.Sleep(20);
//
//        int timeout = 500;
//
//        if (Monitor.TryEnter(lockObj, timeout))
//        {
//            try
//            {
//                // The critical section.
//                Debug.Log("doing..");
//            }
//            finally
//            {
//                // Ensure that the lock is released.
//                Monitor.Exit(lockObj);
//                Debug.Log("done..");
//            }
//        }
//        else
//        {
//            Debug.Log("failed to acquire lock ");
//        }  
//        
//        if (Monitor.TryEnter(lockObj, timeout))
//        {
//            try
//            {
//                // The critical section.
//                Debug.Log("doing..");
//            }
//            finally
//            {
//                // Ensure that the lock is released.
//                Monitor.Exit(lockObj);
//                Debug.Log("done..");
//            }
//        }
//        else
//        {
//            Debug.Log("failed to acquire lock ");
//        }