using UnityIoC;

public interface IUpdatableItem<T> where T : IUpdatable, IUpdatableItem<T>, new()
{
    /// <summary>
    /// Parent group that this item belongs to
    /// </summary>
    UpdatableGroup<T> Group { get; set; }
}