using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace UnityIoC
{
    [CreateAssetMenu(fileName = "default", menuName = "IoC/InjectInto Binding Data", order = 1)]
    public class InjectIntoBindingSetting : ScriptableObject
    {
        [SerializeField] public List<InjectIntoBindingData> defaultSettings = new List<InjectIntoBindingData>();
    }
}