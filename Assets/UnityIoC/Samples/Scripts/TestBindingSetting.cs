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
			
			//create context with automatic binding is enable
			var assemblyContext = new AssemblyContext(GetType(), true);
			
			//try to resolve the object by setting in default setting of this SceneTest assembly
			var testI = assemblyContext.Resolve<AbstractClass>();
			testI.DoSomething();
		}
	}

}