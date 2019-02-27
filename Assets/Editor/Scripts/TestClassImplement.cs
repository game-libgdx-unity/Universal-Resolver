#if UNITY_EDITOR

namespace UnityIoC.Editor
{
    public class TestClass2
    {
        [Inject] public ISomeInterface SomeInterface { get; set; }
        [Inject] public ISomeInterface PropertyAsInterface { get; set; }
    }

    [Binding(typeof(ISomeInterface), LifeCycle.Default, typeof(TestClass2))]
    public class TestClassImplement : ISomeInterface
    {
        public int SomeIntProperty { get; set; }
    }

    public class AnotherTestClass
    {
        [Inject] public ISomeInterface SomeInterface { get; set; }
    }


    [Binding(typeof(ISomeInterface), LifeCycle.Default, typeof(AnotherTestClass))]
    public class AnotherImplement : ISomeInterface
    {
        public int SomeIntProperty { get; set; }
    }


    public class JustATestClass
    {
        [Inject] public ISomeInterface SomeInterface { get; set; }
    }

    public class DefaultImplement : ISomeInterface
    {
        public int SomeIntProperty { get; set; }
    }


    public class DefaultSomethingImplement : ISomeThing
    {
        public int SomeIntProperty { get; set; }
    }

    public interface ISomeThing
    {
        int SomeIntProperty { get; set; }
    }
}

#endif