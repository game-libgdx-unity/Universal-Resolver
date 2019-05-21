using System.Collections;
using System.Collections.Generic;
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
        UniversalResolverDebug.EnableLogging = false;

        var b = Benchmark.Start();

        var context = Context.GetDefaultInstance(typeof(MineSweeper));
        context.ResolveObject<MapGenerator>(LifeCycle.Singleton);

        Benchmark.Stop();
    }
}