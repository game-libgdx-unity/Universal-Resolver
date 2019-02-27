using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using UnityIoC;

public class NewFirebaseTestRunner
{
    [UnityTest]
    public IEnumerator t1_client_saveOnFirebase()
    {
        var _context = new AssemblyContext();

        var firebase1 = _context.Resolve<FirebaseSupportNew>();
        var firebase2 = _context.Resolve<FirebaseSupportNew>();

        var client1 = firebase1.GetComponent<Client>();
        var client2 = firebase2.GetComponent<Client>();
        
        client1.Connect("Vin");
        client2.Connect("John");
        
        yield return new WaitUntil(() =>
            client1.IsReady.Value &&
            client2.IsReady.Value &&
            firebase1.GetComponent<FirebaseSupportNew>().IsReady.Value &&
            firebase2.GetComponent<FirebaseSupportNew>().IsReady.Value);

        client1.SendFriendRequest("John");

        yield return client2.FriendRequestIncoming.Is("Vin");
        yield return client1.GetComponent<FirebaseSupportNew>().IsLoading.Is(false);
    }
}