using System;
using UnityIoC;

[Serializable]
public class PlayerData : IDataBinding<PlayerUI>
{
    public string name;
    public PlayerData(string name)
    {
        this.name = name;
    }
}