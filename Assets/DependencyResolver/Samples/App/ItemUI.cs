using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityIoC;

public class ItemUI : MonoBehaviour
{
	[SerializeField][Inject] PlayerData playerData;
	
	[SerializeField][Inject] private Text txtName;
	
	// Use this for initialization
	void Start ()
	{
		Context.GetDefaultInstance(this);
		
		Debug.Assert(playerData != null);
		Debug.Assert(txtName != null);
		
		txtName.text = playerData.Name;
	}
}

[Serializable]
public class PlayerData
{
	public string Name;
}
