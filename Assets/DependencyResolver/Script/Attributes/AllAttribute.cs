using UnityIoC;

public class AllAttribute : InjectBaseAttribute
{
    public AllAttribute():base(
        LifeCycle.Default | LifeCycle.Cache | LifeCycle.Component | LifeCycle.Prefab | LifeCycle.Transient)
    {
    }
}