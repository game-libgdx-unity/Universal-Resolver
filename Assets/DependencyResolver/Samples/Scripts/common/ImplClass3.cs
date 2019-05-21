using UnityEngine;
using UnityIoC;

namespace SceneTest
{
    public class ImplClass3 : IAbstract
    {

        public void DoSomething()
        {
            UniversalResolverDebug.Log("This is ImplClass3");
        }
    }
}