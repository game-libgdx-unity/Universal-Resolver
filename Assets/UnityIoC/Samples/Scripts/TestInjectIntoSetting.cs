using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityIoC;

namespace SceneTest
{
	public class TestInjectIntoSetting : MonoBehaviour {

		//refer to SceneTest setting to see what kind of type, the field AbstractClass inside will be resolved 
		[Transient] TestComponent2 testComponent2;
		
		//refer to SceneTest setting to see what kind of type, the field AbstractClass inside will be resolved 
		[Transient] TestComponent testComponent;

		private void Start()
		{
			//create context with automatic load binding setting from assembly name
			//in this case, please refer to SceneTest setting from the resources folder.
			new AssemblyContext(this);
		}
	}
}