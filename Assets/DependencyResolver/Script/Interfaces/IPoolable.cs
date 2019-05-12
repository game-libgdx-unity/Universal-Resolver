using System;

namespace UnityIoC
{
    public interface IPoolable : IDisposable
    {
        bool Alive { get; set; }
        void Init();
    }
}