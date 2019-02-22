using System.Collections;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using UnityIoC;
using UnityEngine;
using UnityEngine.TestTools;
using Debug = UnityIoC.Debug;


public class ContainerTests
{
    private Context context;

    [SetUp]
    public void Setup()
    {
        context = new Context();
    }

    [Test]
    public void t1_GameObject_NotNull()
    {
        Assert.IsNotNull(context);
    }

    [Test]
    public void t2_TestClassImplement()
    {
        var obj = context.Resolve<TestClass>(LifeCycle.Default);
        Assert.IsInstanceOf(typeof(TestClassImplement), obj.SomeInterface);
    }

    [Test]
    public void t3_DefaultClassImplement()
    {
        var obj = context.Resolve<AnotherTestClass>(LifeCycle.Default);
        Assert.IsInstanceOf(typeof(AnotherImplement), obj.SomeInterface);
        var obj2 = context.Resolve<TestClass>(LifeCycle.Default);
        Assert.IsInstanceOf(typeof(TestClassImplement), obj2.SomeInterface);
    }

    [Test]
    public void t4_InjectInto()
    {
        var obj = context.Resolve<TestClass>(LifeCycle.Transient);
        Assert.IsInstanceOf(typeof(TestClass), obj);
        Assert.IsInstanceOf(typeof(TestClassImplement), obj.SomeInterface);
    }

    [Test]
    public void t6_ISomeThing()
    {
        var obj = context.Resolve<ISomeThing>(LifeCycle.Transient);
        Assert.IsInstanceOf(typeof(DefaultSomethingImplement), obj);
    }

    [Test]
    public void t7_NoBind_Inject()
    {
        var types = Assembly.GetAssembly(typeof(Context)).GetTypes();

        //find interface with only 1 implement
        var interfaces = types.Where(t => t.IsInterface);
        foreach (var interfaceType in interfaces)
        {
            var classType = types.FirstOrDefault(t =>
                t.GetCustomAttributes(typeof(BindingAttribute), true).Length == 0 &&
                t.GetInterface(interfaceType.Name) != null);

            if (classType != null)
            {
                Debug.LogFormat("Found {0} for {1}", classType, interfaceType);
            }
        }
    }

    [Test]
    public void t8_Internally_Bind_IContainer()
    {
        var container = context.Resolve<IContainer>(LifeCycle.Singleton);

        Assert.IsNotNull(container);
        Assert.AreSame(context.GetContainer(), container);
    }
    [Test]
    public void t9_Resolve_component()
    {
        var component = context.Resolve<ComponentAttributeTest>();

        Assert.IsNotNull(component);
        Assert.IsNotNull(component.SingletonComponentTest);
        Assert.IsNotNull(component.TransientComponentTest);
        Assert.IsNotNull(component.ComponentTest);
    }
}