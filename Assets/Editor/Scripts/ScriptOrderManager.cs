/**
 * Author:    Vinh Vu Thanh
 * This class is a part of Universal Resolver project that can be downloaded free at 
 * https://github.com/game-libgdx-unity/UnityEngine.IoC
 * (c) Copyright by MrThanhVinh168@gmail.com
 **/
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
 
[InitializeOnLoad]
public class ScriptOrderManager {
 
	static ScriptOrderManager() {
		foreach (MonoScript monoScript in MonoImporter.GetAllRuntimeMonoScripts()) {
			if (monoScript.GetClass() != null) {
				foreach (var a in Attribute.GetCustomAttributes(monoScript.GetClass(), typeof(ScriptOrder))) {
					var currentOrder = MonoImporter.GetExecutionOrder(monoScript);
					var newOrder = ((ScriptOrder)a).order;
					if (currentOrder != newOrder)
						MonoImporter.SetExecutionOrder(monoScript, newOrder);
				}
			}
		}
	}
}