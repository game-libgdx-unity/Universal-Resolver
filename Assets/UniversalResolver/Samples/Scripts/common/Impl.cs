using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityIoC;

namespace SceneTest
{
    public class Impl : IAbstract
    {
        public int a;

        public Impl()
        {
        }

        public Impl(int a)
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