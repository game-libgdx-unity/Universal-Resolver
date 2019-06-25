using System;

namespace UnityIoC
{
    /// <summary>
    /// Bind a view by a given id
    /// </summary>
    public interface IBindByID
    {
        object GetID();
    }
    /// <summary>
    /// Use to Bind a Data layer with a View Layer
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IViewBinding<T>
    {
    }
    /// <summary>
    /// Use to Bind a Data layer with multiple View Layers
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IViewBinding<T1, T2, T3, T4, T5>
    {
    }
    /// <summary>
    /// Use to Bind a Data layer with multiple View Layers
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IViewBinding<T1, T2, T3, T4>
    {
    }
    /// <summary>
    /// Use to Bind a Data layer with multiple View Layers
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IViewBinding<T1, T2, T3>
    {
    }
    /// <summary>
    /// Use to Bind a Data layer with multiple View Layers
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IViewBinding<T1, T2>
    {
    }
}