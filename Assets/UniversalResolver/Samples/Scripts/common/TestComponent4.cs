using System.Collections;
using System.Collections.Generic; 
using UnityEngine;

namespace SceneTest
{
	public class TestComponent4 : MonoBehaviour
	{
		[Transient] public IAbstract @abstract;

		// Use this for initialization
		void Start()
		{
			@abstract.DoSomething();
		}
	}
}