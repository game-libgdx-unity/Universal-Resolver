using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityIoC;

// ReSharper disable All

public class TestSceneRunner : SingletonBehaviour<TestSceneRunner>, IMonoBehaviourTest, ITestSceneRunner
{
    public const string TestConfigName = "TestConfiguration";
    private TestConfiguration testConfig;
    private int currentSceneIndex;
    private bool _isFinished;
    private List<EditorBuildSettingsScene> scenes;
    private List<EditorBuildSettingsScene> bk_scenes;

    /// <summary>
    /// Run this method to start tests
    /// </summary>
    public static void AddScenesToBuildAndRunTest()
    {
        Debug.Log("AddScenesToBuildAndRunTest");
        
        List<EditorBuildSettingsScene> scenes = new List<EditorBuildSettingsScene>();
        var testConfiguration = Resources.Load<TestConfiguration>(TestConfigName);
        
        //load from folder path
        if (!string.IsNullOrEmpty(testConfiguration.sceneFolderPath))
        {
            var path = Application.dataPath + "/" + testConfiguration.sceneFolderPath;
            var filePaths = Directory.GetFiles(path).Where(p => !p.EndsWith(".meta"));
            foreach (var filePath in filePaths)
            {
                var scenePathIndex = filePath.IndexOf("/Assets/");
                var scenePath = filePath.Substring(scenePathIndex + 1);
                Debug.Log("Found a scene at " + scenePath);
                EditorBuildSettingsScene scene = new EditorBuildSettingsScene(scenePath, true);
                scenes.Add(scene);
            }
        }
        
        //load from array of scenes
        for (int i = 0; i < testConfiguration.ScenesToTest.Length; i++)
        {
            string pathToScene = AssetDatabase.GetAssetPath(testConfiguration.ScenesToTest[i]);
            EditorBuildSettingsScene scene = new EditorBuildSettingsScene(pathToScene, true);
            scenes.Add(scene);
        }

        EditorBuildSettings.scenes = scenes.ToArray();
    }

    void Start()
    {
        //backup scene list before run the test
        DontDestroyOnLoad(this);
        scenes = EditorBuildSettings.scenes.ToList();
        OpenNextTestScene();
    }

    private void OnDestroy()
    {
    }

    public void OpenNextTestScene()
    {
        if (currentSceneIndex < 0 || currentSceneIndex > scenes.Count)
        {
            _isFinished = true;
            return;
        }

        SceneManager.LoadScene(currentSceneIndex);

        currentSceneIndex++;
    }

    public bool IsTestFinished
    {
        get { return _isFinished; }
    }
}