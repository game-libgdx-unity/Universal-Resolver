using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace UnityIoC
{
    /// <summary>
    /// Custom debug with Conditional attributes and enable logging designed for UnityIoC
    /// </summary>
    public static class Debug
    {
        public static bool EnableLogging = true;
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void Log(string s, params object[] param)
        {
            if (!EnableLogging) return;
            UnityEngine.Debug.LogFormat(s, param);
        }
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void LogFormat(string s, params object[] param)
        {
            if (!EnableLogging) return;
            UnityEngine.Debug.LogFormat(s, param);
        }

        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void LogError(string s, params object[] param)
        {
            if (!EnableLogging) return;
            UnityEngine.Debug.LogErrorFormat(s, param);
        }
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void LogErrorFormat(string s, params object[] param)
        {
            if (!EnableLogging) return;
            UnityEngine.Debug.LogErrorFormat(s, param);
        }

        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void Assert(bool condition, string message)
        {
            UnityEngine.Debug.Assert(condition, message);
        }
        
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void Assert(bool condition, string message, params object[] param)
        {
            UnityEngine.Debug.AssertFormat(condition, message, param);
        }
    }
}