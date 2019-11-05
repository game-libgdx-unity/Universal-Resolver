using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityIoC.Editor
{
	[Binding]
	public class TestClass : TestInterface, IBindingCondition {
		public string JustAProperty { get; set; }

		private TestStruct testStruct;
		
		public void DoSomething()
		{
			UniversalResolverDebug.Log(testStruct.aField.ToString());
			UniversalResolverDebug.Log("TestClass"); 
		}

		public bool ShouldResolveByThisImplement(Context.ResolveInput input)
		{
			return true;
		}
	}

	public struct TestStruct
	{
		public int aField;
	}
}