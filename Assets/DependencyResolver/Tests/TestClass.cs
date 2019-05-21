using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityIoC.Editor
{
	
	public class TestClass : TestInterface {
		public string JustAProperty { get; set; }

		private TestStruct testStruct;
		
		public void DoSomething()
		{
			UniversalResolverDebug.Log(testStruct.aField.ToString());
			UniversalResolverDebug.Log("TestClass"); 
		}
	}

	public struct TestStruct
	{
		public int aField;
	}
}