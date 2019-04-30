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
        }

        Invoke("ChangeName", 1);
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

        //update by filter
        Context.Update<PlayerData>(
            p => p.name == "John",
            data => data.name = "Vinh" //update John to Vinh from cache
        );

        //delete by filter
        Context.Delete<PlayerData>(
            p => p.name == "Jim"
        );

        //update by ref
        var jane = Context.GetObject<PlayerData>(p => p.name == "Jane");
        Context.Update(jane, p => p.name = "Nguyen");

        //update by filter but this object has been deleted already
        Context.Update<PlayerData>(
            p => p.name == "Jim",
            data => data.name = "Hm... not found" //should have no action
        );

        //now check number of cached object
        objCount = Context.ResolvedObjects[typeof(PlayerData)].Count;
        Debug.Log(objCount);
        Debug.Assert(objCount == 3);

        //continue trying to create another player
        var jimmy = Context.Resolve<PlayerData>("Jimmy");

        //update by ref
        Context.Update(jimmy, p => p.name = "Dung");

        //try to delete david
        Context.Delete<PlayerData>(p => p.name == "David");
    }

    IEnumerable<string> GetFriendNames()
    {
        yield return "Jane";
        yield return "John";
        yield return "Jim";
        yield return "David";
    }
}