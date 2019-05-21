using UnityEngine;
using UnityIoC;

namespace SceneTest
{
    public class ImplClass2 : IAbstract
    {

        public void DoSomething()
        {
            UniversalResolverDebug.Log("This is ImplClass2");
        }
    }
}