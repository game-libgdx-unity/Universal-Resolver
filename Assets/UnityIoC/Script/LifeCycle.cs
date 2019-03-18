using System;

namespace UnityIoC
{
   [Flags]
    public enum LifeCycle : byte
    {
        Default = 0,
        Transient = 1 << 0,
        Singleton = 1 << 1,
        Component = 1 << 2,
        SingletonComponent = Singleton | Component
    }

    public static class LifeCycleExtension
    {
        public static bool IsEqual(this LifeCycle first, LifeCycle second)
        {
            const byte max = (byte) LifeCycle.Singleton;
            var firstAsByte = (byte) first;
            var secondAsByte = (byte) second;

            if (firstAsByte < 2)
                firstAsByte = 1;
            
            if (secondAsByte < 2)
                secondAsByte = 1;

            return firstAsByte.Equals(secondAsByte);
        }
    }
}