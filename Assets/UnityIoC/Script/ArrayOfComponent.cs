using UnityEngine;

namespace UnityIoC
{
    public class ArrayOfComponent : MonoBehaviour
    {
        [Component] public ISomeComponentInterface[] SomeComponents;
    }
}