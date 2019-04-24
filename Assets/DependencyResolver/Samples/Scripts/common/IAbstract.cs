using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneTest
{
    public interface IAbstract
    {
        void DoSomething();
    }

    public class JustDTOClass
    {
        public int justAField = 1;
    }
    public class JustUnityComponent : MonoBehaviour
    {
        [Inject] public JustDTOClass justDTOClass;
        [Inject("Child")] public TestComponent componentInChild;
    }
}