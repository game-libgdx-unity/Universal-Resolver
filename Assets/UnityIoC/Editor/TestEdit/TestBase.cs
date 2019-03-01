using System.Reflection;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using UnityEngine;

namespace UnityIoC.Editor
{
    public class TestBase
    {
        private bool stop;

        [SetUp]
        public void SetUp()
        {
            if (stop)
            {
                Assert.Inconclusive("Previous test failed");
            }

            Debug.ClearDeveloperConsole();

            //reflection api will call a method from unity editor object
            //ref: https://github.com/Unity-Technologies/UnityCsReference/blob/master/Editor/Mono/LogEntries.bindings.cs
            var assembly = Assembly.GetAssembly(typeof(UnityEditor.ActiveEditorTracker));
            var type = assembly.GetType("UnityEditor.LogEntries");
            if (type == null)
            {
                Debug.Log("The class not found, you should check the link above to remake the reflection code");
                return;
            }

            var method = type.GetMethod("Clear");
            method.Invoke(new object(), null);
        }

        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {
                stop = true;
            }
        }
    }
}