using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityIoC;
using Object = UnityEngine.Object;

public class ContextBehaviour : SingletonBehaviour<ContextBehaviour>
{
    public bool enableLogging = false;
    public InjectIntoBindingSetting customSetting;

    /// <summary>
    /// This is the default name of the default assembly that unity generated to compile your code
    /// </summary>
    public string AssemblyName = Context.Setting.DefaultAssemblyName;

    /// <summary>
    /// Get views from pools rather than a new object. Default is false.
    /// </summary>
    public bool CreateViewsFromPools;

    /// <summary>
    /// if true, when a new scene is unloaded, call the Dispose method. Default is false.
    /// </summary>
    public bool AutoDisposeOnUnload;

    /// <summary>
    /// if true, when the default instance get initialized, it will process all mono-behaviours in current active scenes. Default is true.
    /// </summary>
    public bool AutoProcessBehaviours = true;

    public BindingInScene[] bindings;

    void Awake()
    {
        UniversalResolverDebug.EnableLogging = enableLogging;

        if (!string.IsNullOrEmpty(AssemblyName)) Context.Setting.AssemblyName = AssemblyName;
        Context.Setting.CreateViewFromPool = CreateViewsFromPools;
        Context.Setting.AutoProcessBehavioursInScene = AutoProcessBehaviours;
        Context.Setting.AutoDisposeWhenSceneChanged = AutoDisposeOnUnload;


        var context = Context.GetDefaultInstance();

        if (customSetting != null)
        {
            context.LoadBindingSetting(customSetting);
        }

        if (bindings.Length > 0)
        {
            foreach (var binding in bindings)
            {
                Context.RegisteredObject registeredObject = new Context.RegisteredObject(typeof(GameObject),
                    typeof(GameObject), binding.GameObject, LifeCycle.Prefab,
                    context.GetTypeFromCurrentAssembly(binding.TypeObjHolder.name));

                context.DefaultContainer.registeredTypes.Add(typeof(GameObject));
                context.DefaultContainer.registeredObjects.Add(registeredObject);
            }
        }
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