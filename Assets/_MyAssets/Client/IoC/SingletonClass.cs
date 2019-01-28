#if UNITY_EDITOR


using SimpleIoc;
using UnityEngine;



[Binding(LifeCycle.Singleton)]
public class SingletonComponent : MonoBehaviour
{
    [Inject] public ISomeInterface PropertyAsInterface { get; set; }
    
    [Inject(LifeCycle.Singleton)] public ISomeInterface SingletonAsInterface { get; set; }
}

[Binding(LifeCycle.Singleton)]
public class SingletonClass
{
    [Inject] public ISomeInterface PropertyAsInterface { get; set; }
    [Inject(LifeCycle.Singleton)] public ISomeInterface SingletonAsInterface { get; set; }
}
public class TestClass
{    
    [Inject(LifeCycle.Singleton)] public ISomeInterface SingletonAsInterface { get; set; }
    [Inject(LifeCycle.Transient)] public ISomeInterface PropertyAsInterface { get; set; }
    [Inject] public SingletonDependency SingletonDependency { get; set; }
    
    [Inject(LifeCycle.Transient)] public SingletonDependency TransientSingletonDependency { get; set; }
}
public class TestComponent : MonoBehaviour
{
    [Inject(LifeCycle.Singleton)] public ISomeInterface SingletonAsInterface { get; set; }
    [Inject(LifeCycle.Transient)] public ISomeInterface PropertyAsInterface { get; set; }
}

[Binding(typeof(ISomeInterface), LifeCycle.Transient)]
public class Dependency : ISomeInterface
{
    public int SomeIntProperty { get; set; }
}

[Binding(LifeCycle.Singleton)]
public class SingletonDependency
{
    public int SomeIntProperty { get; set; }
}

[Binding(typeof(ISomeInterface), LifeCycle.Transient, typeof(TestComponent), typeof(SingletonComponent))]
public class AnotherDependency : ISomeInterface
{
    public int SomeIntProperty { get; set; }
}

public interface ISomeInterface
{
    int SomeIntProperty { get; set; }
}

#endif