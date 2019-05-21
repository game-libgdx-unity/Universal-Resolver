using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityIoC;

namespace SceneTest
{
    public class ImplClass : IAbstract
    {
        public int a;

        public ImplClass()
        {
        }

        public ImplClass(int a)
        {
            this.a = a;
        }

        public void ShowValue()
        {
            UniversalResolverDebug.Log("A: " + a);
        }

        public void DoSomething()
        {
            UniversalResolverDebug.Log("This is ImplClass");
        }
    }
}