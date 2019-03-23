using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using SceneTest;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class ScriptTest : MonoBehaviour, IRunScriptOnEditor
{
    public void DoAction()
    {
        ClearConsole();
        
        Debug.Log("Hello script");

        EditorSceneManager.LoadScene(EditorSceneManager.GetActiveScene().name);
        
        var obj = GameObject.Find("GameObject");
        
        var t = obj.GetComponent<TestComponent>();
        t.abstractClass = new ImplClass(); 
        
        var o2 = Object.Instantiate(obj);
        var t2 = Object.Instantiate(t);
        
        Debug.Log(o2.GetComponent<TestComponent>());
        Debug.Log(o2.GetComponent<TestComponent>().abstractClass == null);
        
        Debug.Log(t2.GetComponent<TestComponent>());
        Debug.Log(t2.GetComponent<TestComponent>().abstractClass == null);
    }

    /**/
    public static void ClearConsole()
    {
        //reflection api will call a method from unity editor object
        //ref: https://github.com/Unity-Technologies/UnityCsReference/blob/master/Editor/Mono/LogEntries.bindings.cs
        var assembly = Assembly.GetAssembly(typeof(UnityEditor.ActiveEditorTracker));
        var type = assembly.GetType("UnityEditor.LogEntries");
        if (type == null)
        {
            Debug.Log("The class not found, you should check the link above to remake the reflection code");
            return;
        }

        var method = type.GetMethod("Clear");
        method.Invoke(new object(), null);
    }
}

public interface IRunScriptOnEditor
{
    void DoAction();
}