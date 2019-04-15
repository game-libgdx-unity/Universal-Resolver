using System.Collections;
using System.Collections.Generic;
using SceneTest;
using UnityEngine;
using UnityIoC;

public class TestOverrideDefaultBindingSetting : MonoBehaviour {

	// Use this for initialization
	void Start () {

		//create a context with automatic load binding setting from current assembly name
		//in this case, please refer to SceneTest setting from the resources folder.
		var assemblyContext = new Context(GetType());
			
		//try to resolve the object by setting in default setting of this SceneTest assembly
		var test1 = assemblyContext.ResolveObject<AbstractClass>();
		//will display a message on console
		test1.DoSomething(); 
		
		//override default setting by a custom one loaded from resources folder
		assemblyContext.LoadBindingSetting(MyResources.Load<InjectIntoBindingSetting>("custom_setting"));
		
		//try to resolve the object by the new setting which this assemblyContext just loaded.
		var test2 = assemblyContext.ResolveObject<AbstractClass>();
		
		//will display another message on console		
		test2.DoSomething();
	}
}
