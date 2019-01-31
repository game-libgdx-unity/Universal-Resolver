using UnityEngine;
using System;
namespace App.Scripts.Boards
{
    [Serializable]
    public class GameSetting
    {
        public int Width = 9;
        public int Height = 9;
        public int MineCount = 10;
    }
}