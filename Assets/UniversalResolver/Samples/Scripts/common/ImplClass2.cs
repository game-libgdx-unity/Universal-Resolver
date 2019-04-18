using UnityEngine;
using UnityIoC;

namespace SceneTest
{
    public class ImplClass2 : IAbstract
    {

        public void DoSomething()
        {
            MyDebug.Log("This is ImplClass2");
        }
    }
}