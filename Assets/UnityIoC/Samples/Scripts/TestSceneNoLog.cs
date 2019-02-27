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
	public class TestSceneNoLog : MonoBehaviour {

		private AssemblyContext assemblyContext;
		// Use this for initialization
		void Start () {
			
			//disable context logs to console
			UnityIoC.Debug.EnableLogging = false;
			
			//create context with automatic binding is enable
			assemblyContext = new AssemblyContext(GetType());
			
			//try to resolve the object by setting in default setting of this SceneTest assembly
			var testI = assemblyContext.Resolve<AbstractClass>();
			testI.DoSomething();
		}
	}

}