using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityIoC;

namespace SceneTest
{
    public class ImplClass : AbstractClass
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
            MyDebug.Log("A: " + a);
        }

        public void DoSomething()
        {
            MyDebug.Log("This is ImplClass");
        }
    }
}