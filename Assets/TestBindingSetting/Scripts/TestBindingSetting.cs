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

		private Context context;
		// Use this for initialization
		void Start () {
			context = new Context(GetType());
			context.LoadDefaultBindingSetting();
			
			var testI = context.Resolve<AbstractClass>(LifeCycle.Transient);
			testI.DoSomething();
		}
	}

}