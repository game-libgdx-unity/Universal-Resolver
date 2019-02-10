using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityIoC;

[TestFixture]
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
        Assert.IsNotNull(component.TransientComponent);   
    }
    
    [Test]
    public void t2_resolve_component_attribute()
    {
        var component = context.Resolve<AnotherComponent>();
        Assert.IsInstanceOf(typeof(SomeComponent), component.SomeComponent);       
        Assert.IsInstanceOf(typeof(SomeComponent), component.TransientComponent);       
        Assert.AreSame(component.gameObject, ((SomeComponent)component.SomeComponent).gameObject);
        Assert.AreNotSame(component.gameObject, ((SomeComponent)component.TransientComponent).gameObject);
    }

    [Test]
    public void t3_resolve_array_of_components_prefab()
    {
        var arrayOfComponent = context.Resolve<ArrayOfComponent>();
        
        Assert.IsNotNull(arrayOfComponent);       
        Assert.IsNotNull(arrayOfComponent.SomeComponents);       
        Assert.AreEqual(3, arrayOfComponent.SomeComponents.Length);
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
