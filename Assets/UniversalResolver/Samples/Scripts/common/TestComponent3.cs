﻿using System.Collections;
using System.Collections.Generic; 
using UnityEngine;

namespace SceneTest
{
	public class TestComponent3 : MonoBehaviour
	{
		[Transient] private IAbstract @abstract;

		// Use this for initialization
		void Start()
		{
			@abstract.DoSomething();
		}
	}
}