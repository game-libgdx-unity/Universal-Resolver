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
		[Transient] TestComponent2 testComponent2;

		private void Start()
		{
			new AssemblyContext(this);
		}
	}
}