using NUnit.Framework;
using UnityEngine;
using UnityIoC;
using UnityIoC.Editor;
using Resources = UnityEngine.Resources;

namespace UnityIoC.Editor
{
    [TestFixture]
    public class BindingSettingTests
    {
        [Test]
        public void t1_default_binding_settings()
        {
            var context = new AssemblyContext(this);
            var testI = context.Resolve<TestInterface>();
            Assert.IsNotNull(testI);
            Assert.IsInstanceOf<TestClass>(testI);
            testI.DoSomething();
        }

        [Test]
        public void t2_load_binding_settings()
        {
            var context = new AssemblyContext(this);
            context.LoadBindingSetting(Resources.Load<InjectIntoBindingSetting>("not_default"));
            var testI = context.Resolve<TestInterface>();
            Assert.IsNotNull(testI);
        }
    }
}