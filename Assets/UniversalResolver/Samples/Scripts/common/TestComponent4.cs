using System.Collections;
using System.Collections.Generic; 
using UnityEngine;

namespace SceneTest
{
	public class TestComponent4 : MonoBehaviour
	{
		[Transient] private AbstractClass abstractClass;

		// Use this for initialization
		void Start()
		{
			abstractClass.DoSomething();
		}
	}
}