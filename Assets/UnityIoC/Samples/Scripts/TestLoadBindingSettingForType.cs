using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityIoC;

namespace SceneTest
{
	public class TestLoadBindingSettingForType : MonoBehaviour
	{
		// Use this for initialization
		void Start()
		{
			var context = new AssemblyContext(typeof(TestInjectIntoSetting));
			
			context.LoadBindingSettingForType<TestComponent>();
			context.LoadBindingSettingForType<TestComponent2>();
			
			context.Resolve<TestComponent>();
			context.Resolve<TestComponent2>();
			context.Resolve<TestComponent>();
		}
	}
}