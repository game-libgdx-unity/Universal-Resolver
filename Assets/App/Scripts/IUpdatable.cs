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
        /// <summary>
        /// If this object should be frequently updated in SystemManager
        /// </summary>
        bool Enable { get; set; }
        
        /// <summary>
        /// Lightweight transform
        /// </summary>
        MyTransform Transform { get; set; }
        
        /// <summary>
        /// Update every frame
        /// </summary>
        /// <param name="delta_time">time since last frame</param>
        /// <param name="game_time">total game time</param>
        void Update(float delta_time, double game_time);
    }
}