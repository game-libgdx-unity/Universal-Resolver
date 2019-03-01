/**
 * Author:    Vinh Vu Thanh
 * This class is a part of Unity IoC project that can be downloaded free at 
 * https://github.com/game-libgdx-unity/UnityEngine.IoC
 * (c) Copyright by MrThanhVinh168@gmail.com
 **/

using System;
using UnityEngine;
using System.Collections;
using System.Text;
using Firebase.Database;
using UniRx;

public class MyLog : SingletonBehaviour<MyLog>
{
    StringBuilder myLog = new StringBuilder();
    Queue myLogQueue = new Queue();

    [SerializeField] public Color ScreenColor = Color.black;
    [SerializeField] public int maxLength = 3000;
    [SerializeField] public int fontSize = 40;
    [SerializeField] public bool logSystem = true;
    [SerializeField] public LogType logType = LogType.Error;

    private GUIStyle labelSkin;
    private Vector2 scrollPosition;

    void OnEnable()
    {
        if (logSystem) Application.logMessageReceived += HandleLog;

        Camera.main.backgroundColor = ScreenColor;
    }

    void OnDisable()
    {
        if (logSystem) Application.logMessageReceived -= HandleLog;
    }

    public void HandleLog(string logString, string stackTrace = null, LogType type = LogType.Log)
    {
        //filter logs
        if (type != LogType.Exception && type > logType)
        {
            return;
        }

        //print logs to screen
        string newString = "\n [" + type + "] : " + logString;
        myLogQueue.Enqueue(newString);
        if (type == LogType.Exception)
        {
            newString = "\n" + stackTrace;
            myLogQueue.Enqueue(newString);
        }

        myLog.Length = 0;
        bool overloaded = false;

        foreach (string mylog in myLogQueue)
        {
            myLog.Append(mylog);
            if (myLog.Length > maxLength)
            {
                overloaded = true;
                break;
            }
        }

        if (overloaded)
            Clear();
    }

    void OnGUI()
    {
        if (GUILayout.Button("Clear", GUILayout.Width(300)))
        {
            Clear();

#if UNITY_EDITOR
            Debug.ClearDeveloperConsole();
#endif
        }

        scrollPosition = GUILayout.BeginScrollView(
            scrollPosition, GUILayout.Width(Screen.width), GUILayout.Height(Screen.height - 20));

        labelSkin = GUI.skin.label;
        labelSkin.fontSize = fontSize;
        GUILayout.Label(myLog.ToString(), labelSkin);

        GUILayout.EndScrollView();
    }

    private void Clear()
    {
        myLog.Length = 0;
        myLogQueue.Clear();
    }
}