using System.Collections;
using System.Collections.Generic;
using SceneTest;
using UnityEngine;

namespace SceneTest
{
    public class TestComponent : MonoBehaviour, IComponentAbstract
    {
        [SerializeField] public int Afield;

        [Transient] public IAbstract @abstract;

        // Use this for initialization
        void Start()
        {
            if (@abstract != null)
            {
                @abstract.DoSomething();
            }
            else
            {
                Debug.Log("Abstract class fails to resolve");
            }
        }

        public void DoSomething()
        {
            Debug.Log("Caller from TestComponent as an IComponentAbstract interface");
        }
    }
}