using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
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

        var b = Benchmark.Start();

        var context = Context.GetDefaultInstance(typeof(MineSweeper));
        context.ResolveObject<MapGenerator>(LifeCycle.Singleton);

        Benchmark.Stop();
    }
}

public class Benchmark
{
    static Stopwatch StopWatch = new Stopwatch(); 
    public static void Stop(Stopwatch stopWatch = null)
    {
        if (stopWatch == null)
        {
            stopWatch = Benchmark.StopWatch;
        }
        
        stopWatch.Stop();
        TimeSpan ts = stopWatch.Elapsed;
        string elapsedTime = string.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);
        Debug.Log("RunTime " + elapsedTime);
    }

    public static Stopwatch Start(Stopwatch stopWatch = null)
    {
        if (stopWatch == null)
        {
            stopWatch = Benchmark.StopWatch;
        }

        stopWatch.Reset();
        stopWatch.Start();
        return stopWatch;
    }
}