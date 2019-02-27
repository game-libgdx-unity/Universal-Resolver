using UnityEngine;

namespace UnityIoC.Editor
{
    public class ArrayOfComponent : MonoBehaviour
    {
        [GetComponent] public ISomeComponentInterface[] SomeComponents;
        [GetComponent] public ISomeComponentInterface[] SomeComponentsAsProperty { get; set; }
    }

    public interface ISomeComponentInterface
    {
    }
}