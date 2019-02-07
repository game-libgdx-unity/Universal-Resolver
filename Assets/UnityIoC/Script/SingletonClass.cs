#if UNITY_EDITOR

 
using UnityEngine;

namespace UnityIoC
{


    [Binding(LifeCycle.Singleton)]
    public class SingletonComponent : MonoBehaviour
    {
        [Transient] public ISomeInterface PropertyAsInterface { get; set; }
        [Singleton] public TestComponent SingletonTestComponent { get; set; }
        [Singleton] public ISomeInterface SingletonAsInterface { get; set; }
    }

    public class ComponentAttributeTest : MonoBehaviour
    {
        [Component] public TestComponent ComponentTest { get; set; }
        [Transient] public TestComponent TransientComponentTest { get; set; }
        [Singleton] public TestComponent SingletonComponentTest { get; set; }
    }

    [Binding(LifeCycle.Singleton)]
    public class SingletonClass
    {
        [Inject] public ISomeInterface PropertyAsInterface { get; set; }
    }

    public class TestComponent : MonoBehaviour
    {
        [Inject(LifeCycle.Singleton)] public ISomeInterface SingletonAsInterface { get; set; }
        [Inject(LifeCycle.Transient)] public ISomeInterface PropertyAsInterface { get; set; }
    }


    [Binding(LifeCycle.Singleton)]
    public class SingletonDependency
    {
        public int SomeIntProperty { get; set; }
    }

}
#endif