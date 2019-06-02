using System;
using UnityEngine;

namespace UnityIoC
{
    public class JsonHelper
    {
        [System.Serializable]
        private class Wrapper<T>
        {
            public T[] array;
        }

        public static T[] getJsonArray<T>(string json)
        {
            string newJson = "{ \"array\": " + json + "}";
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
            return wrapper.array;
        }

        public static T getJson<T>(string json)
        {
            return JsonUtility.FromJson<T>(json);
        }
    }

    [Serializable]
    public struct ClassName
    {
        public const string FIELD = nameof(className);
        public string className;
    }
}