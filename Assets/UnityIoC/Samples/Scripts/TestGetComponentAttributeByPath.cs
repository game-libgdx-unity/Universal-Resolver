using System.Collections;
using System.Collections.Generic;
using SceneTest;
using UnityEngine;
using UnityIoC;

public class TestGetComponentAttributeByPath : MonoBehaviour
{

	//This attribute do the work as "Add Or GetComponent" from this GameObject
	[Component("child/child")] private TestComponent[] testComponents;
	[Component] private TestComponent internalTestComponent;	
	
	[Component("child/child")] TestComponent testComponent;
	[Transient("child/child")] private TestComponent aTestComponen;
	[Singleton] private TestComponent sTestComponen;
	
	// Use this for initialization
	void Awake ()
	{
		//create context which will automatically load binding setting by the assembly name
		//in this case, please refer to SceneTest setting from the resources folder.
		 new AssemblyContext(this);

		 print("internalTestComponent null: "+ (internalTestComponent==null));
		internalTestComponent.abstractClass.DoSomething();
	}

	private void Start()
	{
		print("Number of test components: "+testComponents.Length);
		print("Afield " + testComponent.Afield);
	}
}
