using System.Collections;
using System.Collections.Generic;
using SceneTest;
using UnityEngine;
using UnityIoC;
using Debug = UnityEngine.Debug;

public class TestComponentSingletonAttribute : MonoBehaviour
{
	[ComponentSingleton] private TestComponent testComp;
	
	// Use this for initialization
	void Start ()
	{
		var assemblyContext = new AssemblyContext(this);
		//create a reference to this testComponent as singleton
		var testComp = assemblyContext.Resolve<TestComponent>(LifeCycle.Singleton);
		
		print("Number of TestComponent instances: "+FindObjectsOfType<TestComponent>().Length);
	}
}
