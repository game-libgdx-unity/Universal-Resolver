using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using SceneTest;
using UnityEngine;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

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
            var c = gameObject.ResolveComponent<TestComponent>();
            
            Assert.IsNotNull(c);
            Assert.AreSame(c.gameObject, gameObject);

        }
        [Test]
        public void t3_gameobject_resolvecomponent()
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
        //this test can't be done due to eror "Can't add script behaviour ComponentTest because it is an editor script."
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
        
             [Test]
        public void t5_bind_life_cycle()
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
            Assert.IsInstanceOf<TestClass2>(obj);
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
            Context.Bind<TestInterface, TestClass>();
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
        public void t10_binding_attributes()
        {
            Context.Bind<TestInterface, TestClass>();
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
        public void t14_Preload_From_Pools()
        {
            var gameObjects = new List<GameObject>();

            gameObjects.Preload(10, new GameObject());
            Assert.AreEqual(gameObjects.Count, 10);
        }


#if UNITY_2018_3_OR_NEWER

        [Test]
        public void t11_resolve_with_parameters()
        {
            var obj = Context.Resolve<ImplClass>(1);
            Assert.AreEqual(obj.a, 1);
            obj = Context.Resolve<ImplClass>();
            Assert.AreEqual(obj.a, 0);
        }

        [Test]
        public void t12_resolve_with_inject_into()
        {            
            var t1 = Context.Resolve<TestComponent>();
            var t2 = Context.Resolve<TestComponent2>();
            var t3 = Context.Resolve<TestComponent3>();

            var i1 = Context.Resolve<IAbstract>(resolveFrom: t1.GetType());
            var i2 = Context.Resolve<IAbstract>(resolveFrom: t2.GetType());
            var i3 = Context.Resolve<IAbstract>(resolveFrom: t3.GetType());

            Assert.IsInstanceOf<ImplClass>(i1);
            Assert.IsInstanceOf<ImplClass2>(i2);
            Assert.IsInstanceOf<ImplClass2>(i3);
        }

        [Test]
        public void t13_resolve_with_object_context()
        {
            var t1 = Context.Resolve<TestComponent>();

            ObjectContext objectContext = new ObjectContext(t1);

            var i1 = objectContext.Resolve<IAbstract>();

            Assert.IsInstanceOf<ImplClass>(i1);
        }


        [Test]
        public void t15_Get_From_Pools()
        {
            var gameObjects = new List<TestComponent>();

            var obj = gameObjects.GetInstanceFromPool<TestComponent>();
            var obj2 = gameObjects.GetInstanceFromPool();
            var obj3 = gameObjects.GetInstanceFromPool();

            Assert.AreEqual(3, gameObjects.Count);
        }

        [Test]
        public void t16_resolve_with_generic_ObjectContext()
        {
            var i1 = Context.FromObject<TestComponent>().Resolve<IAbstract>();
            var i2 = Context.FromObject<TestComponent2>().Resolve<IAbstract>();
            var i3 = Context.FromObject<TestComponent3>().Resolve<IAbstract>();

            Assert.IsInstanceOf<ImplClass>(i1);
            Assert.IsInstanceOf<ImplClass2>(i2);
            Assert.IsInstanceOf<ImplClass2>(i3);
        }

        [Test]
        public void t16_recycle_objects_From_Pools()
        {
            var gameObjects = new List<TestComponent>();

            var obj = gameObjects.GetInstanceFromPool();
            var obj2 = gameObjects.GetInstanceFromPool();
            var obj3 = gameObjects.GetInstanceFromPool();

            //put obj back to the pool
            obj2.gameObject.SetActive(false);

            //get an obj out
            var obj4 = gameObjects.GetInstanceFromPool();

            //the num of obj in pool shouldn't be changed
            Assert.AreEqual(gameObjects.Count, 3);
        }

        [Test]
        public void t17_get_interfaces_as_components()
        {
            var obj = Context.Resolve<IComponentAbstract>();
            Assert.IsNotNull(obj);
            Assert.IsInstanceOf<TestComponent>(obj);
        }

        [Test]
        public void t18_get_from_pools()
        {
            var testComponentPool = Context.GetPool<TestComponent>();
            
            Assert.AreEqual(testComponentPool.Count, 0);

            var testComponent2Pool = Context.GetPool<TestComponent2>();
            
            Assert.AreEqual(testComponent2Pool.Count, 0);


            var obj = testComponentPool.GetInstanceFromPool();
            
            Assert.AreEqual(1, testComponentPool.Count);

            var obj2 = testComponentPool.GetInstanceFromPool();
            var obj3 = testComponentPool.GetInstanceFromPool();
            
            var obj5 = testComponent2Pool.GetInstanceFromPool();
            var obj6 = testComponent2Pool.GetInstanceFromPool();
            var obj7 = testComponent2Pool.GetInstanceFromPool();

            obj2.gameObject.SetActive(false);

            var obj4 = testComponentPool.GetInstanceFromPool();
            Assert.AreEqual(testComponentPool.Count, 3);

            obj6.gameObject.SetActive(false);
            var obj8 = testComponent2Pool.GetInstanceFromPool();
            Assert.AreEqual(testComponent2Pool.Count, 3);
        }

        [Test]
        public void t19_test_IObjectResolvable()
        {
            //new prefab
            var go = new GameObject();
            go.AddComponent<TestComponent>();
            go.AddComponent<TestComponent5>();

            //process inject attributes
            var context = Context.GetDefaultInstance(typeof(TestComponent));
            context.ProcessInjectAttribute(go);

            //assert
            Assert.IsNotNull(go.GetComponent<TestComponent>().@abstract);
            Assert.IsNotNull(go.GetComponent<TestComponent5>().getFromGameObject);
            Assert.AreSame(
                go.GetComponent<TestComponent>().@abstract,
                go.GetComponent<TestComponent5>().getFromGameObject
            );
        }

        [Test]
        public void t20_test_IObjectResolvableGeneric()
        {
            //new prefab
            var go = new GameObject(); 
            go.AddComponent<TestComponent2>();
            go.AddComponent<TestComponent5>();

            Context.GetDefaultInstance(typeof(TestComponent2)).ProcessInjectAttribute(go);

            //process inject attributes
            var context = Context.GetDefaultInstance(typeof(TestComponent2));
            context.ProcessInjectAttribute(go);

            //assert
            Assert.IsNotNull(go.GetComponent<TestComponent2>().@abstract);
            Assert.IsNotNull(go.GetComponent<TestComponent5>().getFromGameObject);
            Assert.AreSame(
                go.GetComponent<TestComponent2>().@abstract,
                go.GetComponent<TestComponent5>().getFromGameObject
            );
        }
#endif

        [Test]
        public void t21_test_inject_attributes_no_caches()
        {
            //resolve a component
            var comp = Context.Resolve<JustUnityComponent>();

            //JustUnityComponent using [Inject] attribute, which returns a new obj if nothing found in the cache
            Assert.IsNotNull(comp.justDTOClass);
        }

        [Test]
        public void t22_test_inject_attributes_caches()
        {
            //resolve a class
            var justDtoClass = Context.Resolve<JustDTOClass>();
            var value = 10;

            justDtoClass.justAField = value;

            //resolve a component
            var comp = Context.Resolve<JustUnityComponent>();

            //JustUnityComponent using [Inject] attribute which support caches, so 
            Assert.IsNotNull(comp.justDTOClass);
            Assert.AreEqual(comp.justDTOClass.justAField, value);
            Assert.AreSame(comp.justDTOClass, justDtoClass);
        }

        [Test]
        public void t24_test_get_component()
        {
            //create game obj
            GameObject gameObject = new GameObject();
            gameObject.GetOrAddComponent<TestComponent>();

            //try to get its component
            var comp = Context.ResolveFromHierarchy(typeof(IComponentAbstract), gameObject, null);
            //resolve a component

            Assert.IsNotNull(comp);
            Assert.IsInstanceOf<TestComponent>(comp);
            Assert.IsTrue(gameObject.GetComponents(typeof(IComponentAbstract)).Length == 1);
        }

        [Test]
        public void t25_test_get_component_generic()
        {
            //create game obj
            GameObject gameObject = new GameObject();
            gameObject.GetOrAddComponent<TestComponent>();

            //try to get its component
            var comp = Context.ResolveFromHierarchy<IComponentAbstract>(gameObject, null);
            //resolve a component

            Assert.IsNotNull(comp);
            Assert.IsInstanceOf<TestComponent>(comp);
            Assert.IsTrue(gameObject.GetComponents<IComponentAbstract>().Length == 1);
        }

        [Test]
        public void t26_test_get_object()
        {
            //create game obj
            GameObject gameObject = new GameObject();
            gameObject.GetOrAddComponent<TestComponent>();

            //try to get its component
            var comp = Context.ResolveFromHierarchy<IComponentAbstract>(gameObject, null);
            //resolve a component

            Assert.IsNotNull(comp);
            Assert.IsInstanceOf<TestComponent>(comp);
            Assert.IsTrue(gameObject.GetComponents<IComponentAbstract>().Length == 1);
        }

        [Test]
        public void t27_get_from_json()
        {
            //create obj from json
            var json = "{\"userId\": 1}";
            var userData = Context.ResolveFromJson<UserData>(json);

            //verify it
            Assert.AreEqual(1, userData.userId);
        }

        [UnityTest]
        public IEnumerator t28_post_raw_result()
        {
            //create obj from json
            var json = "{\"userId\": 1}";
            var link = "https://us-central1-zalgame-1ae27.cloudfunctions.net/checkServer";
            var request = "{\"serverip\":\"127.0.0.1\"}";

            //call the api
            yield return Context.Post(link, request,
                msg => { Assert.AreEqual("OK", msg); },
                error => { Assert.Fail("Error: " + error); });
        }

        [Test]
        public void t29_test_IDataBinding()
        {
            Context.Resolve<UserData>();
            var userDataView = Object.FindObjectOfType<UserDataView>();
            Assert.IsNotNull(userDataView);
        }

        [Test]
        public void t30_get_object_IObjectResolvable()
        {
            //arrange
            var comp = Context.Resolve<TestComponent>();
            var obj = comp.@abstract;

            //act
            //try to get object from the comp
            var get = Context.GetObject<IAbstract>(comp);

            //assert
            Assert.AreSame(obj, get);
        }
        
        [Test]
        public void t35_data_linked_multiple_views()
        {
            var obj = Context.Resolve<UserData>();
            var view1 = Object.FindObjectOfType<UserDataView>();
            var view2 = Object.FindObjectOfType<UserDataView2>();

            Assert.IsNotNull(view1);
            Assert.IsNotNull(view2);
        }

        //
        //
        [Test]
        public void t36_resolve_by_className_generic()
        {
            var imp = Context.ResolveFromClassName<IAbstract>("ImplClass");
            var imp2 = Context.ResolveFromClassName<IAbstract>("ImplClass2");
            var imp3 = Context.ResolveFromClassName<IAbstract>("ImplClass3");

            Assert.IsNotNull(imp);
            Assert.IsNotNull(imp2);
            Assert.IsNotNull(imp3);

            Assert.IsInstanceOf<ImplClass>(imp);
            Assert.IsInstanceOf<ImplClass2>(imp2);
            Assert.IsInstanceOf<ImplClass3>(imp3);
        }

        [Test]
        public void t37_resolve_by_className_general()
        {
            Context.GetDefaultInstance(typeof(IAbstract));

            var imp = Context.ResolveFromClassName("ImplClass");
            var imp2 = Context.ResolveFromClassName("ImplClass2");
            var imp3 = Context.ResolveFromClassName("ImplClass3");

            Assert.IsNotNull(imp);
            Assert.IsNotNull(imp2);
            Assert.IsNotNull(imp3);

            Assert.IsInstanceOf<ImplClass>(imp);
            Assert.IsInstanceOf<ImplClass2>(imp2);
            Assert.IsInstanceOf<ImplClass3>(imp3);
        }

        //
        //
        [Test]
        public void t38_Pool_General_Add()
        {
            //add a data
            Pool.Add(new PlayerData("John"));

            Assert.IsNotNull(Pool<PlayerData>.List);
            Assert.IsNotEmpty(Pool<PlayerData>.List);
            Assert.AreEqual(1, Pool<PlayerData>.List.Count);

            //make sure to clear the general pool before quiting your app
            Pool.Clear();
        }

        [Test]
        public void t39_Pool_General_clear()
        {
            Assert.AreEqual(0, Pool.Types.Count);

            Pool.Add(new PlayerData("John"));
            Assert.AreEqual(1, Pool<PlayerData>.List.Count);

            Assert.AreEqual(1, Pool.Types.Count);

            Pool.Clear();
            Assert.AreEqual(0, Pool<PlayerData>.List.Count);
        }
        [Test]
        public void t40_Pool_General_getList()
        {
            Pool<PlayerData>.AddItem(new PlayerData("John"));

            Context.Setting.UseSetForCollection = false;

            var list = Pool.GetList(typeof(PlayerData)) as List<PlayerData>;

            Assert.IsNotNull(list);
            Assert.AreEqual(1, list.Count);
        }
        [Test]
        public void t41_Pool_General_getList()
        {
            Context.Setting.UseSetForCollection = false;

            var list = Pool.GetList(typeof(PlayerData)) as List<PlayerData>;

            Pool<PlayerData>.AddItem(new PlayerData("John"));
            Pool<PlayerData>.AddItem(new PlayerData("Jean"));
            Pool<PlayerData>.AddItem(new PlayerData("Jan"));

            Assert.IsNotNull(list);
            Assert.AreEqual(3, list.Count);
        }

        IEnumerable<string> GetFriendNames()
        {
            yield return "Jane";
            yield return "John";
            yield return "Jim";
            yield return "David";
        }

        [Test]
        public void t99_dispose_instance()
        {
            //should be false as default
            Assert.IsFalse(Context.Initialized);

            //create
            Context.GetDefaultInstance(this);

            Assert.IsTrue(Context.Initialized);
            //dispose
            Context.Reset();
            //assert
            Assert.IsFalse(Context.Initialized);
        }
   
    }
}