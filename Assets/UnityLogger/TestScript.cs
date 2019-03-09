using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEditor;
using UnityEngine;
using Harmony;
using UnityIoC.Editor;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

public static class UnityConsole
{
    public static void WriteLine(string format, object[] parameters)
    {
        Debug.LogFormat(format, parameters);
    }

    public static void WriteLine(object format)
    {
        Debug.Log(format);
    }

    public static void LogOneParameters(object msg)
    {
        Debug.Log(msg);
//        StackTrace trace = new StackTrace(true);
//        Debug.LogFormat("Called from method " + trace.GetFrame(1).GetMethod().Name);
////        Debug.LogFormat("Called from method " + trace.GetFrame(2).GetMethod().Name);
////        Debug.LogFormat("Called from file " + trace.GetFrame(1).GetFileName());
//        Debug.LogFormat("Called from file " + trace.GetFrame(2).GetFileName());
    }
    
    public static void LogNoParameters()
    {
        StackTrace trace = new StackTrace(true);
        Debug.LogFormat("Called from method " + trace.GetFrame(1).GetMethod().Name);
//        Debug.LogFormat("Called from method " + trace.GetFrame(2).GetMethod().Name);
//        Debug.LogFormat("Called from file " + trace.GetFrame(1).GetFileName());
        Debug.LogFormat("Called from file " + trace.GetFrame(2).GetFileName());
    }
}

[InitializeOnLoad]
public static class AutoLogProcesser
{
    public static HarmonyInstance harmony;
    public static Type customType;
    private static BindingFlags bindingFlags;

    static AutoLogProcesser()
    {
        //clear console, take extra care cause it can remove logs from previous run
        TestBase.ClearConsole();

//        customType = CustomType.AddMethods();

        var assembly = Assembly.GetExecutingAssembly();

        Debug.LogFormat("Current Assembly: {0}", assembly.GetName().Name);

        Patcher.Process(assembly);

        
        
        foreach (var mono in Object.FindObjectsOfType<MonoBehaviour>())
        {
//            try to invoke the method
            var unityComp = mono as UnityComponent;
            if (unityComp != null)
            {
                unityComp.Start();
            }
        }
    }

    private static void ProcessMethodInfo(object mono, MethodInfo method)
    {
    }
}


public static class Patcher
{
    public const string AssemblyName = "MyDynamicAssembly";
    public const string ModuleName = "MyModule";
    public const string TypeName = "MyType";
    public const string MethodNameFormat = "PatchMethod_{0}";

    private static AssemblyBuilder _defaultAssemblyBuilder;
    private static ModuleBuilder _defaultModuleBuilder;
    public static HarmonyInstance _harmony;

    /// <summary>
    /// Get default module for default custom assembly
    /// </summary>
    /// <returns></returns>
    private static ModuleBuilder GetDefaultModuleBuilder()
    {
        if (_defaultModuleBuilder == null)
        {
            AppDomain currentDomain = Thread.GetDomain();

            AssemblyName assemblyName = new AssemblyName();
            assemblyName.Name = AssemblyName;

            _defaultAssemblyBuilder = currentDomain.DefineDynamicAssembly(
                assemblyName,
                AssemblyBuilderAccess.RunAndSave);

            _defaultModuleBuilder = _defaultAssemblyBuilder.DefineDynamicModule(ModuleName);
        }

        return _defaultModuleBuilder;
    }

    /// <summary>
    /// Process an assemably for [Log] annotations
    /// </summary>
    /// <param name="assembly"></param>
    public static void Process(Assembly assembly)
    {
        _harmony = HarmonyInstance.Create(assembly.GetName().Name);

        Debug.Log("Process assembly " + assembly.GetName().Name);

        foreach (var type in assembly.GetTypes())
        {
            //get all methods with LogAttribute
            var methods = type.GetMethods(BindingFlags.Public
                                          | BindingFlags.NonPublic
                                          | BindingFlags.Static
                                          | BindingFlags.Instance)
                .Where(method => method.IsDefined(typeof(LogAttribute), true))
                .ToArray();

            if (methods.Length == 0)
            {
                continue;
            }

            Debug.Log("Process type " + type.FullName);

            //create a customType for type
            var customType = GetDefaultModuleBuilder()
                .AddType(String.Format("Patch_{0}", type.FullName))
                .AddMethods(methods)
                .CreateType();

            //define binding flags
            var bindingFlags = BindingFlags.Public
                               | BindingFlags.Static;

            //patch the method with a logPatch
            foreach (var method in methods)
            {
                var patchMethod = customType.GetMethod(
                    string.Format(MethodNameFormat, method.Name),
                    bindingFlags);

                Debug.LogFormat("Patch {1} for {0} in type {2}", method.Name, patchMethod.Name, type.FullName);

                _harmony.Patch(method, new HarmonyMethod(patchMethod));
            }
        }
    }

