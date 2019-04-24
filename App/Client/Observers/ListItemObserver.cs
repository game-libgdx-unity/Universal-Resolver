/**
 * Author:    Vinh Vu Thanh
 * This class is a part of Unity IoC project that can be downloaded free at 
 * https://github.com/game-libgdx-unity/UnityEngine.IoC or http://u3d.as/1rQJ
 * (c) Copyright by MrThanhVinh168@gmail.com
 **/
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class ListItemObserver : IObserver<IList<string>>
{
	private readonly GameObject canvas;
	private readonly GameObject playerPrefab;
	private readonly Action<string> onPlayerIdClicked;

	private GameObject canvasInstance;

	public ListItemObserver(GameObject canvasPrefab, GameObject playerPrefab, Action<string> onPlayerIdClicked)
	{
		this.canvas = canvasPrefab;
		this.playerPrefab = playerPrefab;
		this.onPlayerIdClicked = onPlayerIdClicked;
	}
	public void OnNext(IList<string> list)
	{
		if(canvasInstance == null)
		{
			canvasInstance = GameObject.Instantiate(canvas);
		}
		//delete all child player prefab
		foreach (Transform tf in canvasInstance.transform.GetChild(0))
		{
			tf.gameObject.SetActive(false);
			GameObject.Destroy(tf.gameObject);
		}

		//create players in friend list canvas
		foreach (var playerId in list)
		{
			var playerGO = GameObject.Instantiate(playerPrefab);
			playerGO.GetComponentInChildren<Text>().text = playerId;
			playerGO.transform.SetParent(canvasInstance.transform.GetChild(0));

			playerGO.GetComponent<Button>()
				.OnClickAsObservable()
				.Subscribe(_ =>
				{
					if (onPlayerIdClicked != null)
					{
						onPlayerIdClicked(playerId);
					}
				})
				.AddTo(playerGO);
		}
	}
	public void OnError(Exception error)
	{
		GameObject.Destroy(canvasInstance);
	}

	public void OnCompleted()
	{
		GameObject.Destroy(canvasInstance);
	}
}
