using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class DoPassComp : MonoBehaviour
{
    private void Start()
    {
        UnityEngine.Assertions.Assert.IsTrue(true);
    }
}

public class DoFailComp : MonoBehaviour
{
    private void Start()
    {
        UnityEngine.Assertions.Assert.IsTrue(false);
//        TestContext.Error.WriteLine("Error");


//        TestContext.CurrentContext.Test.S
        Assert.Inconclusive("This test failed");
    }
}

public class TestComp : MonoBehaviour, IMonoBehaviourTest
{
    private void Start()
    {
    }

    public bool IsTestFinished
    {
        get { return true; }
    }
}