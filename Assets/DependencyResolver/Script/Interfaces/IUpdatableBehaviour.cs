using UnityEngine;

namespace UnityIoC
{
    /// <summary>
    /// Allow game objects to be updated in SystemManager
    /// </summary>
    public interface IUpdatableBehaviour : IUpdatable
    {
        Transform transform { get; }
        Renderer renderer { get; }
    }
}