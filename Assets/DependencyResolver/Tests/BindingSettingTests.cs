using NUnit.Framework;
using NUnit.Framework.Interfaces;
using UnityEngine;
using UnityIoC;
using UnityIoC.Editor;
using Resources = UnityEngine.Resources;

namespace UnityIoC.Editor
{
    public class BindingSettingTests : TestBase
    {
        [Test]
        public void t1_default_binding_settings()
        {
            var context = new Context(this);
            var testI = context.ResolveObject<TestInterface>();
            Assert.IsNotNull(testI);
            Assert.IsInstanceOf<TestClass>(testI);
            testI.DoSomething();
        }

        [Test]
        public void t2_load_binding_settings()
        {
            var context = new Context(this);
            context.LoadBindingSetting(MyResources.Load<InjectIntoBindingSetting>("not_default"));
            var testI = context.ResolveObject<TestInterface>();
            Assert.IsNotNull(testI);
        }
        
        [Test]
        public void t3_object_context_TestClass()
        {
            TestClass obj = new TestClass();
            var context = new ObjectContext(obj);
            
            var testObj = context.Resolve<TestInterface>();
            Assert.IsNotNull(testObj);
        }
        
        [Test]
        public void t4_object_context_TestClass2()
        {
            var obj = new TestClass2();
            var context = new ObjectContext(obj);
            
            var testObj = context.Resolve<TestInterface>();
            testObj.DoSomething();
            
            Assert.IsNotNull(testObj);
        }
    }
}