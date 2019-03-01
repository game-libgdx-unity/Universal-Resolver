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
			var objectContext = new ObjectContext(this);
			var obj = objectContext.Resolve<AbstractClass>();
			obj.DoSomething();
		}
	}
}