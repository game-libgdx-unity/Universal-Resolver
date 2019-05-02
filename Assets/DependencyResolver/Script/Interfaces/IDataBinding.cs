using System;

namespace UnityIoC
{
    /// <summary>
    /// Use to Bind a Data layer with a View Layer
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDataBinding<T>
    {
    }
    /// <summary>
    /// Use to Bind a View layer with a Data Layer
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDataView<T>
    {
        void OnNext(T t);
    }
}