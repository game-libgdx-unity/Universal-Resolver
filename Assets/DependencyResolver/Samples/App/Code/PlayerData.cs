using System;
using UnityIoC;

[Serializable]
public class PlayerData : IDataBinding<PlayerUI>
{
    public MetaData meta;

    public Combat combat;

    [Serializable]
    public class MetaData
    {
        public string Id;
        public string DisplayName;
        public string Genre;
        public string Rank;
        public int Level;
        public int CurrentExp;
        public int MaxExp;
    }

    [Serializable]
    public class Combat
    {
        public int Hitpoint;
        public int MaxHitpoint;
        public int PhysicDamage;
        public int Amor;
        public int MagicResistent;
    }

    public PlayerData(string name)
    {
        if (meta == null)
        {
            meta = new MetaData();
        }
        
        this.meta.DisplayName = name;
    }
}