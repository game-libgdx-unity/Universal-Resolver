using System.Collections;
using System.Collections.Generic;
using SceneTest;
using UnityEngine;
using UnityIoC;
using Resources = UnityIoC.Resources;

public class TestOverrideDefaultBindingSetting : MonoBehaviour {

	// Use this for initialization
	void Start () {
		//create context with automatic binding is enable
		var assemblyContext = new AssemblyContext(GetType());
			
		//try to resolve the object by setting in default setting of this SceneTest assembly
		var test1 = assemblyContext.Resolve<AbstractClass>();
		test1.DoSomething(); 
		
		//override default setting by a custom one loaded from resources folder
		assemblyContext.LoadBindingSetting(Resources.Load<InjectIntoBindingSetting>("custom_setting"));
		
		//try to resolve the object by setting in default setting of this SceneTest assembly
		var test2 = assemblyContext.Resolve<AbstractClass>();
		test2.DoSomething();
	}
}
