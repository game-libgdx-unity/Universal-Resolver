#if  UNITY_EDITOR

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
	public class MenuTest : MonoBehaviour
	{
		static string lastScenePathKey = "lastScenePath";

		[MenuItem("Tools/Open things %`")]
		static void DoSomething()
		{
			EditorApplication.ExecuteMenuItem("Assets/Open");
		}
		
		public static void CreateAsset<T> () where T : ScriptableObject
		{
			T asset = ScriptableObject.CreateInstance<T> ();
 
			string path = AssetDatabase.GetAssetPath (Selection.activeObject);
			if (path == "") 
			{
				path = "Assets";
			} 
			else if (Path.GetExtension (path) != "") 
			{
				path = path.Replace (Path.GetFileName (AssetDatabase.GetAssetPath (Selection.activeObject)), "");
			}
 
			string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath (path + "/" + typeof(T).ToString() + ".asset");
 
			AssetDatabase.CreateAsset (asset, assetPathAndName);
 
			AssetDatabase.SaveAssets ();
			AssetDatabase.Refresh();
			EditorUtility.FocusProjectWindow ();
			Selection.activeObject = asset;
		}
		
//		[MenuItem("Tools/UpLoad Assetbundles")]
//		static void UploadBundles()
//		{
//			if (EditorApplication.isPlaying)
//			{
//				//restore last opened scene
//				EditorApplication.isPlaying = false;
//				var lastPath = PlayerPrefs.GetString(lastScenePathKey, "AssetBundleUploader");
//				print("Restore scene: " + lastPath);
//				EditorApplication.playModeStateChanged += p =>
//				{
//					if (p == PlayModeStateChange.EnteredEditMode)
//					{
//						EditorSceneManager.OpenScene(lastPath);
//
//					}
//				};
//			}
//			else
//			{
//				//save changes
//				EditorSceneManager.SaveOpenScenes();
//				
//				var scenePath = SceneManager.GetActiveScene().path;
//				if (!scenePath.Contains("AssetBundleUploader"))
//				{
//					PlayerPrefs.SetString(lastScenePathKey, scenePath);
//					PlayerPrefs.Save();
//					print("Last scene: " + scenePath);
//				}
//				//open scene to upload bundles
//				EditorSceneManager.OpenScene("Assets/App/Scenes/AssetBundleUploader.unity");
//				EditorApplication.isPlaying = true;
//			}
//		}
	}
}

#endif