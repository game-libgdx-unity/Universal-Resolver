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
        Context.Setting.CreateViewFromPool = true;
        
        foreach (var friendName in GetFriendNames())
        {
            Context.Resolve<PlayerData>(friendName);
        }
    }

    /// <summary>
    /// Demonstrate how to modify objects from context cache
    /// </summary>
    void ChangeName()
    {
        // check number of cached object
        var objCount = Context.ResolvedObjects[typeof(PlayerData)].Count;
        Debug.Log(objCount);
        Debug.Assert(GetFriendNames().Count() == objCount);
    }

    IEnumerable<string> GetFriendNames()
    {
        yield return "Jane";
        yield return "John";
        yield return "Jim";
        yield return "David";
    }
}