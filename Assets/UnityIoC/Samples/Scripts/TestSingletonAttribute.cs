using System.Collections;
using System.Collections.Generic;
using SceneTest;
using UnityEngine;
using UnityIoC;
using Debug = UnityEngine.Debug;

public class TestSingletonAttribute : MonoBehaviour
{
	[Singleton] private TestComponent testComp1;
	[Singleton] private TestComponent testComp2;
	[Singleton] private TestComponent testComp3;

	[Transient] private TestComponent2 testComp21;
	[Transient] private TestComponent2 testComp22;
	[Transient] private TestComponent2 testComp23;
	
	// Use this for initialization
	void Start ()
	{
		new AssemblyContext(this);
		
		Debug.Log("singleton component count: " + FindObjectsOfType<TestComponent>().Length);
		Debug.Log("transient component count: " + FindObjectsOfType<TestComponent2>().Length);
	}
}
