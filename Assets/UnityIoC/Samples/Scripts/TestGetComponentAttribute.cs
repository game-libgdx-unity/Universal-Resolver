using System.Collections;
using System.Collections.Generic;
using SceneTest;
using UnityEngine;
using UnityIoC;

public class TestGetComponentAttribute : MonoBehaviour
{

	[GetComponent] private TestComponent[] testComponents;
	// Use this for initialization
	void Awake ()
	{
		 new AssemblyContext(this);
	}

	private void Start()
	{
		print("Number of test components: "+testComponents.Length);
	}
}
