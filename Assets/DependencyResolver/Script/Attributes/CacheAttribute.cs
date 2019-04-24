using UnityIoC;

public class CacheAttribute : InjectBaseAttribute
{
    public CacheAttribute()
        : base(LifeCycle.Cache, null)
    {
    }
}