namespace UnityIoC
{
    public interface IPoolable
    {
        bool Alive { get; set; }
        void OnRecycle();
    }
}