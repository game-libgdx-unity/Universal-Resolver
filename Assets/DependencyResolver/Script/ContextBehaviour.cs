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
    public BindingInScene[] bindings;

    static object lockObj = new object();

    void Awake()
    {
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

        MyDebug.EnableLogging = enableLogging;

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
        Context.DisposeDefaultInstance();
    }
}

[Serializable]
public struct BindingInScene
{
    public Object TypeObjHolder;
    public GameObject GameObject;
}