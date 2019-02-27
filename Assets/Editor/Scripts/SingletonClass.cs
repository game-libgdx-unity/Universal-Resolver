//#if DEVELOPMENT_BUILD || UNITY_EDITOR
//
//using UnityEngine;
//
//namespace UnityIoC.Editor
//{
//    [Binding(LifeCycle.Singleton)]
//    public class SingletonComponent : MonoBehaviour
//    {
//        [Transient] public ISomeInterface PropertyAsInterface { get; set; }
//        [Singleton] public TestComponent SingletonTestComponent { get; set; }
//        [Singleton] public ISomeInterface SingletonAsInterface { get; set; }
//    }
//
//    public class ComponentAttributeTest : MonoBehaviour
//    {
//        [GetComponent] public TestComponent ComponentTest { get; set; }
//        [Transient] public TestComponent TransientComponentTest { get; set; }
//        [Singleton] public TestComponent SingletonComponentTest { get; set; }
//    }
//
//    [Binding(LifeCycle.Singleton)]
//    public class SingletonClass
//    {
//        [Inject] public ISomeInterface PropertyAsInterface { get; set; }
//    }
//
//    public class TestComponent : MonoBehaviour
//    {
//        [Singleton] public ISomeInterface SingletonAsInterface { get; set; }
//        [Transient] public ISomeInterface PropertyAsInterface { get; set; }
//    }
//
//
//    [Binding(LifeCycle.Singleton)]
//    public class SingletonDependency
//    {
//        public int SomeIntProperty { get; set; }
//    }
//
//    public class AnotherComponent : MonoBehaviour
//    {
//        [Transient] public ISomeComponentInterface TransientComponent { get; private set; }
//    }
//
//    public interface ISomeComponentInterface
//    {
//    }
//}
//#endif