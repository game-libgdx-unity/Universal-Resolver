using UnityEngine;

namespace UnityIoC
{
    public class ArrayOfComponent : MonoBehaviour
    {
        [FromComponent] public ISomeComponentInterface[] SomeComponents;
        [FromComponent] public ISomeComponentInterface[] SomeComponentsAsProperty { get; set; }
    }
}