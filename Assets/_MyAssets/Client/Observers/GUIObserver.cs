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