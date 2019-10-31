using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using SceneTest;
using UnityEngine;
using UnityEngine.TestTools;

namespace UnityIoC.Editor
{
    [TestFixture]
    public class ContextTests : TestBase
    {
        [Test]
        public void t1_Resolve_As_Singleton()
        {
            var context = new Context(this);

            var singleton1 = context.ResolveObject<TestInterface>(LifeCycle.Singleton);
            var singleton2 = context.ResolveObject<TestInterface>(LifeCycle.Singleton);
            
            Assert.AreSame(singleton1, singleton2);
        }

        [Test]
        public void t2_Resolve_As_Transient()
        {
            var context = new Context(this);

            var singleton1 = context.ResolveObject<TestInterface>(LifeCycle.Transient);
            var singleton2 = context.ResolveObject<TestInterface>(LifeCycle.Transient);
            
            Assert.AreNotSame(singleton1, singleton2);
        }
        
        [Test]
        public void t3_Binding_instance()
        {
            TestInterface obj = new TestClass();
            var context = new Context(this);

            context.Bind<TestInterface>(obj);
            
            var obj2 = context.ResolveObject<TestInterface>();
            Assert.AreSame(obj,obj2);
        }
        
        [Test]
        public void t1_automatic_bind_component_attribute()
        {
            var assemblyContext = new Context(this);
            var testClass = assemblyContext.ResolveObject<TestInterface>();
            Assert.IsNotNull(testClass);
            Assert.IsInstanceOf<TestClass>(testClass);
        }
        [Test]
        public void t2_gameobject_resolvecomponent()
        {
            var gameObject = new GameObject();
            var c = gameObject.AddComponent<TestComponent>();
            var c2 = c.ResolveComponent<TestComponent2>();
            
            Assert.IsNotNull(c2);
            Assert.AreSame(c.gameObject, c2.gameObject);

        }
        
        //you can enable this test if running on Unity 2018.3 or newer
#if UNITY_2018_3_OR_NEWER
        [Test]
        public void t2_resolve_component_attribute()
        {
            var assemblyContext = new Context(typeof(IAbstract));
            var component = assemblyContext.ResolveObject<TestComponent>();
            Assert.IsInstanceOf(typeof(ImplClass), component.@abstract);
        }
#endif
        //this test can't be done due to "Can't add script behaviour ComponentTest because it is an editor script."
        //please check the samples for resolving unity components
        //if you know how to work around it, please email me at mrthanhvinh168@gmail.com
        /*
        
        [NUnit.Framework.Test]
        public void t4_resolve_binding_setting_for_component()
        {
            var context = new AssemblyContext(this);
            context.LoadBindingSetting("resolve_component");
            
            var obj = context.Resolve<TestInterface>();
            Assert.IsInstanceOf<ComponentTest>(obj);
        }
        
        */
    }
}