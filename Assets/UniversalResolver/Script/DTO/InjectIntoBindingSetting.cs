using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace UnityIoC
{
    [CreateAssetMenu(fileName = "Assembly-CSharp", menuName = "IoC/InjectInto Binding Data", order = 1)]
    [Serializable]
    public class InjectIntoBindingSetting : BaseBindingSetting
    {
        [SerializeField] public List<InjectIntoBindingAsset> defaultSettings = new List<InjectIntoBindingAsset>();
    }
}