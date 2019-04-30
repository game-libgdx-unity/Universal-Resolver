using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

[TestFixture]
public class PlayTests
{
    [SetUp]
    public void Setup()
    {
//        SceneManager.CreateScene("Test");
    }

    [UnityTest]
    public IEnumerator _UnityPlayModeTestRunner()
    {
        yield return new MonoBehaviourTest<TestSceneRunner>();
    }

    [Test]
    public void DoPass()
    {
        new GameObject().AddComponent<DoPassComp>();
    }

    [Test]
    public void DoFail()
    {
        new GameObject().AddComponent<DoFailComp>();
    }
    
    void print(string str)
    {
        Debug.Log(str);
    }

}