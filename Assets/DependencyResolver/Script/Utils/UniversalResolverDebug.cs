using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace UnityIoC
{
    /// <summary>
    /// Custom debug with Conditional attributes and enable logging designed for UnityIoC
    /// </summary>
    public static class UniversalResolverDebug
    {
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void Log(this Logger logger, string s, params object[] param)
        {
            if (Context.Setting.EnableLogging && logger.enableLogging)
            {
                UnityEngine.Debug.LogFormat("{0}\t{1}", logger.type.Name,
                    string.Format(s.CapitalizeFirstChar(), param));
            }
        }

        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void LogError(this Logger logger, string s, params object[] param)
        {
            if (Context.Setting.EnableLogging && logger.enableLogging)
            {
                UnityEngine.Debug.LogErrorFormat("{0}\t{1}", logger.type.Name,
                    string.Format(s.CapitalizeFirstChar(), param));
            }
        }

        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void Log(string s, params object[] param)
        {
            if (!Context.Setting.EnableLogging) return;
            UnityEngine.Debug.LogFormat(s.TabAndCapitalizeFirstChar(), param);
        }

        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void LogError(string s, params object[] param)
        {
            if (!Context.Setting.EnableLogging) return;
            UnityEngine.Debug.LogErrorFormat(s.TabAndCapitalizeFirstChar(), param);
        }

        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void LogFormat(string s, params object[] param)
        {
            if (!Context.Setting.EnableLogging) return;
            UnityEngine.Debug.LogFormat(s.TabAndCapitalizeFirstChar(), param);
        }

        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void LogErrorFormat(string s, params object[] param)
        {
            if (!Context.Setting.EnableLogging) return;
            UnityEngine.Debug.LogErrorFormat(s.TabAndCapitalizeFirstChar(), param);
        }

        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void Assert(bool condition, string message)
        {
            UnityEngine.Debug.Assert(condition, message.TabAndCapitalizeFirstChar());
        }

        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void Assert(bool condition, string message, params object[] param)
        {
            UnityEngine.Debug.AssertFormat(condition, message.TabAndCapitalizeFirstChar(), param);
        }

        private static string CapitalizeFirstChar(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            return input.First().ToString().ToUpper() + input.Substring(1);
        }

        private static string TabAndCapitalizeFirstChar(this string input)
        {
            return "\t\t" + input.First().ToString().ToUpper() + input.Substring(1);
        }
    }

    [Serializable]
    public class Logger
    {
        [SerializeField] public bool enableLogging = true;
        [HideInInspector] public Type type;
        [HideInInspector] public object context;

        public Logger(object context):this(context.GetType())
        {
            this.type = context.GetType();
            this.context = context;
            
            //doesn't allow to log in production code
            #if !UNITY_EDITOR && !DEVELOPMENT_BUILD
            
            enableLogging = false;
            
            #endif
        }

        public Logger(Type type, bool enableLogging = true)
        {
            this.enableLogging = enableLogging;
            this.type = type;
        }
    }
}