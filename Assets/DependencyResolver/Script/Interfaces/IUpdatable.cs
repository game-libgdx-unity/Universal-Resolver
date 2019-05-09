namespace UnityIoC
{
    /// <summary>
    /// Allow objects to be updated in SystemManager
    /// </summary>
    public interface IUpdatable
    {
        bool Enable { get; set; }
        void Update(float delta_time, float game_time);
    }
}