using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityIoC
{
    [CreateAssetMenu(fileName = "Assembly-CSharp", menuName = "IoC/Binding Data", order = 1)]
    [Serializable]
    public class BindingSetting : BaseBindingSetting
    {
        [SerializeField] public BindingAsset[] defaultSettings = new BindingAsset[] { };
    }
}