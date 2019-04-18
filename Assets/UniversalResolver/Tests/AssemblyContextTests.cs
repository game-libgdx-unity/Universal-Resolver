using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using NUnit.Framework;
using SceneTest;

namespace UnityIoC.Editor
{
    public class AssemblyContextTests : TestBase
    {
        [Test]
        public void t1_get_default_instance()
        {
            Assert.IsNotNull(Context.DefaultInstance);
        }


        [Test]
        public void t2_bind_from_tests_setting()
        {
            var obj = Context.Resolve<TestInterface>();
            Assert.IsNotNull(obj);
        }


        [Test]
        public void t3_bind_types()
        {
            //TestClass implements the TestInterface
            Context.Bind<TestInterface, TestClass>();
            var obj = Context.Resolve<TestInterface>();
            Assert.IsNotNull(obj);
        }


        [Test]
        public void t4_bind_instance()
        {
            var instance = new TestClass();
            Context.Bind(instance);
            var obj = Context.Resolve<TestInterface>();
            Assert.IsNotNull(obj);
        }


        [Test]
        public void t5_bind_as()
        {
            var instance = Context.Resolve<TestClass>(LifeCycle.Singleton);
            //assign singleton value to
            var testString = "Hello";
            instance.JustAProperty = testString;

            //try to resolve as transient, this is a brand new object
            var transient = Context.Resolve<TestClass>(LifeCycle.Transient);

            //so the value of the property should be as default
            Assert.IsNull(transient.JustAProperty);

            //now try to resolve as singleton, this should be the previous object which have been created
            var singleton = Context.Resolve<TestClass>(LifeCycle.Singleton);

            //verify by the property's value
            Assert.AreSame(singleton, instance);
            Assert.AreEqual(singleton.JustAProperty, testString);
        }


        [Test]
        public void t6_load_custom_setting()
        {
            Context.Bind(Resources.Load<InjectIntoBindingSetting>("not_default"));
            var obj = Context.Resolve<TestInterface>();
            Assert.IsNotNull(obj);
        }


        [Test]
        public void t7_resolve_unregistered_objects()
        {
            var obj = Context.Resolve<TestClass>();
            Assert.IsNotNull(obj);

            var assem = Assembly.Load("Tests");
            var type = assem.GetType("TestInterface");
            Assert.IsNotNull(type);
        }

        //you can enable this test if running on Unity 2018.3 or newer

#if UNITY_2018_3_OR_NEWER
        [Test]
        public void t8_instantiate_prefabs()
        {
            //create a context which processes the SceneTest assembly
            Context.GetDefaultInstance(typeof(TestComponent));

            //create a gameobject with TestComponent as a prefab
            var prefab = Context.Resolve<TestComponent>();
            prefab.gameObject.SetActive(false);

            //Instantiate the prefab (a.k.a clone)
            var instance = Context.Instantiate(prefab);
            instance.gameObject.SetActive(true);

            Assert.IsNotNull(instance);
            Assert.IsNotNull(instance.@abstract);
        }
#endif
        [Test]
        public void t9_resolve_actions()
        {
            Action<TestInterface> action = t =>
            {
                Assert.IsNotNull(t);
                t.DoSomething();
            };
            Context.GetDefaultInstance(this).ResolveAction(action);
        }

        [Test]
        public void t10_resolve_funcs()
        {
            var obj = Context.Resolve<TestInterface>(LifeCycle.Singleton);
            obj.JustAProperty = "Hello";

            Func<TestInterface, string> func = t =>
            {
                Assert.IsNotNull(t);
                return t.JustAProperty;
            };

            var func_output = Context.GetDefaultInstance(this).ResolveFunc(func, LifeCycle.Singleton);
            Assert.AreEqual(func_output, "Hello");
        }


        [Test]
        public void t11_resolve_with_parameters()
        {
            var obj = Context.Resolve<Impl>(parameters: new object[] {1});
            Assert.AreEqual(obj.a, 1);
            obj = Context.Resolve<Impl>();
            Assert.AreEqual(obj.a, 0);
        }

        [Test]
        public void t12_resolve_with_inject_into()
        {
            var t1 = Context.Resolve<TestComponent>();
            var t2 = Context.Resolve<TestComponent2>();
            var t3 = Context.Resolve<TestComponent3>();

            var i1 = Context.Resolve<IAbstract>(resolveFrom: t1);
            var i2 = Context.Resolve<IAbstract>(resolveFrom: t2);
            var i3 = Context.Resolve<IAbstract>(resolveFrom: t3);

            Assert.IsInstanceOf<Impl>(i1);
            Assert.IsInstanceOf<ImplClass2>(i2);
            Assert.IsInstanceOf<ImplClass2>(i3);
        }

        [Test]
        public void t13_resolve_with_inject_into()
        {
            var t1 = Context.Resolve<TestComponent>();

            ObjectContext objectContext = new ObjectContext(t1);

            var i1 = objectContext.Resolve<IAbstract>();

            Assert.IsInstanceOf<Impl>(i1);
        }

        [Test]
        public void t14_Preload_From_Pools()
        {
            var gameObjects = new List<GameObject>();

            gameObjects.Preload(10, new GameObject());
            Assert.AreEqual(gameObjects.Count, 10);
        }

        [Test]
        public void t15_Get_From_Pools()
        {
            var gameObjects = new List<TestComponent>();

            var obj = gameObjects.GetFromPool();
            var obj2 = gameObjects.GetFromPool();
            var obj3 = gameObjects.GetFromPool();

            Assert.AreEqual(gameObjects.Count, 3);
        }

        [Test]
        public void t16_resolve_with_generic_ObjectContext()
        {
            var i1 = Context.FromObject<TestComponent>().Resolve<IAbstract>();
            var i2 = Context.FromObject<TestComponent2>().Resolve<IAbstract>();
            var i3 = Context.FromObject<TestComponent3>().Resolve<IAbstract>();

            Assert.IsInstanceOf<Impl>(i1);
            Assert.IsInstanceOf<ImplClass2>(i2);
            Assert.IsInstanceOf<ImplClass2>(i3);
        }

        [Test]
        public void t16_recycle_objects_From_Pools()
        {
            var gameObjects = new List<TestComponent>();

            var obj = gameObjects.GetFromPool();
            var obj2 = gameObjects.GetFromPool();
            var obj3 = gameObjects.GetFromPool();

            //put obj back to the pool
            obj2.gameObject.SetActive(false);

            //get an obj out
            var obj4 = gameObjects.GetFromPool();

            //the num of obj in pool shouldn't be changed
            Assert.AreEqual(gameObjects.Count, 3);
        }

        [Test]
        public void t17_get_interfaces_as_components()
        {
            var obj = Context.Resolve<IComponentAbstract>();
            Assert.IsInstanceOf<TestComponent>(obj);
        }


        [Test]
        public void t99_dispose_instance()
        {
            Assert.IsFalse(Context.Initialized);
//create
            Context.GetDefaultInstance(this);

            Assert.IsTrue(Context.Initialized);
//dispose
            Context.DisposeDefaultInstance();
//assert
            Assert.IsFalse(Context.Initialized);
        }


        [TearDown]
        public void Dispose()
        {
            Context.DisposeDefaultInstance();
        }
    }
}