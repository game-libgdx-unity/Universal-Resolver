using NUnit.Framework;
using UnityEngine;
using UnityIoC;

[TestFixture]
public class BindingSettingTests
{
    [Test]
    public void t1_default_binding_settings()
    {
        var context = new Context(GetType());
        context.LoadDefaultBindingSetting();
        
        var testI = context.Resolve<TestI>();

        Assert.IsNotNull(testI);
        Assert.IsInstanceOf<TestC>(testI);
        testI.DoSomething();
    }
    
    [Test]
    public void t2_load_binding_settings()
    {
        var context = new Context(GetType());
        context.LoadBindingSetting(Resources.Load<ImplementClass>("not_default"));
        
        var testI = context.Resolve<TestI>();

        Assert.IsNotNull(testI);
        Assert.IsInstanceOf<TestC2>(testI);
    }
}