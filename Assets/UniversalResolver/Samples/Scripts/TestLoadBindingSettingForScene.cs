using System.Collections;
using System.Collections.Generic;
using SceneTest;
using UnityEngine;
using UnityIoC;

public class TestLoadBindingSettingForScene : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
		//create context which will automatically load binding setting by the assembly name
		//in this case, please refer to SceneTest setting from the resources folder.
		var context = new Context(this);
		//you should see a log in unity console
		context.ResolveObject<TestComponent>();
		
		//This method will load a setting with a sceneName, overriden the current setting
		//in this case, please refer to 3. TestLoadBindingSettingForScene setting from the resources folder.
		context.LoadBindingSettingForScene();
		//you should see another log in unity console
		context.ResolveObject<TestComponent>();
	}
}
