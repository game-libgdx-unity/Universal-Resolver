using System.Collections;
using System.Linq;
using Unity.Linq;
using UnityEngine;
using UnityEngine.Rendering;

namespace UnityIoC
{
    /// <summary>
    /// Allow objects to be updated in SystemManager
    /// </summary>
    public interface IUpdatable : IPoolable
    {
        bool Enable { get; set; }
        void Update(float delta_time, float game_time);
    }

    /// <summary>
    /// Allow objects to be updated in SystemManager
    /// </summary>
    public interface IUpdatableItem : IUpdatable
    {
        Matrix4x4 Transform { get; }
    }
}