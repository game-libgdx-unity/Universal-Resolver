/**
 * Author:    Vinh Vu Thanh
 * This class is a part of Unity IoC project that can be downloaded free at 
 * https://github.com/game-libgdx-unity/UnityEngine.IoC or http://u3d.as/1rQJ
 * (c) Copyright by MrThanhVinh168@gmail.com
 **/
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