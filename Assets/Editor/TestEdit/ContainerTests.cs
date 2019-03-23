using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace UnityIoC.Editor
{
    [TestFixture]
    public class ContainerTests : TestBase
    {
        [NUnit.Framework.Test]
        public void t1_Resolve_As_Singleton()
        {
            var context = new AssemblyContext(this);

            var singleton1 = context.Resolve<TestInterface>(LifeCycle.Singleton);
            var singleton2 = context.Resolve<TestInterface>(LifeCycle.Singleton);
            
            Assert.AreSame(singleton1, singleton2);
        }

        [NUnit.Framework.Test]
        public void t2_Resolve_As_Transient()
        {
            var context = new AssemblyContext(this);

            var singleton1 = context.Resolve<TestInterface>(LifeCycle.Transient);
            var singleton2 = context.Resolve<TestInterface>(LifeCycle.Transient);
            
            Assert.AreNotSame(singleton1, singleton2);
        }
        
        [NUnit.Framework.Test]
        public void t3_Binding_instance()
        {
            TestInterface obj = new TestClass();
            var context = new AssemblyContext(this);

            context.Bind<TestInterface>(obj);
            
            var obj2 = context.Resolve<TestInterface>();
            Assert.AreSame(obj,obj2);
        }
        
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