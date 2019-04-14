using UnityEngine;
using UnityIoC;

namespace SceneTest
{
    public class ImplClass3 : AbstractClass
    {

        public void DoSomething()
        {
            MyDebug.Log("This is ImplClass3");
        }
    }
}