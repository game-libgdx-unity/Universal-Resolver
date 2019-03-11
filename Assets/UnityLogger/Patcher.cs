using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using Harmony;
using UnityEngine;
using UnityIoC;
using Logger = UnityIoC.Logger;

public static class Patcher
{
    public const string AssemblyName = "MyDynamicAssembly";
    public const string ModuleName = "MyModule";
    public const string TypeNameFormat = "Patch_{0}";
    public const string MethodNameFormat = "PatchMethod_{0}";

    private static AssemblyBuilder _defaultAssemblyBuilder;
    private static ModuleBuilder _defaultModuleBuilder;
    private static HarmonyInstance _harmony;
    private static Logger debug = new Logger(typeof(Patcher));

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
    /// Process an assembly for [Log] annotations
    /// </summary>
    /// <param name="assembly"></param>
    public static void Process(Assembly assembly)
    {
        _harmony = HarmonyInstance.Create(assembly.GetName().Name);
        debug.Log("Process assembly " + assembly.GetName().Name);
        foreach (var type in assembly.GetTypes())
        {
//get all methods with LogAttribute
            var methods = type.GetMethods(BindingFlags.Public
                                          | BindingFlags.NonPublic
                                          | BindingFlags.Static
                                          | BindingFlags.Instance)
                .Where(method => method.IsDefined(typeof(LogAttribute), true))
                .ToArray();

//process next type if current one doesn't have [Log]
            if (methods.Length == 0)
            {
                continue;
            }

            debug.Log("Process type " + type.FullName);

//create a customType for type
            var customType = GetDefaultModuleBuilder()
                .AddType(String.Format(TypeNameFormat, type.FullName))
                .AddMethods(methods)
                .CreateType();

//define binding flags
            var bindingFlags = BindingFlags.Public
                               | BindingFlags.Static;


            //get transpiler method
            var methodTranspiler = typeof(UnityConsole).GetMethod("Transpiler", bindingFlags);
//patch the method with a logPatch
            foreach (var method in methods)
            {
//get the method to patch
                var patchMethod = customType.GetMethod(
                    string.Format(MethodNameFormat, method.Name),
                    bindingFlags);
                debug.Log("Patch {1} for {0} in type {2}", method.Name, patchMethod.Name, type.FullName);
                _harmony.Patch(method, new HarmonyMethod(patchMethod), null, new HarmonyMethod(methodTranspiler));
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
//get the method properties
        var returnType = typeof(void);
        var parameterInfos = methodInfo.GetParameters();
        var parameterTypes = parameterInfos.Select(p => p.ParameterType).ToArray();
        var methodName = string.Format(MethodNameFormat, methodInfo.Name);

//define the method
        var methodBuilder = typeBuilder.DefineMethod(methodName,
            MethodAttributes.Public | MethodAttributes.Static,
            returnType,
            parameterTypes);

//copy the parameters
        for (int i = 1; i <= parameterInfos.Length; i++)
        {
            methodBuilder.DefineParameter(i, parameterInfos[i - 1].Attributes, parameterInfos[i - 1].Name);
        }

        var logAttribute = (LogAttribute)methodInfo.GetCustomAttributes(typeof(LogAttribute), true).FirstOrDefault();

//define the method body
        ILGenerator il = methodBuilder.GetILGenerator();

        var bindingFlags = BindingFlags.Public | BindingFlags.Static;
        var cacheMethod = typeof(UnityConsole).GetMethod("Cache",
            bindingFlags
        );       
        
        var logMethod = typeof(UnityConsole).GetMethod("Log",
            bindingFlags,
            null,
            parameterTypes,
            null
        );

        logAttribute.MethodName = methodInfo.Name;
        logAttribute.DeclaringType = methodInfo.DeclaringType.Name;
        var json = JsonUtility.ToJson(logAttribute);

        il.Emit(OpCodes.Ldstr, json);
        il.EmitCall(OpCodes.Call, cacheMethod, null);
        il.Emit(OpCodes.Nop);
        
        if (parameterInfos.Length == 1)
        {
            il.Emit(OpCodes.Ldarg_0);
        }
        else if (parameterInfos.Length == 2)
        {
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
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
        TypeBuilder typeBuilder = moduleBuilder.DefineType(TypeNameFormat);
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