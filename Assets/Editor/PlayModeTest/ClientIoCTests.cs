using System.Collections;
using System.Collections.Generic;
using Google.JarResolver;
using NUnit.Framework;
using UnityIoC;
using UnityEngine;
using UnityEngine.TestTools;

[TestFixture]
public class ClientIoCTests
{
    private Client client1;
    private Client client2;
    private Context context;


    [SetUp]
    public void Setup()
    {
        context = new Context();
        client1 = context.Resolve<Client>();
        client2 = context.Resolve<Client>();
        
        client1.Connect("Vin");
        client2.Connect("John");
        
        Context.DeleteDefaultInstance();
    }

    [UnityTest]
    public IEnumerator t1_client_connectedTo_server()
    {
        yield return client1.IsReady.IsValued(true);
        yield return client2.IsReady.IsValued(true);
    }
    
    [UnityTest]
    public IEnumerator t2_client_sendChallengeRequest()
    {
        yield return new WaitUntil(() => client1.IsReady.Value && client2.IsReady.Value);

        client1.SendChallengeRequest("John");

        yield return client2.NewChallengeRequest.IsValued("Vin");
    }

    [Test]
    public void t3_ResolveFrom_Interface()
    {
        var obj = Context.DefaultInstance.Resolve<ISomeInterface>();
        Assert.IsInstanceOf(typeof(Dependency), obj);
    }
    [Test]
    public void t4_TestClass_TestPropertyAsInterface()
    {
        var obj = Context.DefaultInstance.Resolve<TestClass>();
        Assert.IsNotNull(obj);
        Assert.IsNotNull(obj.PropertyAsInterface);
        Assert.IsInstanceOf(typeof(Dependency), obj.PropertyAsInterface);
    }
    [Test]
    public void t5_Singleton_Transient()
    {
        var obj = context.Resolve<TestClass>(LifeCycle.Singleton);
        obj.PropertyAsInterface.SomeIntProperty = 10;

        var obj2 = context.Resolve<TestClass>(LifeCycle.Transient);
        obj2.PropertyAsInterface.SomeIntProperty = 5;

        var obj3 = context.Resolve<TestClass>(LifeCycle.Singleton);
        Assert.AreEqual(10, obj3.PropertyAsInterface.SomeIntProperty);
        
        var obj4 = context.Resolve<TestClass>(LifeCycle.Transient);
        Assert.AreNotEqual(5, obj4.PropertyAsInterface.SomeIntProperty);
        Assert.AreEqual(0, obj4.PropertyAsInterface.SomeIntProperty);
    }
}