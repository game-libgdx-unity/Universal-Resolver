using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityIoC;

public class ContextEditModeTests
{
    private Context _context;

    [SetUp]
    public void Setup()
    {
        _context = new Context();
        Debug.Log("Setup test for " + GetType());
        
        Context.DeleteDefaultInstance();
    }

    [Test]
    public void t01_Create_Object_Instance()
    {
        //arrange: do setup context obj for this test

        _context.loadDefaultSetting = false;


        var r = new FirebaseManager.SendFriendRequest {created = 1};

        _context.Bind<FirebaseManager.SendFriendRequest>(r);

        //act: try to resolve
        var obj = _context.Resolve<FirebaseManager.SendFriendRequest>(LifeCycle.Singleton);

        //assert
        Assert.AreEqual(obj.created, 1);
    }

    [Test]
    public void t02_Create_Object_Transient()
    {
        //arrange: do setup context obj for this test

        _context.loadDefaultSetting = false;


        _context.Bind<FirebaseManager.SendFriendRequest>(LifeCycle.Transient);

        //act: try to resolve
        var obj = _context.Resolve<FirebaseManager.SendFriendRequest>();

        //assert
        Assert.IsNotNull(obj);
    }
    
    [Test]
    public void t03_InjectInto()
    {
        var obj = _context.Resolve<TestClass>(LifeCycle.Transient);
        Assert.IsInstanceOf(typeof(Dependency), obj.PropertyAsInterface);
        Assert.IsInstanceOf(typeof(Dependency), obj.SingletonAsInterface);
        
        var obj2 = _context.Resolve<TestComponent>(LifeCycle.Transient);
        Assert.IsInstanceOf(typeof(AnotherDependency), obj2.PropertyAsInterface);
        Assert.IsInstanceOf(typeof(AnotherDependency), obj2.SingletonAsInterface);
    }
    
    [Test]
    public void t04_Inject_Component()
    {
        var obj = _context.Resolve<ComponentAttributeTest>();
        
        Assert.IsNotNull(obj.ComponentTest);
        
        Assert.AreSame(obj.gameObject, obj.ComponentTest.gameObject);
    }
    
    [Test]
    public void t05_Inject_Transient_Component()
    {
        var obj = _context.Resolve<ComponentAttributeTest>();
        
        Assert.IsNotNull(obj.ComponentTest);
//        Assert.IsNotNull(obj.TransientComponentTest);
        
        Assert.AreSame(obj.gameObject, obj.ComponentTest.gameObject);
        Assert.AreNotSame(obj.gameObject, obj.TransientComponentTest.gameObject);
    }
    
    [Test]
    public void t06_Inject_Singleton_Component()
    {
        var obj = _context.Resolve<ComponentAttributeTest>();
        
        Assert.IsNotNull(obj.ComponentTest);
//        Assert.IsNotNull(obj.TransientComponentTest);
        
        Assert.AreSame(obj.gameObject, obj.ComponentTest.gameObject);
        Assert.AreNotSame(obj.gameObject, obj.SingletonComponentTest.gameObject);
    }
}