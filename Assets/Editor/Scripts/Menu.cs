#if UNITY_EDITOR

/**
 * Author:    Vinh Vu Thanh
 * This class is a part of Universal Resolver project that can be downloaded free at 
 * https://github.com/game-libgdx-unity/UnityEngine.IoC
 * (c) Copyright by MrThanhVinh168@gmail.com
 **/
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        [MenuItem("Tools/Force recompile %#R")]
        public static void ForceRebuild()
        {
//            string[] rebuildSymbols = {"RebuildToggle1", "RebuildToggle2"};
//            string definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(
//                EditorUserBuildSettings.selectedBuildTargetGroup);
//            if (definesString.Contains(rebuildSymbols[0]))
//            {
//                definesString = definesString.Replace(rebuildSymbols[0], rebuildSymbols[1]);
//            }
//            else if (definesString.Contains(rebuildSymbols[1]))
//            {
//                definesString = definesString.Replace(rebuildSymbols[1], rebuildSymbols[0]);
//            }
//            else
//            {
//                definesString += ";" + rebuildSymbols[0];
//            }
//
//            PlayerSettings.SetScriptingDefineSymbolsForGroup(
//                EditorUserBuildSettings.selectedBuildTargetGroup,
//                definesString);

//			EditorUtility.SetDirty(AssetDatabase.GetAssetPath ( EditorSceneManager.GetActiveScene());
//            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());

            var comps = FindObjectsOfType<MonoBehaviour>();
            foreach (var comp in comps)
            {
                if(comp)
                    Debug.Log("Found mono behaviour");
                
                var mono = comp.GetComponent<IRunScriptOnEditor>();
                if (mono != null)
                {
                    mono.DoAction();
                }
            }
        }

        public static void CreateAsset<T>() where T : ScriptableObject
        {
            T asset = ScriptableObject.CreateInstance<T>();

            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (path == "")
            {
                path = "Assets";
            }
            else if (Path.GetExtension(path) != "")
            {
                path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
            }

            string assetPathAndName =
                AssetDatabase.GenerateUniqueAssetPath(path + "/" + typeof(T).ToString() + ".asset");

            AssetDatabase.CreateAsset(asset, assetPathAndName);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
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