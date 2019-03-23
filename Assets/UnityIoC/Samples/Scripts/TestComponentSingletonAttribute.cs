using System.Collections;
using System.Collections.Generic;
using SceneTest;
using UnityEngine;
using UnityIoC;
using Debug = UnityEngine.Debug;

public class TestComponentSingletonAttribute : MonoBehaviour
{
	
	// Use this for initialization
	void Start ()
	{
		//create context which will automatically load binding setting by the assembly name
		//in this case, please refer to SceneTest setting from the resources folder.
		var assemblyContext = new AssemblyContext(this);
		
		//resolve this testComponent as singleton
		//TestComponent has been resolved by [ComponentSingleton] Attribute as singleton below, so 
		//this is just a reference to it, this is pointing to the same object.
		var testComp = assemblyContext.Resolve<TestComponent>(LifeCycle.Singleton);
		
		//to verify, you should see only 1 instance for this type
		print("Number of TestComponent instances: "+FindObjectsOfType<TestComponent>().Length);
	}
	
	//after creating AssemblyContext, it will resolve this component because you added [Component] attribute
	//you should see a log of this action in unity console
	//this component will be resolved as singleton and attached to current gameObject
	[SingletonComponent] private TestComponent2 testComp2;
	
	//after creating AssemblyContext, it will resolve this component because you added [Component] attribute
	//you should see a log of this action in unity console
	[SingletonComponent] private TestComponent testComp;
}
