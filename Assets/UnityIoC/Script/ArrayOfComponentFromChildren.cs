using UnityEngine;

namespace UnityIoC
{
    public class ArrayOfComponentFromChildren : MonoBehaviour
    {
        [Children] public ISomeComponentInterface[] SomeComponents;
        [Children] public ISomeComponentInterface[] SomeComponentsAsProperty { get; set; }
    }
}