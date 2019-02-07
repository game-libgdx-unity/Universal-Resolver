namespace UnityIoC
{
    public enum LifeCycle : byte
    {
        Default,
        Transient,
        Singleton,
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