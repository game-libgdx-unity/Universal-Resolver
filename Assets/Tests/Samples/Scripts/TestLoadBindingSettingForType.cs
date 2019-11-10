using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityIoC;

namespace SceneTest
{
	public class TestLoadBindingSettingForType : MonoBehaviour
	{
		void Start()
		{
			//create a context with automatic load binding setting from current assembly name
			//in this case, please refer to SceneTest setting from the resources folder.
			var context = new Context(this);
			
			//load a custom binding setting from this TestComponent
			//in this case, please refer to SceneTest setting from the resources folder.
			context.LoadBindingSettingForType<TestComponent>();
			
			
			//load a custom binding setting from this TestComponent2
			//in this case, please refer to SceneTest setting from the resources folder.
			context.LoadBindingSettingForType<TestComponent2>();
			
			//try to resolve each component
			
			//you should see a log in console
			context.ResolveObject<TestComponent>();
			
			//you should see another log in console
			context.ResolveObject<TestComponent2>();
			
			context.ResolveObject<TestComponent>();
		}
	}
}