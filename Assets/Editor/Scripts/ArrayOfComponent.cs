using UnityEngine;

namespace UnityIoC.Editor
{
    public class ArrayOfComponent : MonoBehaviour
    {
        [Component] public ISomeComponentInterface[] SomeComponents;
        [Component] public ISomeComponentInterface[] SomeComponentsAsProperty { get; set; }
    }

    public interface ISomeComponentInterface
    {
    }
}