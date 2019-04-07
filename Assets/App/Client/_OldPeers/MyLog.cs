using System;
using UnityEngine;
using System.Collections;
using Firebase.Database;

public class MyLog : SingletonBehaviour<MyLog>
{
    string myLog;
    Queue myLogQueue = new Queue();

    [SerializeField] public int maxLine = 100;
    [SerializeField] public bool logSystem = true;
    [SerializeField] public LogType logType = LogType.Error;

    void OnEnable()
    {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
        if (logSystem) Application.logMessageReceived += HandleLog;
#endif
    }

    void OnDisable()
    {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
        if (logSystem) Application.logMessageReceived -= HandleLog;
#endif
    }

    public void HandleLog(string logString, string stackTrace = null, LogType type = LogType.Log)
    {
        //filter logs
        if (type != LogType.Exception && type > logType)
        {
            return;
        }

        //print logs to screen
        myLog = logString;
        string newString = "\n [" + type + "] : " + myLog;
        myLogQueue.Enqueue(newString);
        if (type == LogType.Exception)
        {
            newString = "\n" + stackTrace;
            myLogQueue.Enqueue(newString);
            if (myLogQueue.Count > maxLine)
            {
                Clear();
            }
        }

        myLog = string.Empty;
        foreach (string mylog in myLogQueue)
        {
            myLog += mylog;
        }
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

        GUILayout.Label(myLog);
    }

    private void Clear()
    {
        myLog = "";
        myLogQueue.Clear();
    }
}