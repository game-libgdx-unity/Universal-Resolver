using System.Collections;
using System.Collections.Generic;
using SceneTest;
using UnityEngine;
using UnityIoC;

public class TestComponentAttribute : MonoBehaviour
{

	[Component] private TestComponent testComponent;
	// Use this for initialization
	void Start ()
	{
		new AssemblyContext(this);
	}
}
