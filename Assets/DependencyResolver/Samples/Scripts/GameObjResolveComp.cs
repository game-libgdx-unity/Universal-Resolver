using System.Collections;
using System.Collections.Generic;
//using NUnit.Framework;
using UnityEngine;
using UnityIoC;
using System.Collections;
using System.Collections.Generic;
//using NUnit.Framework;
using UnityEngine.UI;

namespace SceneTest
{
    public class GameObjResolveComp : MonoBehaviour
    {

        void Start()
        {
            gameObject.ResolveComponent<TestComponent>();
        }
    }
}