    /// <summary>
    /// Build a method based on a method info
    /// </summary>
    /// <param name="methodInfo"></param>
    /// <returns>The method that is generated</returns>
    private static void AddMethod(this TypeBuilder typeBuilder, MethodInfo methodInfo)
    {
        var returnType = typeof(void);
        var parameterInfos = methodInfo.GetParameters();
        var parameterTypes = parameterInfos.Select(p => p.ParameterType).ToArray();
        var methodName = string.Format(MethodNameFormat, methodInfo.Name);

        //define the method
        var methodBuilder = typeBuilder.DefineMethod(
            methodName,
            MethodAttributes.Public | MethodAttributes.Static,
            returnType,
            parameterTypes);

        //copy the parameters
        for (int i = 1; i <= parameterInfos.Length; i++)
        {
            methodBuilder.DefineParameter(i, parameterInfos[i - 1].Attributes, parameterInfos[i - 1].Name);
        }
        
        //define the method body
        MethodInfo logMethod;
        ILGenerator il = methodBuilder.GetILGenerator();
        if (parameterInfos.Length == 0)
        {
            logMethod = typeof(UnityConsole).GetMethod("LogNoParameters",
                BindingFlags.Public | BindingFlags.Static);
        }
        if (parameterInfos.Length == 1)
        {
            il.Emit(OpCodes.Ldarg_0);
            logMethod = typeof(UnityConsole).GetMethod("LogOneParameters",
                BindingFlags.Public | BindingFlags.Static);
        }
        else
        {
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            logMethod = typeof(UnityConsole).GetMethod("WriteLine",
                BindingFlags.Public | BindingFlags.Static,
                null,
                parameterTypes,
                null
            );
        }

        il.EmitCall(OpCodes.Call, logMethod, null);
        il.Emit(OpCodes.Ret);
    }
    /// <summary>
    /// Build a custom type
    /// </summary>
    /// <param name="objectName"></param>
    /// <returns></returns>
    public static TypeBuilder AddType(this ModuleBuilder moduleBuilder, string objectName)
    {
        TypeBuilder typeBuilder = moduleBuilder.DefineType(TypeName);
        return typeBuilder;
    }

    /// <summary>
    /// Build methods for this CustomType class
    /// </summary>
    /// <param name="methodInfos"></param>
    private static TypeBuilder AddMethods(this TypeBuilder typeBuilder, MethodInfo[] methodInfos)
    {
        foreach (var info in methodInfos)
        {
            AddMethod(typeBuilder, info);
        }

        return typeBuilder;
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
    public static object Parameter_0;
    public static object Parameter_1;
    public static object Parameter_2;
    public static object Parameter_3;
    public static object Parameter_4;
    public static object Parameter_5;
    public static object Parameter_6;

    public static IEnumerable<CodeInstruction> Transpiler(MethodBase original,
        IEnumerable<CodeInstruction> instructions)
    {
        MethodInfo logMethod =
            typeof(UnityConsole).GetMethod("LogNoParameters", BindingFlags.Public | BindingFlags.Static);

        var codes = new List<CodeInstruction>(instructions);

//        codes.Insert(0, new CodeInstruction(OpCodes.Call, logMethod));

        UnityConsole.LogNoParameters();
        UnityConsole.WriteLine(original.GetParameters().Length.ToString());

        var ci = codes.FirstOrDefault(c => c.opcode == OpCodes.Ldarg_0);
        if (ci != null) //this condition is never happened
        {
            Debug.Log("First parameter: " + ci.operand);
        }
        else
        {
            Debug.Log("No parameter");
        }

        codes.ForEach(c => { Debug.LogFormat("{0}: {1}", c.opcode, c.operand); });

        return codes.AsEnumerable();
    }
}