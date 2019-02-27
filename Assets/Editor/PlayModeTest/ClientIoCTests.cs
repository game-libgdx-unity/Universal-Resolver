using System.Collections;
using System.Collections.Generic;
using Google.JarResolver;
using NUnit.Framework;
using UnityIoC;
using UnityEngine;
using UnityEngine.TestTools;
using UnityIoC.Editor;

[TestFixture]
public class ClientIoCTests
{
    private Client client1;
    private Client client2;
    private AssemblyContext assemblyContext;


    [SetUp]
    public void Setup()
    {
        assemblyContext = new AssemblyContext();
        client1 = assemblyContext.Resolve<Client>();
        client2 = assemblyContext.Resolve<Client>();
        
        client1.Connect("Vin");
        client2.Connect("John");
        
        AssemblyContext.DisposeDefaultInstance();
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
        var obj = AssemblyContext.GetDefaultInstance().Resolve<ISomeInterface>();
        Assert.IsInstanceOf(typeof(Dependency), obj);
    }
    [Test]
    public void t4_TestClass_TestPropertyAsInterface()
    {
        var obj = AssemblyContext.GetDefaultInstance().Resolve<TestClass2>();
        Assert.IsNotNull(obj);
        Assert.IsNotNull(obj.PropertyAsInterface);
        Assert.IsInstanceOf(typeof(Dependency), obj.PropertyAsInterface);
    }
    [Test]
    public void t5_Singleton_Transient()
    {
        var obj = assemblyContext.Resolve<TestClass2>(LifeCycle.Singleton);
        obj.PropertyAsInterface.SomeIntProperty = 10;

        var obj2 = assemblyContext.Resolve<TestClass2>(LifeCycle.Transient);
        obj2.PropertyAsInterface.SomeIntProperty = 5;

        var obj3 = assemblyContext.Resolve<TestClass2>(LifeCycle.Singleton);
        Assert.AreEqual(10, obj3.PropertyAsInterface.SomeIntProperty);
        
        var obj4 = assemblyContext.Resolve<TestClass2>(LifeCycle.Transient);
        Assert.AreNotEqual(5, obj4.PropertyAsInterface.SomeIntProperty);
        Assert.AreEqual(0, obj4.PropertyAsInterface.SomeIntProperty);
    }
}