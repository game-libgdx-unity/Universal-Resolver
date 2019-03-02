/**
 * Author:    Vinh Vu Thanh
 * This class is a part of Unity IoC project that can be downloaded free at 
 * https://github.com/game-libgdx-unity/UnityEngine.IoC
 * (c) Copyright by MrThanhVinh168@gmail.com
 **/

using System;
using UnityEngine;
using System.Collections;
using System.Diagnostics;
using System.Text;
using Firebase.Database;
using UniRx;
using Debug = UnityEngine.Debug;

public class MyLog : SingletonBehaviour<MyLog>
{
    StringBuilder myLog = new StringBuilder();
    Queue myLogQueue = new Queue();

    [SerializeField] public Color screenColor = Color.black;
    [SerializeField] public int maxLength = 3000;
    [SerializeField] public int fontSize = 40;
    [SerializeField] public bool autoScroll = true;
    [SerializeField] public bool logSystem = true;
    [SerializeField] public LogType logType = LogType.Error;

    private GUIStyle labelSkin;
    private Vector2 scrollPosition;

    void OnEnable()
    {
        if (logSystem) Application.logMessageReceived += HandleLog;

        Camera.main.backgroundColor = screenColor;
    }

    void OnDisable()
    {
        if (logSystem) Application.logMessageReceived -= HandleLog;
    }

    public void HandleLog(string logString, string stackTrace = null, LogType type = LogType.Log)
    {
        var stackTrace2 = new StackTrace(true);
        var frame = stackTrace2.FrameCount;
        
        //filter logs
        if (type != LogType.Exception && (int)type > (int)logType)
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

        if (autoScroll)
            scrollPosition.y = Mathf.Infinity;

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