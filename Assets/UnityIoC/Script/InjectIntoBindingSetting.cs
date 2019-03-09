using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace UnityIoC
{
    [CreateAssetMenu(fileName = "default", menuName = "IoC/InjectInto Binding Data", order = 1)]
    [Serializable]
    public class InjectIntoBindingSetting : ScriptableObject
    {
        [SerializeField] public List<InjectIntoBindingAsset> defaultSettings = new List<InjectIntoBindingAsset>();
    }
}