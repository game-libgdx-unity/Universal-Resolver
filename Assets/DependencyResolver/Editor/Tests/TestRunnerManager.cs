using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityIoC;

// ReSharper disable All

public class TestRunner : SingletonBehaviour<TestRunner>, IMonoBehaviourTest, ITestScene
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
    public void AddScenesToBuildAndRunTest()
    {
        for (int i = 0; i < testConfig.ScenesToTest.Length; i++)
        {
            string pathToScene = AssetDatabase.GetAssetPath(testConfig.ScenesToTest[i]);
            EditorBuildSettingsScene scene = new EditorBuildSettingsScene(pathToScene, true);
            scenes.Add(scene);
        }

        EditorBuildSettings.scenes = scenes.ToArray();
        EditorUtility.SetDirty(testConfig);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    IEnumerator Start()
    {
        //backup scene list before run the test
        var testConfig = Resources.Load<TestConfiguration>(TestConfigName);
        bk_scenes = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);

        DontDestroyOnLoad(this);

        testConfig = Resources.Load<TestConfiguration>(TestConfigName);
        currentSceneIndex = 0;
        scenes = new List<EditorBuildSettingsScene>();

        if (!string.IsNullOrEmpty(testConfig.sceneFolderPath))
        {
            var path = Application.dataPath + "/" + testConfig.sceneFolderPath;
            var filePaths = Directory.GetFiles(path).Where(p => !p.EndsWith(".meta"));
            foreach (var filePath in filePaths)
            {
                var scenePathIndex = filePath.IndexOf("/Assets/");
                var scenePath = filePath.Substring(scenePathIndex + 1);
                EditorBuildSettingsScene scene = new EditorBuildSettingsScene(scenePath, true);
                scenes.Add(scene);
            }
        }

        for (int i = 0; i < testConfig.ScenesToTest.Length; i++)
        {
            string pathToScene = AssetDatabase.GetAssetPath(testConfig.ScenesToTest[i]);
            EditorBuildSettingsScene scene = new EditorBuildSettingsScene(pathToScene, true);
            scenes.Add(scene);
        }

        EditorBuildSettings.scenes = scenes.ToArray();

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        OpenNextTestScene();

        while (!_isFinished)
        {
            yield return null;
        }

//        _isFinished = true;
    }

    private void OnDestroy()
    {
        //restore scene
        EditorBuildSettings.scenes = bk_scenes.ToArray();

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
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