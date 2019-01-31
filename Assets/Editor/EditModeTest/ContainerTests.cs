using System.Collections;
using NUnit.Framework;
using UnityIoC;
using UnityEngine;
using UnityEngine.TestTools;


public class ContainerTests
{
    private GameObject gameObject;
    private Context context;
    
    [SetUp]
    public void Setup()
    {
        gameObject = new GameObject();
        context = new Context();
        context.Initialize(typeof(Context));
    }

    [Test]
    public void t1_GameObject_NotNull()
    {
        Assert.IsNotNull(context);
        Assert.IsNotNull(context.Resolve<SingletonClass>());
    }

    [Test]
    public void t2_InjectInto()
    {
        var obj = context.Resolve<TestClass>(LifeCycle.Transient);
        Assert.IsInstanceOf(typeof(Dependency), obj.PropertyAsInterface);
        Assert.IsInstanceOf(typeof(Dependency), obj.SingletonAsInterface);
        
        var obj2 = context.Resolve<TestComponent>(LifeCycle.Transient);
        Assert.IsInstanceOf(typeof(AnotherDependency), obj2.PropertyAsInterface);
        Assert.IsInstanceOf(typeof(AnotherDependency), obj2.SingletonAsInterface);
    }  
    
    [Test]
    public void t2b_InjectInto()
    {
        var obj3 = context.Resolve<SingletonClass>(LifeCycle.Transient);
        Assert.IsInstanceOf(typeof(SingletonClass), obj3);
        Assert.IsInstanceOf(typeof(Dependency), obj3.PropertyAsInterface);
        Assert.IsInstanceOf(typeof(Dependency), obj3.SingletonAsInterface);
        
        
        var obj2 = context.Resolve<TestComponent>(LifeCycle.Transient);
        Assert.IsInstanceOf(typeof(TestComponent), obj2);
        Assert.IsInstanceOf(typeof(AnotherDependency), obj2.PropertyAsInterface);
        Assert.IsInstanceOf(typeof(AnotherDependency), obj2.SingletonAsInterface);
        
        var obj = context.Resolve<TestClass>(LifeCycle.Transient);
        Assert.IsInstanceOf(typeof(TestClass), obj);
        Assert.IsInstanceOf(typeof(Dependency), obj.PropertyAsInterface);
        Assert.IsInstanceOf(typeof(Dependency), obj.SingletonAsInterface);
        
        var obj4 = context.Resolve<SingletonComponent>(LifeCycle.Transient);
        Assert.IsInstanceOf(typeof(SingletonComponent), obj4);
        Assert.IsInstanceOf(typeof(AnotherDependency), obj4.PropertyAsInterface);
        Assert.IsInstanceOf(typeof(AnotherDependency), obj4.SingletonAsInterface);

    }
    
    [Test]
    public void t3_TransientSingleton_dependency()
    {
        var obj = context.Resolve<TestClass>(LifeCycle.Transient);
        obj.SingletonDependency.SomeIntProperty = 1;
        obj.TransientSingletonDependency.SomeIntProperty = 2;
        
        var obj2 = context.Resolve<TestClass>(LifeCycle.Transient);
        obj2.SingletonDependency.SomeIntProperty = 3;
        obj2.TransientSingletonDependency.SomeIntProperty = 4;
        
        var obj3 = context.Resolve<TestClass>(LifeCycle.Transient);
        obj3.SingletonDependency.SomeIntProperty = 5;
        obj3.TransientSingletonDependency.SomeIntProperty = 6;
        
        var obj4 = context.Resolve<TestClass>(LifeCycle.Transient);
        Assert.AreEqual(5, obj4.SingletonDependency.SomeIntProperty);
        Assert.AreEqual(0, obj4.TransientSingletonDependency.SomeIntProperty);
    }
    [Test]
    public void t4_Singleton_dependency()
    {
        var obj = context.Resolve<TestClass>(LifeCycle.Transient);
        obj.SingletonDependency.SomeIntProperty = 1;
        obj.TransientSingletonDependency.SomeIntProperty = 2;
        
        var obj3 = context.Resolve<TestClass>(LifeCycle.Transient);
        obj3.SingletonDependency.SomeIntProperty = 5;
        obj3.TransientSingletonDependency.SomeIntProperty = 6;
        
        var obj4 = context.Resolve<TestClass>(LifeCycle.Transient);
        Assert.AreEqual(5, obj4.SingletonDependency.SomeIntProperty);
        Assert.AreEqual(0, obj4.TransientSingletonDependency.SomeIntProperty);
    }

    [Test]
    public void Test1_NoBind_Inject()
    {
    }
}