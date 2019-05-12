using System;

namespace UnityIoC
{
    public interface IPoolable
    {
        bool Alive { get; set; }
        void Init();
    }
}