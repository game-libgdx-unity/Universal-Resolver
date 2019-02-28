using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityIoC;

namespace SceneTest
{
	public class TestInjectIntoSetting : MonoBehaviour {

		[Transient] TestComponent2 testComponent2;
		[Transient] TestComponent testComponent;

		private void Start()
		{
			new AssemblyContext(this);
		}
	}
}