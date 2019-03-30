/**
 * Author:    Vinh Vu Thanh
 * This class is a part of Unity IoC project that can be downloaded free at 
 * https://github.com/game-libgdx-unity/UnityEngine.IoC or http://u3d.as/1rQJ
 * (c) Copyright by MrThanhVinh168@gmail.com
 **/
using System;
using UniRx;
using UnityEngine;

public class GUIObserver<T> : IObserver<T>
{
    private readonly string subject;

    public GUIObserver(string subject = "")
    {
        this.subject = subject;
    }

    public void OnNext(T value)
    {
        MyLog.Instance.HandleLog(string.Format("{0}: {1}", subject, value));
    }

    public void OnError(Exception error)
    {
        MyLog.Instance.HandleLog(string.Format("{0} - OnError: {1}", subject, error.Message),
            string.Format("\t {0}", error.StackTrace),
            LogType.Exception);
    }

    public void OnCompleted()
    {
        MyLog.Instance.HandleLog(string.Format("{0} - OnCompleted()", subject));
    }
}