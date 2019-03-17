using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityIoC;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MineSweeper : MonoBehaviour
{
    private void Awake()
    {
        Debug.Log("Initializing.........");
        MyDebug.EnableLogging = true;
        var context = AssemblyContext.GetDefaultInstance(typeof(MineSweeper));
        context.Resolve<MapGenerator>(LifeCycle.Singleton);
    }
}