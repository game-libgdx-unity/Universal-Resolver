using UnityIoC;

public class SingletonAttribute : InjectAttribute
{
    public SingletonAttribute(string path = null)
        : base(LifeCycle.Singleton, path)
    {
    }

    public SingletonAttribute()
        : base(LifeCycle.Singleton)
    {
    }
}