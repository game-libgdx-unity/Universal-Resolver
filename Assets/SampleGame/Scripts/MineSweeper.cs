using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using UniRx;
using UnityIoC;
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

public class MineSweeper : MonoBehaviour
{
    private void Awake()
    {
        
        
        Debug.LogFormat("Time loading scene: {0}", Time.realtimeSinceStartup);
        MyDebug.EnableLogging = false;
        
        Stopwatch stopWatch = new Stopwatch();
        stopWatch.Reset();
        stopWatch.Start();
        
        var context = Context.GetDefaultInstance(typeof(MineSweeper));
        context.Resolve<MapGenerator>(LifeCycle.Singleton);
        
        
        stopWatch.Stop();
        TimeSpan ts = stopWatch.Elapsed;
        string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);
        Debug.Log("RunTime " + elapsedTime);
    }
}

public class Benmark
{
    
}