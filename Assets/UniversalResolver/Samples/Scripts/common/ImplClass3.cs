using UnityEngine;
using UnityIoC;

namespace SceneTest
{
    public class ImplClass3 : IAbstract
    {

        public void DoSomething()
        {
            MyDebug.Log("This is ImplClass3");
        }
    }
}