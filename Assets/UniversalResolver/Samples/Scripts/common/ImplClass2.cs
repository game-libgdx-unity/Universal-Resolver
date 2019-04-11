using UnityEngine;
using UnityIoC;

namespace SceneTest
{
    public class ImplClass2 : AbstractClass
    {

        public void DoSomething()
        {
            MyDebug.Log("This is ImplClass2");
        }
    }
}