using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;


#if USE_LOG_DEFAULT_TEST
[InitializeOnLoad]
#endif
public static class AutoLogProcesser
{
    static AutoLogProcesser()
    {
        if (SceneManager.GetActiveScene().name != "TestLogScene")
            return;
//clear console, take extra care cause it can remove logs from previous run
//        TestBase.ClearConsole();

//        customType = CustomType.AddMethods();
        var assembly = Assembly.GetExecutingAssembly();
        Patcher.Process(assembly);
        foreach (var mono in Object.FindObjectsOfType<MonoBehaviour>())
        {
//            try to invoke the method Start()
            var unityComp = mono as UnityComponent;
            if (unityComp != null)
            {
                unityComp.Start();
                unityComp.Start2();
                unityComp.Start3();
            }
        }
    }
}

public class PatchFactory
{
    public static MethodInfo Get(MethodInfo method)
    {
        var returnType = method.ReturnType;
        var parameterTypes = method.GetParameters().Select(p => p.ParameterType).ToArray();
        var methodDeclaringType = method.DeclaringType;
        var logMethods = typeof(UnityConsole).GetMethods(BindingFlags.Public | BindingFlags.Static);
        MethodInfo logMethod =
            logMethods.FirstOrDefault(m => m.GetParameters().Length == method.GetParameters().Length);
        return logMethod;
    }
}

public static class TranspilerPatcher
{
}