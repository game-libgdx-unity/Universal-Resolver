using System.Collections;
using System.Linq;
using Unity.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UTJ;

namespace UnityIoC
{
    /// <summary>
    /// Allow objects to be updated in SystemManager
    /// </summary>
    public interface IUpdatable : IPoolable
    {
        bool Enable { get; set; }
        void Update(float delta_time, double game_time);
        MyTransform Transform { get; }
    }
}