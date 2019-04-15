using System.Collections;
using System.Collections.Generic;
//using NUnit.Framework;
using UnityEngine;
using UnityIoC;
using System.Collections;
using System.Collections.Generic;
//using NUnit.Framework;
using UnityEngine;
using UnityIoC;

namespace SceneTest
{
	public class TestSceneNoLog : MonoBehaviour {

		private Context context;
		// Use this for initialization
		void Start () {
			
			//disable context logs to console
			UnityIoC.MyDebug.EnableLogging = false;
			
			
			//create a context with automatic load binding setting from current assembly name
			//in this case, please refer to SceneTest setting from the resources folder.
			context = new Context(GetType());
			
			//try to resolve the object by setting in default setting of this SceneTest assembly
			//just no logs in console.
			var testI = context.ResolveObject<AbstractClass>();
			testI.DoSomething();
		}
	}

}