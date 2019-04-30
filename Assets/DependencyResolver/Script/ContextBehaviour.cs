using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityIoC;

public class ContextBehaviour : MonoBehaviour
{
	public bool enableLogging = false;
	public InjectIntoBindingSetting customSetting;
	void Awake ()
	{
		MyDebug.EnableLogging = enableLogging;
		
		var context = Context.GetDefaultInstance();

		if (customSetting != null)
		{
			context.LoadBindingSetting(customSetting);
		}
	}

	private void OnDestroy()
	{
		Context.DisposeDefaultInstance();
	}
}
