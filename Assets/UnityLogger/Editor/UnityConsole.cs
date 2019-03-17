using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using Harmony;
using UnityEngine;
using UnityIoC;
using Debug = UnityEngine.Debug;
using Logger = UnityIoC.Logger;

public static class UnityConsole
{
    static Logger debug = new Logger(typeof(UnityConsole));

    public static LogAttribute currentLogAttribute;
    
    private static int logDetailsCount;

    public static string currentAttributeJson;

    public static object Parameter_0;
    public static object Parameter_1;
    public static object Parameter_2;
    public static object Parameter_3;
    public static object Parameter_4;
    public static object Parameter_5;
    public static object Parameter_6;

    static UnityConsole()
    {
        logDetailsCount = Enum.GetValues(typeof(LogDetails)).Length;
    }

    public static IEnumerable<CodeInstruction> Transpiler(MethodBase original,
        IEnumerable<CodeInstruction> instructions)
    {
//        Debug.Log("Transpiler called "+original.Name);
//        currentMethod = original;
//        currentLogAttribute = (LogAttribute) original.GetCustomAttributes(typeof(LogAttribute), true).FirstOrDefault();
        return instructions;
    }

    public static void Cache(string originalJson)
    {
        currentLogAttribute = JsonUtility.FromJson<LogAttribute>(originalJson);
    }

    public static void Log(
        object param_1,
        object param_2,
        object param_3,
        object param_4,
        object param_5)
    {
        var trace = new StackTrace(true);
        var stackFrame = trace.GetFrame(1);

        var paramArray = new object[] {param_1, param_2, param_3, param_4, param_5};
        var strBuilder = new StringBuilder();
        var typeDetails = String.Empty;
        var methodDetails = String.Empty;
        var fileNameDetails = String.Empty;
        var lineNumberDetails = String.Empty;

        var typeDetailsFormat = currentLogAttribute.Data.headerFormat.IndexOf("{0}", StringComparison.Ordinal);
        var headerDetailsFormat = currentLogAttribute.Data.headerFormat;
        var contentDetailsFormat = currentLogAttribute.Data.contentFormat;

        if ((currentLogAttribute.Data.LogDetails & LogDetails.LineNumber) == LogDetails.LineNumber)
        {
            lineNumberDetails = string.Format("({0}, {1})",
                stackFrame.GetFileLineNumber(),
                stackFrame.GetFileColumnNumber());
        }
        else
        {
            headerDetailsFormat = TrimStringFormat(headerDetailsFormat, "{3}");
            headerDetailsFormat = TrimStringFormatFromEnd(headerDetailsFormat, "{3}");
        }

        if ((currentLogAttribute.Data.LogDetails & LogDetails.Filename) == LogDetails.Filename)
        {
            var fileName = stackFrame.GetFileName();

            fileNameDetails = fileName == null
                ? string.Empty
                : fileName.Substring(fileName.LastIndexOf("/" + 1, StringComparison.Ordinal));
        }
        else
        {
            headerDetailsFormat = TrimStringFormat(headerDetailsFormat, "{2}");
            headerDetailsFormat = TrimStringFormatFromEnd(headerDetailsFormat, "{2}");
        }

        if ((currentLogAttribute.Data.LogDetails & LogDetails.Method) == LogDetails.Method)
        {
            methodDetails = currentLogAttribute.MethodName;
        }
        else
        {
            headerDetailsFormat = TrimStringFormat(headerDetailsFormat, "{1}");
            headerDetailsFormat = TrimStringFormatFromEnd(headerDetailsFormat, "{1}");
        }

        if ((currentLogAttribute.Data.LogDetails & LogDetails.Type) == LogDetails.Type)
        {
            typeDetails = currentLogAttribute.DeclaringType;
        }
        else
        {
            headerDetailsFormat = TrimStringFormat(headerDetailsFormat, "{0}");
            headerDetailsFormat = TrimStringFormatFromEnd(headerDetailsFormat, "{0}");
        }

//        Debug.Log(headerDetailsFormat);
        
        var headerStr = string.Format(headerDetailsFormat,
            typeDetails, methodDetails, fileNameDetails, lineNumberDetails);

        debug.Log("Header str " + headerStr);


        for (int i = 0; i < paramArray.Length; i++)
        {
            //trim empty elements
            if (paramArray[i] == null)
            {
                contentDetailsFormat = TrimAndRemoveStringPatter(contentDetailsFormat, "{" + i + "}");
            }
        }

//        Debug.Log(contentDetailsFormat);


        var contentStr = string.Format(contentDetailsFormat,
            param_1);


        var finalLog = string.Format(currentLogAttribute.Data.logFormat, headerStr, contentStr);

        Debug.Log(finalLog);
    }

    public static void Log(
        object param_1)
    {
        Log(param_1, null, null, null, null);
    }
    
    
    public static string SubstringInRange(this string s, int startIndex, int endIndex)
    {
        var length = endIndex - startIndex + 1;

        if (string.IsNullOrEmpty(s) || s.Length < length || length < 0)
        {
            return s;
        }

        return s.Substring(startIndex, length);
    }
    private static string TrimAndRemoveStringPatter(this string str, string pattern)
    {
        if (str.Length < pattern.Length)
            return str;

        var endIndex = str.LastIndexOf(pattern, StringComparison.Ordinal) + pattern.Length - 1;

        debug.Log("endIndex " + endIndex);

        var stringToPattern = str.SubstringInRange(0, endIndex - pattern.Length);

        debug.Log("stringToPattern " + stringToPattern);

        var lastIndex = stringToPattern.LastIndexOf("}", StringComparison.Ordinal) - 1;

        debug.Log("lastIndex " + lastIndex);

        return str.SubstringInRange(0, lastIndex) + str.Substring(endIndex);
    }
    
    private static string TrimStringFormat(this string str, string pattern)
    {
        if (str.Length < pattern.Length)
            return str;
        var endIndex = str.LastIndexOf(pattern, StringComparison.Ordinal) + pattern.Length - 1;
        var nextIndex = str.IndexOf("{", endIndex);
        return str.SubstringInRange(0, endIndex) + str.Substring(nextIndex < 0? str.Length : nextIndex);
    }
    private static string TrimStringFormatFromEnd(this string str, string pattern)
    {
        if (str.Length < pattern.Length)
            return str;
        var endIndex = str.LastIndexOf(pattern, StringComparison.Ordinal);
        var stringToPattern = str.SubstringInRange(0, endIndex);
        var lastIndex = stringToPattern.LastIndexOf("}", StringComparison.Ordinal);
        return str.SubstringInRange(0, lastIndex) + str.Substring(endIndex);
    }
}