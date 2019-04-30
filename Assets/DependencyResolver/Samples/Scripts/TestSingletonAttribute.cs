using System.Collections;
using System.Collections.Generic;
using SceneTest;
using UnityEngine;
using UnityIoC;
using Debug = UnityEngine.Debug;

public class TestSingletonAttribute : MonoBehaviour
{
	
	void Start ()
	{
		//create a context with automatic load binding setting from current assembly name
		//in this case, please refer to SceneTest setting from the resources folder.
		new Context(this);
		
		//you can see the different between [Singleton] and [Transient] 
		
		// allow to create only 1 instance if using [Singleton] attribute
		Debug.Log("singleton component count: " + FindObjectsOfType<TestComponent>().Length);
		
		//a new object will be created if using [Transient] attribute
		Debug.Log("transient component count: " + FindObjectsOfType<TestComponent2>().Length);

		Context.DisposeDefaultInstance();
		Context.Resolve<ITestSceneRunner>(LifeCycle.Singleton).OpenNextTestScene();
	}
	
	
	/*
	 * These fields will be resolved by assemblyContext
	 * since they have [Singleton] or [Transient] marked
	 */
	
	[Singleton] private TestComponent testComp1;
	[Singleton] private TestComponent testComp2;
	[Singleton] private TestComponent testComp3;

	[Transient] private TestComponent2 testComp21;
	[Transient] private TestComponent2 testComp22;
	[Transient] private TestComponent2 testComp23;

}
