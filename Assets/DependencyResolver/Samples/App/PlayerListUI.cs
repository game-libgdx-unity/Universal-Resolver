using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityIoC;

public class PlayerListUI : MonoBehaviour
{
	// Use this for initialization
	void Start ()
	{
		foreach (var friendName in GetFriendNames())
		{
			var playerData = Context.Resolve<PlayerData>();

			playerData.Name = friendName;

			Context.OnResolved.Subscribe(obj =>
			{
				if (obj.GetType() == typeof(PlayerData))
				{
					var item = Context.Resolve<ItemUI>();
				
					item.transform.SetParent(transform);
				}
			});
		}
	}

	IEnumerable<string> GetFriendNames()
	{
		yield return "Jane";
		yield return "John";
		yield return "Jim";
	}
}