using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneTest
{
    public class TestComponent2 : MonoBehaviour, IObjectResolvable
    {
        [Transient] public IAbstract @abstract;

        // Use this for initialization
        void Start()
        {
            @abstract.DoSomething();
            
            print("TestComponent2 Start()");
        }

        public object GetObject(Type type)
        {
            if (type == typeof(IAbstract))
                return @abstract;

            return null;
        }
    }
}