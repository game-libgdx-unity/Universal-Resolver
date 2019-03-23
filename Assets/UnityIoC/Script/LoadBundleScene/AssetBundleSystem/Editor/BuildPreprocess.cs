using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Callbacks;
using UnityEngine;

public class BuildPreprocess : IPreprocessBuild
{
	public int callbackOrder { get { return 0; } }

	public void OnPreprocessBuild(BuildTarget target, string path)
	{

	}
}
