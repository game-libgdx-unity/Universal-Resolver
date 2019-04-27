using System.Collections;
using System.Collections.Generic;
using SceneTest;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class PlayerDataUI : MonoBehaviour
{
	[SerializeField, Inject] PlayerData playerData;
	
	[SerializeField, Inject] Text txtName;
	
	[SerializeField, Singleton] PlayerListUI playerList;
	
	// Use this for initialization
	void Start ()
	{
		txtName.text = playerData.Name;
		
		//set itself as a child of playerList 
		transform.SetParent(playerList.transform);
	}
}