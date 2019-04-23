using UnityIoC;

public class AllAttribute : InjectAttribute
{
    public AllAttribute():base(
        LifeCycle.Default | LifeCycle.Cache | LifeCycle.Component | LifeCycle.Prefab | LifeCycle.Transient)
    {
    }
}