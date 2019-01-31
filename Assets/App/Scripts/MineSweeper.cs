using System.Collections;
using System.Collections.Generic;
using UniRx;
using SimpleIoc;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MineSweeper : MonoBehaviour
{
    public static Context context;
    private void Start()
    {
        Debug.Log("Initializing.........");
        context = new Context(typeof(MineSweeper));
        context.Resolve<MapGenerator>(LifeCycle.Singleton);
    }
}

