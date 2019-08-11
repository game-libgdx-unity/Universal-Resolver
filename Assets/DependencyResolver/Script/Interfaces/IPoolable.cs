using System;

namespace UnityIoC
{
    public interface IPoolable
    {
        /// <summary>
        /// if Object is alive or dead in Pool life cycle
        /// </summary>
        bool Alive { get; set; }
        
        /// <summary>
        /// Get called when this object is retrieved from Pool
        /// </summary>
        void Init();
    }
}