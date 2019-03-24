using System;

namespace UnityIoC
{
   [Flags]
    public enum LifeCycle : byte
    {
        Default = 1 << 0,
        Transient = 1 << 1,
        Singleton = 1 << 2,
        Component = 1 << 3,
        Prefab = 1 << 4,
        SingletonComponent = Singleton | Component
    }

    public static class LifeCycleExtension
    {
        public static bool IsEqual(this LifeCycle first, LifeCycle second)
        {
            var firstAsByte = (byte) first;
            var secondAsByte = (byte) second;

            if (firstAsByte < (byte) LifeCycle.Singleton)
                firstAsByte = (byte) LifeCycle.Transient;
            
            if (secondAsByte < (byte) LifeCycle.Singleton)
                secondAsByte = (byte) LifeCycle.Transient;

            return firstAsByte.Equals(secondAsByte);
        }
    }
}