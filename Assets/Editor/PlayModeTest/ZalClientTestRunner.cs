using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityIoC;
using UniRx;

public class ZalClientTestRunner
{
    private Client client;
    private Client client2;
    private bool finished;

    [SetUp]
    public void ZalClientTestRunnerSimplePasses()
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
        
        finished = false;
    }

    [UnityTest]
    public IEnumerator t1_client_connectedTo_server()
    {
        yield return client.IsReady.IsValued(true);
        yield return client2.IsReady.IsValued(true);
    }

    [UnityTest]
    public IEnumerator t2_client_sendChallengeRequest()
    {
        yield return new WaitUntil(() => client.IsReady.Value && client2.IsReady.Value);

        client.SendChallengeRequest("John");

        yield return client2.NewChallengeRequest.IsValued("Vinh");
    }

    [UnityTest]
    public IEnumerator t3_client_sendFriendRequest()
    {
        yield return new WaitUntil(() => client.IsReady.Value && client2.IsReady.Value);

        var firebaseSupportCLient1 = client.GetComponent<FirebaseSupport>();
        var firebaseSupportCLient2 = client.GetComponent<FirebaseSupport>();
        
        yield return firebaseSupportCLient1.IsReady.IsValued(true);
        yield return firebaseSupportCLient2.IsReady.IsValued(true);
        
        client.SendFriendRequest("John");

        yield return firebaseSupportCLient1.IsLoading.IsValued(false);
    }

    [UnityTest]
    public IEnumerator t4_firebase_connectedTo_server()
    {
        var firebaseSupportCLient1 = client.GetComponent<FirebaseSupport>();
        var firebaseSupportCLient2 = client.GetComponent<FirebaseSupport>();

        yield return firebaseSupportCLient1.IsReady.IsValued(true);
        yield return firebaseSupportCLient2.IsReady.IsValued(true);
    }

    [UnityTest]
    public IEnumerator t5_client_saveOnFirebase()
    {
        yield return new WaitUntil(() =>
            client.IsReady.Value &&
            client2.IsReady.Value &&
            client.GetComponent<FirebaseSupport>().IsReady.Value &&
            client2.GetComponent<FirebaseSupport>().IsReady.Value);

        client.SendFriendRequest("John");

        yield return client2.FriendRequestIncoming.IsValued("Vinh");

        yield return client2.GetComponent<FirebaseSupport>().IsLoading.IsValued(false);
    }

    [UnityTest]
    public IEnumerator t6_client_SaveFriend_SentMessage()
    {
        yield return new WaitUntil(() =>
            client.IsReady.Value &&
            client.GetComponent<FirebaseSupport>().IsReady.Value);

        client.SendAcceptFriendRequest("John"); //friends/Vinh/John

        yield return client.GetComponent<FirebaseSupport>().IsLoading.IsValued(false);
    }
}