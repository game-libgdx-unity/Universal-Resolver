using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class FriendRequestUITests
{
    private Client client;


    [SetUp]
    public void Setup()
    {
        Client.AutoLogin = false;
        Client.EnableLogging = true;

        var gameObject = new GameObject();
        client = gameObject.AddComponent<Client>();
        client.Connect("Someone", null);

        gameObject.AddComponent<FirebaseSupport>();
        gameObject.AddComponent<FriendRequestUI>();
    }

    [UnityTest]
    public IEnumerator Test1_DisplayFriendRequest_UI()
    {
        yield return new WaitUntil(() =>
            client.IsReady.Value &&
            client.GetComponent<FirebaseSupport>().IsReady.Value);
        
        
        yield return new WaitForSeconds(10f);
    }
}