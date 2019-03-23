using System.Collections;
using System.Collections.Generic;
using SceneTest;
using UnityEngine;

namespace SceneTest
{
    public class TestComponent : MonoBehaviour
    {
        [SerializeField] public int Afield;

        [Transient] public AbstractClass abstractClass;

        // Use this for initialization
        void Start()
        {
            if (abstractClass != null)
            {
                abstractClass.DoSomething();
            }
            else
            {
                Debug.Log("Abstract class fails to resolve");
            }
        }
    }
}