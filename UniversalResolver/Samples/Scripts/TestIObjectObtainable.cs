using System.Collections;
using System.Collections.Generic;
using SceneTest;
using UnityEngine;
using UnityIoC;

public class TestIObjectObtainable : MonoBehaviour
{

	//This attribute do the work as "Add Or GetComponent" from this GameObject
	[Singleton("GameObject")] private AbstractClass abstractClass;
	// Use this for initialization
	void Awake ()
	{
		//create context which will automatically load binding setting by the assembly name
		//in this case, please refer to SceneTest setting from the resources folder.
		 new Context(this);
	}

	private void Start()
	{
		abstractClass.DoSomething();
	}
}
