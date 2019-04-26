using System;
using System.Collections;
using System.Collections.Generic;
using SceneTest;
using UnityEngine;
using UnityEngine.UI;
using UnityIoC;

public class PlayerDataUI : MonoBehaviour
{
	[SerializeField, Inject] PlayerData playerData;
	
	[SerializeField, Inject] private Text txtName;
	
	[SerializeField, Singleton] private PlayerListUI playerList;
	
	// Use this for initialization
	void Start ()
	{
		Debug.Assert(playerData != null);
		Debug.Assert(txtName);
		
		txtName.text = playerData.Name;
		
		//set itself as a child of playerList 
		transform.SetParent(playerList.transform);
	}
}

[Serializable]
public class PlayerData : IDataBinding<PlayerDataUI>
{
	public string Name;

//	public PlayerData(string name)
//	{
//		Name = name;
//	}
}
