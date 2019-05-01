using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityIoC;

public class FriendList : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        foreach (var friendName in GetFriendNames())
        {
            var friendData = Context.Resolve<FriendData>(friendName);
        }   
    }

    IEnumerable<string> GetFriendNames()
    {
        yield return "Jane";
        yield return "Jim";
        yield return "Nguyen";
        yield return "Mark";
    }
}
