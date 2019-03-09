using System.Collections;
using System.Collections.Generic;
using SceneTest;
using UnityEngine;
using UnityIoC;

public class TestGetComponentAttribute : MonoBehaviour
{

	//This attribute do the work as "Add Or GetComponent" from this GameObject
	[GetComponent] private TestComponent[] testComponents;
	// Use this for initialization
	void Awake ()
	{
		//create context which will automatically load binding setting by the assembly name
		//in this case, please refer to SceneTest setting from the resources folder.
		 new AssemblyContext(this);
	}

	private void Start()
	{
		print("Number of test components: "+testComponents.Length);
	}
}
