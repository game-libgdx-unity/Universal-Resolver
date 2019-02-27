using System.Collections.Generic;
using UnityEngine;

namespace UnityIoC
{
    [CreateAssetMenu(fileName = "default", menuName = "IoC/Binding Data", order = 1)]
    public class BindingSetting : ScriptableObject
    {
        [SerializeField] public BindingData[] defaultSettings;
    }
}