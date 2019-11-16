using System.Collections;
using System.Collections.Generic;
//using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityIoC;

namespace SceneTest
{
    public class TestInjectIntoSetting : MonoBehaviour
    {
        
        //refer to SceneTest setting to see what kind of type, the field AbstractClass inside will be resolved 
        [Transient] TestComponent2 testComponent2;
        
        //refer to SceneTest setting to see what kind of type, the field AbstractClass inside will be resolved 
        [Transient] TestComponent testComponent;

        //refer to SceneTest setting to see what kind of type, the field AbstractClass inside will be resolved 
        [Transient] TestComponent3 testComponent3;
        
        //refer to SceneTest setting to see what kind of type, the field AbstractClass inside will be resolved 
        [Transient] TestComponent4 testComponent4;

        private void Awake()
        {
            //create context which will automatically load binding setting by the assembly name
            //in this case, please refer to SceneTest setting from the resources folder.
            new Context(this);
        }
    }
}