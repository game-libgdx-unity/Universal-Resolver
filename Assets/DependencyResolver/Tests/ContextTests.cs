using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using NUnit.Framework;
using SceneTest;
using UnityEditor.SceneManagement;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

namespace UnityIoC.Editor
{
    public class AssemblyContextTests : TestBase
    {
        [SetUp]
        public void Setup()
        {
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
            Context.DisposeDefaultInstance();
        }
        
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
            Assert.IsInstanceOf<TestClass>(obj);
        }


        [Test]
        public void t3_bind_types()
        {
            //TestClass implements the TestInterface
            Context.Bind<TestInterface, TestClass>();
            var obj = Context.Resolve<TestInterface>();
            Assert.IsNotNull(obj);
            Assert.IsInstanceOf<TestClass>(obj);
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
            var obj = Context.Resolve<Impl>(1);
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
        public void t15_Get_From_Pools()
        {
            var gameObjects = new List<TestComponent>();

            var obj = gameObjects.GetInstanceFromPool();
            var obj2 = gameObjects.GetInstanceFromPool();
            var obj3 = gameObjects.GetInstanceFromPool();

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
            Assert.IsInstanceOf<TestComponent>(obj);
        }

        [Test]
        public void t18_get_from_pools()
        {
            var testComponentPool = Context.GetFromPool<TestComponent>();
            var testComponent2Pool = Context.GetFromPool<TestComponent2>();

            var obj = testComponentPool.GetInstanceFromPool();
            var obj5 = testComponent2Pool.GetInstanceFromPool();
            var obj2 = testComponentPool.GetInstanceFromPool();
            var obj6 = testComponent2Pool.GetInstanceFromPool();
            var obj3 = testComponentPool.GetInstanceFromPool();
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
        public void t23_test_inject_attributes_create_child()
        {
            //resolve a component
            var comp = Context.Resolve<InjectChildComponent>();

            //JustUnityComponent using [Inject] attribute which support caches, so 
            Assert.IsNotNull(comp.componentInChild);
            Assert.AreEqual(comp.transform.childCount, 1);
            Assert.IsNotNull(comp.transform.GetChild(0));
            Assert.IsNotNull(comp.transform.GetChild(0).GetComponent<TestComponent>());
            Assert.AreSame(comp.componentInChild, comp.transform.GetChild(0).GetComponent<TestComponent>());
        }

        [Test]
        public void t24_test_get_component()
        {
            //create game obj
            GameObject gameObject = new GameObject();
            gameObject.GetOrAddComponent<TestComponent>();

            //try to get its component
            var comp = Context.ResolveFromGameObject(typeof(IComponentAbstract), gameObject, null);
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
            var comp = Context.ResolveFromGameObject<IComponentAbstract>(gameObject, null);
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
            var comp = Context.ResolveFromGameObject<IComponentAbstract>(gameObject, null);
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
                msg =>
                {
                    Assert.AreEqual("OK", msg);
                },
                error =>
                {
                    Assert.Fail("Error: " + error);
                });
        }
        
        [UnityTest]
        public IEnumerator t28_test_get_from_api()
        {
            //test a random api searched from the internet
            string link = "https://jsonplaceholder.typicode.com/todos/1";
            yield return Context.GetObjects<UserData>(link, null, msg => { Assert.Fail("error: " + msg); });
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
        public void t31_get_update_delete()
        {
            foreach (var friendName in GetFriendNames())
            {
                Context.Resolve<PlayerData>(friendName);
            }

            // check number of cached object
            var objCount = Context.ResolvedObjects[typeof(PlayerData)].Count;
            Assert.IsTrue(GetFriendNames().Count() == objCount);

            //update by filter
            Context.Update<PlayerData>(
                p => p.name == "John",
                data => data.name = "Vinh" //update John to Vinh from cache
            );

            var vinh = Context.GetObject<PlayerData>(p => p.name == "Vinh");
            Assert.AreEqual(vinh.name, "Vinh");
            
            //delete by filter
            Context.Delete<PlayerData>(
                p => p.name == "Jim"
            );

            //update by ref
            var jane = Context.GetObject<PlayerData>(p => p.name == "Jane");
            Context.Update(jane, p => p.name = "Nguyen");

            //update by filter but this object has been deleted already
            Context.Update<PlayerData>(
                p => p.name == "Jim",
                data => data.name = "Hm... not found" //should have no action
            );

            //now check number of cached object
            objCount = Context.ResolvedObjects[typeof(PlayerData)].Count;
            Assert.IsTrue(objCount == 3);

            //continue trying to create another player
            var jimmy = Context.Resolve<PlayerData>("Jimmy");

            //update by ref
            Context.Update(jimmy, p => p.name = "Dung");

            //try to delete david
            Context.Delete<PlayerData>(p => p.name == "David");

            Assert.IsTrue(jimmy.name == "Dung");
            Assert.IsTrue(jane.name == "Nguyen");
            
            Context.DeleteAll<PlayerData>();
            //now check number of cached object
            objCount = Context.ResolvedObjects[typeof(PlayerData)].Count;
            Assert.IsTrue(objCount == 0);
            
        }
        
        [Test]
        public void t32_resolve_scriptable_object()
        {
             var t4 =  Context.Resolve<TestComponent4>();
            Assert.IsNotNull(t4.ScriptableObject);
            Assert.AreEqual(100, t4.ScriptableObject.Amount);
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
            Context.DisposeDefaultInstance();
//assert
            Assert.IsFalse(Context.Initialized);
        }
    }
    
    
    [Serializable]
    public class PlayerData
    {
        public string name;
        public PlayerData(string name)
        {
            this.name = name;
        }
    }
}