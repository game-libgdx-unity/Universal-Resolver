using System.Collections;
using System.Collections.Generic;
//using NUnit.Framework;
using UnityEngine;
using UnityIoC;
using System.Collections;
using System.Collections.Generic;
//using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;
using UnityIoC;

namespace SceneTest
{
    public class TestBindingSetting : MonoBehaviour
    {
        [Component("/Canvas/Text")] private Text text;
        [Component("/Canvas/Text/Text2")] private Text text2;
        [Component("Canvas/Text/Text2")] private Text text4;
        [Transient] private Text text3;

        void Start()
        {
            //create context which will automatically load binding setting by the assembly name
            //in this case, please refer to SceneTest setting from the resources folder.
            var assemblyContext = new Context(GetType());

            //try to resolve the object by the default settings of this SceneTest assembly
            var obj = assemblyContext.ResolveObject<IAbstract>(LifeCycle.Singleton);
            var obj2 = assemblyContext.ResolveObject<IAbstract>(LifeCycle.Singleton);
            var obj3 = assemblyContext.ResolveObject<IAbstract>(LifeCycle.Transient);

            //you should see a log of this action in unity console
            obj.DoSomething();

            //check ReferenceEquals, should be true
            Debug.Log("OKKKKK: " + ReferenceEquals(obj, obj2));
            Debug.Log("OKKKKK: " + !ReferenceEquals(obj3, obj2));

            text.text = "Hello world";
            text2.text = "Hello world";
            text3.text = "Hello world";
            text4.text = "Hello world";

//            GameObject.Find("MonoBehaviourTest: TestRunner").GetComponent<ITestScene>().OpenNextTestScene();

            Context.DisposeDefaultInstance();
            Context.Resolve<ITestSceneRunner>(LifeCycle.Singleton).OpenNextTestScene();
        }
    }
}