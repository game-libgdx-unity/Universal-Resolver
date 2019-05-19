using System;
using UTJ;

namespace UnityIoC
{
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