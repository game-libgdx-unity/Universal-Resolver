using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentTest : MonoBehaviour, TestInterface {
	public string JustAProperty { get; set; }

	public void DoSomething()
	{
		Debug.Log("This is ComponentTest");
	}
}
