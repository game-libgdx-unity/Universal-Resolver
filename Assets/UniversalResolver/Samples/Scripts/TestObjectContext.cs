using System.Collections;
using System.Collections.Generic;
//using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityIoC;

namespace SceneTest
{
	public class TestObjectContext : MonoBehaviour {
		
		private void Start()
		{
			//create a context with automatic load binding setting from current assembly name
			//in this case, please refer to SceneTest setting from the resources folder.
			var assemblyContext = Context.GetDefaultInstance(this);
			
			//resolve the abstract type using default binging setting of this assembly.
			assemblyContext.ResolveObject<AbstractClass>();
			
			//create a context with automatic load binding setting for the current class, TestObjectContext
			//in this case, please refer to TestObjectContext setting from the resources folder.
			var objectContext = new ObjectContext(this);
			
			//using this objectContext to resolve things.
			//the output should be different to default assemblyContext's.
			//due to different binding settings loaded.
			var obj = objectContext.Resolve<AbstractClass>();
			obj.DoSomething();
		}
	}
}