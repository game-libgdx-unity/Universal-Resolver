using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityIoC;

public class ContextBehaviour : MonoBehaviour
{
	public InjectIntoBindingSetting customSetting;
	void Awake ()
	{
		var context = Context.GetDefaultInstance(this);

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
