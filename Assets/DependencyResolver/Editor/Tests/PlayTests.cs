//using System.Collections;
//using NUnit.Framework;
//using UnityEngine;
//using UnityEngine.SceneManagement;
//using UnityEngine.TestTools;
//using UnityIoC;
//
//[TestFixture]
//public class PlayTests
//{
//    [SetUp]
//    public void Setup()
//    {
//        SceneManager.CreateScene("Test");
//    }
//
////    [UnityTest]
////    public IEnumerator _UnityPlayModeTestRunner()
////    {
////        yield return new MonoBehaviourTest<TestSceneRunner>();
////    }
////    
//    
//    [Test]
//    public void context_resolve_before_start_call()
//    {
//        Context.Resolve<TestComponent2>();
//        Context.Resolve<TestComponent2>(LifeCycle.Transient);
//        Context.Resolve<TestComponent2>(LifeCycle.Prefab);
//    }
//
////    [Test]
////    public void DoPass()
////    {
////        new GameObject().AddComponent<DoPassComp>();
////    }
////
////    [Test]
////    public void DoFail()
////    {
////        new GameObject().AddComponent<DoFailComp>();
////    }
//    
//    void print(string str)
//    {
//        Debug.Log(str);
//    }
//
//}