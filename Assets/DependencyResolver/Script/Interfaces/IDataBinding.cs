using System;

namespace UnityIoC
{
    public interface IDataBinding<T>
    {
    }
    public interface IDataView<T>
    {
        void OnNext(T t);
    }
}