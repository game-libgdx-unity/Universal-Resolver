using System;
using UnityIoC;

[Serializable]
public class PlayerData : SimpleObservable<PlayerData>, IDataBinding<PlayerUI>
{
    public string name;
    public PlayerData(string name)
    {
        this.name = name;
    }
}