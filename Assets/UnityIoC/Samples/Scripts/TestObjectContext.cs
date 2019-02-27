using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityIoC;

namespace SceneTest
{
	public class TestObjectContext : MonoBehaviour {
		
		private void Start()
		{
			var context = new ObjectContext(this);
			var obj = context.Resolve<AbstractClass>();
			obj.DoSomething();
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		static void Init()
		{
			if (SceneManager.GetActiveScene().name == "6. TestObjectContext")
			{
				UnityIoC.Debug.EnableLogging = true;
				AssemblyContext.GetDefaultInstance(typeof(TestObjectContext));
			}
		}
	}
}