//using System.Collections;
//using NUnit.Framework;
//using SimpleIoc;
//using UnityEngine.TestTools;
//
//public class ClientEditTests
//{
//    private Client client1;
//    private Client client2;
//    private Context context;
//
//    [SetUp]
//    public void ZalClientTestRunnerSimplePasses()
//    {
//        Client.AutoLogin = false;
//        Client.EnableLogging = true;
//
//        context = new Context();
//        client1 = context.Resolve<Client>();
//        client2 = context.Resolve<Client>();
//        
//        client1.Connect("Vinh", null);
//        client2.Connect("John", null);
//    }
//    
//    [UnityTest]
//    public IEnumerator t1_clients_connectedTo_server()
//    {
//        while (!client1.IsReady.Value)
//        {
//            yield return null;
//        }
//        
//        Assert.Pass();
//    }
//}