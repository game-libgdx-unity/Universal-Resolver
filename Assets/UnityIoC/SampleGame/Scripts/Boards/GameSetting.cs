using UnityEngine;
/**
 * Author:    Vinh Vu Thanh
 * This class is a part of Unity IoC project that can be downloaded free at 
 * https://github.com/game-libgdx-unity/UnityEngine.IoC
 * (c) Copyright by MrThanhVinh168@gmail.com
 **/
using System;
namespace App.Scripts.Boards
{
    [Serializable]
    public class GameSetting
    {
        public int Width = 10;
        public int Height = 10;
        public int MineCount = 9;
    }
}