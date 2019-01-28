using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UserObserver : IObserver<string>
{
	private readonly GameObject prefab;

	public UserObserver(GameObject prefab)
	{
		this.prefab = prefab;
	}
	public void OnNext(string value)
	{
		prefab.GetComponentInChildren<Text>().text = value.ToString();
	}
	public void OnError(Exception error)
	{
		prefab.GetComponentInChildren<Text>().text = "Error: " + error.Message;
	}
	public void OnCompleted()
	{
	}
}
