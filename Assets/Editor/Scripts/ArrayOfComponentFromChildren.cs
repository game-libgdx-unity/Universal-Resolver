using UnityEngine;

namespace UnityIoC.Editor
{
    public class ArrayOfComponentFromChildren : MonoBehaviour
    {
        [Children] public ISomeComponentInterface[] SomeComponents;
        [Children] public ISomeComponentInterface[] SomeComponentsAsProperty { get; set; }
    }
}