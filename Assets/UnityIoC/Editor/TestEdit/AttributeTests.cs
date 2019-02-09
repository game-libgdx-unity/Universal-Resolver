using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityIoC;

public class AttributeTests {

    
    private Context context;

    [SetUp]
    public void Setup()
    {
        context = new Context();
    }
    
    [Test]
    public void t1_automatic_bind_component_attribute()
    {
        var component = context.Resolve<AnotherComponent>();
        Assert.IsNotNull(component);       
        Assert.IsNotNull(component.SomeComponent);       
    }
    [Test]
    public void t2_resolve_component_attribute()
    {
        var component = context.Resolve<AnotherComponent>();
        Assert.IsInstanceOf(typeof(SomeComponent), component.SomeComponent);       
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
