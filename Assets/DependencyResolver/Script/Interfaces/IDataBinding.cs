namespace UnityIoC
{
    /// <summary>
    /// Use to Bind a View layer with a Data Layer
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDataBinding<T>
    {
        void OnNext(T t);
    }
    
    /// <summary>
    /// Use to Bind a View layer with multiple data Layers
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDataView<T1, T2, T3, T4, T5>
    {
        void OnNext(T1 t);
        void OnNext(T2 t);
        void OnNext(T3 t);
        void OnNext(T4 t);
        void OnNext(T5 t);
    }
    
    /// <summary>
    /// Use to Bind a View layer with multiple data Layers
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDataView<T1, T2, T3, T4>
    {
        void OnNext(T1 t);
        void OnNext(T2 t);
        void OnNext(T3 t);
        void OnNext(T4 t);
    }
    
    /// <summary>
    /// Use to Bind a View layer with multiple data Layers
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDataView<T1, T2, T3>
    {
        void OnNext(T1 t);
        void OnNext(T2 t);
        void OnNext(T3 t);
    }
    
    /// <summary>
    /// Use to Bind a View layer with multiple data Layers
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDataView<T1, T2>
    {
        void OnNext(T1 t);
        void OnNext(T2 t);
    }
}