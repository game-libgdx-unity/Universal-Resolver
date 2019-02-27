using System.Collections;
using System.Collections.Generic;
using SceneTest;
using UnityEngine;
using UnityIoC;

public class TestLoadBindingSettingForScene : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
		var context = new AssemblyContext(this);
		context.LoadBindingSettingForScene();
		context.Resolve<TestComponent>();
	}
}
