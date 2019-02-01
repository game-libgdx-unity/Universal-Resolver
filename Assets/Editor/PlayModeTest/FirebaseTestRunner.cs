using System.Collections;
using NUnit.Framework;
using UniRx;
using UnityEngine;
using UnityEngine.TestTools;

public class FirebaseTestRunner
{
    private Client client;
    private Client client2;

    [SetUp]
    public void Setup()
    {
        Client.AutoLogin = false;
        Client.EnableLogging = true;

        var gameObject = new GameObject();
        client = gameObject.AddComponent<Client>();
        client.Connect("Vinh", null);
        client.gameObject.AddComponent<FirebaseSupport>();

        gameObject = new GameObject();
        client2 = gameObject.AddComponent<Client>();
        client2.Connect("John", null);
        client2.gameObject.AddComponent<FirebaseSupport>();
    }

    [UnityTest]
    public IEnumerator t1_client_saveOnFirebase()
    {
        yield return new WaitUntil(() =>
            client.IsReady.Value &&
            client2.IsReady.Value &&
            client.GetComponent<FirebaseSupport>().IsReady.Value &&
            client2.GetComponent<FirebaseSupport>().IsReady.Value);

        client.SendFriendRequest("John");

        yield return client2.FriendRequestIncoming.IsValued("Vinh");
        yield return client.GetComponent<FirebaseSupport>().IsLoading.IsValued(false);
    }

    [UnityTest]
    public IEnumerator t2_clientReadFriendRequest()
    {
        yield return new WaitUntil(() =>
            client2.IsReady.Value &&
            client2.GetComponent<FirebaseSupport>().IsReady.Value);

        yield return client2.GetComponent<FirebaseSupport>().GetFriendRequests()
            .Select(lst => lst.Count).IsValued(1);
    }

    [UnityTest]
    public IEnumerator t3_firebase_not_existing()
    {
        yield return new WaitUntil(() =>
            client.IsReady.Value &&
            client.GetComponent<FirebaseSupport>().IsReady.Value);


        yield return client.GetComponent<FirebaseSupport>().IsExisting("some path that doesn't exist").IsValued(false);
    }

    [UnityTest]
    public IEnumerator t4_firebase_existing()
    {
        yield return new WaitUntil(() =>
            client.IsReady.Value &&
            client.GetComponent<FirebaseSupport>().IsReady.Value);


        yield return client.GetComponent<FirebaseSupport>().IsExisting("friends/Vinh/John").Is(true);
    }
}