using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;
using UnityIoC;
using Object = UnityEngine.Object;
#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(ContextBehaviour))]
public class ContextBehaviourInspector : Editor
{
    public override void OnInspectorGUI()
    {
        //hide the AutoLoadSetting if customBindingSetting is null
        ContextBehaviour cb = (ContextBehaviour) serializedObject.targetObject;

        EditorGUI.BeginChangeCheck();

        if (cb.customSetting != null)
        {
            string[] propertyToExclude = {nameof(cb.AutoFindBindingSetting)};
            
            if (cb.customSetting.autoProcessSceneObjects)
            {
                propertyToExclude = new[] { nameof(cb.bindingsInScene), nameof(cb.AutoFindBindingSetting)};
               
            }
            
            DrawPropertiesExcluding(serializedObject, propertyToExclude);
        }

        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();
        }
    }
}

#endif


public class ContextBehaviour : MonoBehaviour
{
    /// <summary>
    /// Context will be created and initilized with this setting
    /// </summary>
    public BaseBindingSetting customSetting;


    /// <summary>
    /// This is the default name of the default assembly that unity generated to compile your code
    /// </summary>
    public string DefaultBundleName = "resources";

    /// <summary>
    /// Allow Context to log its actions when it registers or resolves objects.
    /// </summary>
    public bool enableLogging = false;

    /// <summary>
    /// While running in editor, always load from resources before searching in asset bundles
    /// </summary>
    public bool EditorLoadFromResource = true;

    /// <summary>
    /// Get views from pools rather than creating a brand new one. Default is true.
    /// </summary>
    public bool CreateViewsFromPools = true;

    /// <summary>
    /// If true, Context will be never process on all Behaviours, Default is false
    /// </summary>
    public bool DisableProcessAllBehaviour = true;

    /// <summary>
    /// if true, when a new scene is unloaded, call the Dispose method. Default is false.
    /// </summary>
    public bool AutoDisposeOnDestroy = false;

    /// <summary>
    /// If true, Pool will use HashSet as the collection instead of using List
    /// </summary>
    public bool UseSetForPoolCollection = false;

    /// <summary>
    /// Allow to search on objects in the scene for the needed prefabs, Default is false
    /// </summary>
    public bool SearchPrefabFromScene = false;

    /// <summary>
    /// Allow to search for default binding setting files
    /// </summary>
    public bool AutoFindBindingSetting = true;

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

    public BindingInScene[] bindingsInScene;

    void Awake()
    {
        Context.Setting.EnableLogging = enableLogging;

        if (!string.IsNullOrEmpty(DefaultBundleName))
        {
            Context.Setting.DefaultBundleName = DefaultBundleName;
        }

        Context.Setting.CreateViewFromPool = CreateViewsFromPools;
        Context.Setting.AutoFindBindingSetting = AutoFindBindingSetting;
        Context.Setting.AutoDisposeWhenSceneChanged = AutoDisposeOnDestroy;
        Context.Setting.UseSetForCollection = UseSetForPoolCollection;
        Context.Setting.EditorLoadFromResource = EditorLoadFromResource;

        if (customSetting != null || bindingsInScene.Length > 0 || assetPaths.Length > 0)
        {
            Debug.Log("Context is created automatically!");

            Context context = null;

            if (!customSetting)
            {
                context = new Context(this, AutoFindBindingSetting, SearchPrefabFromScene, DisableProcessAllBehaviour,
                    assetPaths);
            }
            else
            {
                context = new Context(customSetting, SearchPrefabFromScene, DisableProcessAllBehaviour,
                    assetPaths);
            }

            Context.DefaultInstance = context;

            if (bindingsInScene.Length > 0)
            {
                context.DefaultContainer.registeredTypes.Add(typeof(GameObject));

                foreach (var binding in bindingsInScene)
                {
                    Context.RegisteredObject registeredObject =
                        new Context.RegisteredObject(
                            typeof(GameObject),
                            typeof(GameObject),
                            binding.GameObject,
                            LifeCycle.Prefab,
                            context.GetTypeFromCurrentAssembly(binding.TypeObjHolder.name));

                    context.DefaultContainer.registeredObjects.Add(registeredObject);
                }
            }

            //force auto process if settings require it to run.
            if (customSetting && !customSetting.autoProcessSceneObjects || !customSetting)
            {
                if (!DisableProcessAllBehaviour)
                {
                    context.ProcessInjectAttributeForMonoBehaviours();
                }
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