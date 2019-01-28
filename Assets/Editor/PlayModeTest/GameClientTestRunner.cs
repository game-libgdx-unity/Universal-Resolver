using System;
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Firebase.Database;
using UniRx;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UtilTestRunner
{
    [SetUp]
    public void Setup()
    {
//        GameClient.EnableLogging = true;
//        GameClient.AutoLogin = true;
//        GameObject obj = new GameObject();
//        obj.AddComponent<GameClient>();
    }

    void print(string str)
    {
        Debug.Log(str);
    }

    [UnityTest]
    public IEnumerator MyLog_ShowConsoleLog()
    {
        Observable.Range(0, 10).SubscribeToGUI("my test");
        Observable.Range(0, 10).SubscribeToConsole("my test");

        yield return new WaitForSeconds(2);
    }

    [UnityTest]
    public IEnumerator Test_Observable()
    {
        var observable = Observable.Range(0, 10);

        observable.SubscribeToConsole("Number");

        yield return observable.IsValued(9);
    }

    [UnityTest]
    public IEnumerator Test_Observable2()
    {
        var observable = Observable.Range(0, 10);

        observable.SubscribeToConsole("Number");

        yield return observable.IsValued(12);
    }


    [UnityTest]
    public IEnumerator Test_Is_Observable()
    {
        var observable = Observable.Range(0, 3);

        observable.SubscribeToConsole("Number");

        yield return observable.Is(0, 1, 2);
    }


    void Fail()
    {
        Assert.Fail();
    }

    void Pass()
    {
        Assert.Pass();
    }

//    [Test]
//    public void Test_CreateInstance()
//    {
//        var testConfig = Resources.Load<ImplementedClass>("TestConfiguration");
//
//        var type = testConfig.classObjectsToLoad.name;
//
////        var obj = Activator.CreateInstance(type);
//        
//        var obj = Activator.CreateInstance("MyAssembly",type);
//        
//        print("objType "+obj.GetType());
//        
//        Assert.IsNotNull(obj);
//    }

//    [UnityTest]
//    public IEnumerator MyLog_ShowConsoleLog()
//    {
//        var myLog = MyLog.Instance;
//        myLog.logSystem = false;
//        myLog.logType = LogType.Log;
//
//        print("hello");
//
//        yield return new WaitForSeconds(2);
//    }

//    [Test]
//    public void ModifyTestConfig()
//    {
//        var testConfig = Resources.Load<TestConfiguration>("TestConfiguration");
//        var scenes = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
//        Object[] newScenes = scenes
//            .Select(editorBuildSettingsScene => AssetDatabase.LoadAssetAtPath<Object>(editorBuildSettingsScene.path))
//            .ToArray();
//
//        testConfig.ScenesToTest = newScenes;
//
////        EditorUtility.SetDirty(testConfig);
//
//        AssetDatabase.SaveAssets();
//        AssetDatabase.Refresh();
//    }
//
//    [Test]
//    public void SceneBuildSettings()
//    {
//        // Use the Assert class to test conditions.
//        Debug.ClearDeveloperConsole();
//
//        var testConfig = Resources.Load<TestConfiguration>("TestConfiguration");
//        var scenes = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
//
//        for (int i = 0; i < testConfig.ScenesToTest.Length; i++)
//        {
//            string pathToScene = AssetDatabase.GetAssetPath(testConfig.ScenesToTest[i]);
//            EditorBuildSettingsScene scene = new EditorBuildSettingsScene(pathToScene, true);
//            scenes.Add(scene);
//        }
//
//        EditorBuildSettings.scenes = scenes.ToArray();
//        EditorUtility.SetDirty(testConfig);
//    }
}