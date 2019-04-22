using UnityIoC;

public class CacheAttribute : InjectAttribute
{
    public CacheAttribute()
        : base(LifeCycle.Cache, null)
    {
    }
}