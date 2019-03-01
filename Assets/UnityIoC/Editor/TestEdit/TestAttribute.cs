using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using SceneTest;

namespace UnityIoC.Editor
{
    [TestFixture]
    public class TestAttribute : TestBase
    {
        [NUnit.Framework.Test]
        public void t1_automatic_bind_component_attribute()
        {
            var assemblyContext = new AssemblyContext(this, true);
            var testClass = assemblyContext.Resolve<TestInterface>();
            Assert.IsNotNull(testClass);
            Assert.IsInstanceOf<TestClass>(testClass);
        }

        [NUnit.Framework.Test]
        public void t2_resolve_component_attribute()
        {
            var assemblyContext = new AssemblyContext(typeof(AbstractClass));
            var component = assemblyContext.Resolve<TestComponent>();
            Assert.IsInstanceOf(typeof(ImplClass), component.abstractClass);
        }

        [NUnit.Framework.Test]
        public void t3_resolve_array_of_components_as_variable_from_prefab()
        {
            var assemblyContext = new AssemblyContext(typeof(AbstractClass));
            var arrayOfComponent = assemblyContext.Resolve<ArrayOfComponent>();

            Assert.IsNotNull(arrayOfComponent);
            Assert.IsNotNull(arrayOfComponent.SomeComponents);
            Assert.AreEqual(3, arrayOfComponent.SomeComponents.Length);
        }

        [NUnit.Framework.Test]
        public void t4_resolve_array_of_components_as_property_from_prefab()
        {
            var assemblyContext = new AssemblyContext(typeof(AbstractClass));
            var arrayOfComponent = assemblyContext.Resolve<ArrayOfComponent>();

            Assert.IsNotNull(arrayOfComponent);
            Assert.IsNotNull(arrayOfComponent.SomeComponentsAsProperty);
            Assert.AreEqual(3, arrayOfComponent.SomeComponentsAsProperty.Length);
        }


        [NUnit.Framework.Test]
        public void t5_resolve_array_of_components_from_children_as_variable_by_prefab()
        {
            var assemblyContext = new AssemblyContext(typeof(AbstractClass));
            var arrayOfComponent = assemblyContext.Resolve<ArrayOfComponentFromChildren>();

            Assert.IsNotNull(arrayOfComponent);
            Assert.IsNotNull(arrayOfComponent.SomeComponents);
            Assert.AreEqual(5, arrayOfComponent.SomeComponents.Length);
        }

        [NUnit.Framework.Test]
        public void t6_resolve_array_of_components_from_children_as_property_by_prefab()
        {
            var assemblyContext = new AssemblyContext(typeof(AbstractClass));
            var arrayOfComponent = assemblyContext.Resolve<ArrayOfComponentFromChildren>();

            Assert.IsNotNull(arrayOfComponent);
            Assert.IsNotNull(arrayOfComponent.SomeComponentsAsProperty);
            Assert.AreEqual(5, arrayOfComponent.SomeComponentsAsProperty.Length);
        }
    }
}