using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneTest
{
    [ProcessingOrder(2)]
    public class TestComponent3 : MonoBehaviour, IObjectResolvable, IObjectResolvable<IAbstract>
    {
        [Transient] public IAbstract @abstract;

        // Use this for initialization
        void Start()
        {
            @abstract.DoSomething();
        }

        public object GetObject(Type type)
        {
            if (type == typeof(IAbstract))
                return @abstract;

            return null;
        }

        public IAbstract GetObject()
        {
            return @abstract;
        }
    }
}