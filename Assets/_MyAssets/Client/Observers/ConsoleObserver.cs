using System;
using UniRx;
using UnityEngine;

public class ConsoleObserver<T> : IObserver<T>
{
	private readonly string subject;

	public ConsoleObserver(string subject = "")
	{
		this.subject = subject;
	}
	public void OnNext(T value)
	{
		Debug.LogFormat("{0}: {1}", subject, value);
	}
	public void OnError(Exception error)
	{
		Debug.LogErrorFormat("{0} - OnError: {1}", subject, error.Message);
		Debug.LogErrorFormat("\t {0}", error.StackTrace);
	}
	public void OnCompleted()
	{
		Debug.LogFormat("{0} - OnCompleted()", subject);
	}
}