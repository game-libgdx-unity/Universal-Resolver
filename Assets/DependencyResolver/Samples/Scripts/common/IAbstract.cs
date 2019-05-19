using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityIoC;

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

    public class InjectChildComponent : MonoBehaviour
    {
        [Inject("Child")] public TestComponent componentInChild;
    }

    [Serializable]
    public class UserData : IViewBinding<UserDataView, UserDataView2>
    {
        public int userId;
        public int id;
        public string title;
        public bool completed;
    }
}