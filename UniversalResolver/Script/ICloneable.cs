using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityIoC
{
	public interface ICloneable<T>
	{
		 T Clone();
	}
}