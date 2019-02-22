using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityIoC;

namespace SceneTest
{
	public class TestInjectIntoSetting : MonoBehaviour {

		[Transient] TestComponent testComponent;
		
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		static void Init()
		{
			if (SceneManager.GetActiveScene().name == "TestInjectIntoSetting")
			{
				UnityIoC.Debug.EnableLogging = true;
				var context = new Context(typeof(TestInjectIntoSetting));
			}
		}
	}
}