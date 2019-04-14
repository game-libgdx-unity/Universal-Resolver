using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UnityIoC
{
    [Serializable]
    public class BaseBindingSetting : ScriptableObject
    {
        [SerializeField] public Object assemblyHolder;
        [SerializeField] public bool autoProcessSceneObjects = false;
        [SerializeField] public bool ignoreGameComponent = false;
    }
}