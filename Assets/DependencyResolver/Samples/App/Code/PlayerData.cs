using System;
using UnityIoC;

[Serializable]
public class PlayerData  : IDataBinding<PlayerDataUI>
{
    public PlayerData(string name)
    {
        Name = name;
    }

    public string Name;
}