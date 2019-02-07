#if UNITY_EDITOR

namespace UnityIoC
{
    public class TestClass
    {
        [Inject] public ISomeInterface SomeInterface { get; set; }
        [Inject] public ISomeInterface PropertyAsInterface { get; set; }
    }

    [Binding(typeof(ISomeInterface), LifeCycle.Default, typeof(TestClass))]
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