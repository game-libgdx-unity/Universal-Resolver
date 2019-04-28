using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityIoC;

public class PlayerListUI : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        foreach (var friendName in GetFriendNames())
        {
            Context.Resolve<PlayerData>(friendName);
//            Context.Resolve<PlayerDataUI>();
        }
    }

    IEnumerable<string> GetFriendNames()
    {
        yield return "Jane";
        yield return "John";
        yield return "Jim";
    }
}