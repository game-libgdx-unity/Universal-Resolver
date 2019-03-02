using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityIoC;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityIoC;

namespace SceneTest
{
	public class TestBindingSetting : MonoBehaviour {
		// Use this for initialization
		void Start () {
			
			//create context with automatic load binding setting from assembly name
			//in this case, please refer to SceneTest setting from the resources folder.
			var assemblyContext = new AssemblyContext(GetType());
			
			//try to resolve the object by the default settings of this SceneTest assembly
			var obj = assemblyContext.Resolve<AbstractClass>();
			
			//you should see a log of this action in unity console
			obj.DoSomething();
		}
	}

}