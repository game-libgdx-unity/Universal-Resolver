using System.Collections;
using System.Collections.Generic;
using SceneTest;
using UnityEngine;
using UnityIoC;

public class TestComponentAttribute : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
		//create context with automatic load binding setting from the current assembly name
		//in this case, please refer to SceneTest setting from the resources folder.
		new Context(this);
	}

	//after creating AssemblyContext, it will resolve this component because you added [Component] attribute
	//you should see a log of this action in unity console
	[Component] private TestComponent testComponent;
}
