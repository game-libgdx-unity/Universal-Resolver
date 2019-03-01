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
    }
}