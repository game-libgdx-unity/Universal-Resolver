﻿﻿/**
 * Author:    Vinh Vu Thanh
 * This class is a part of Universal Resolver project that can be downloaded free at 
 * https://github.com/game-libgdx-unity/UnityEngine.IoC or http://u3d.as/1rQJ
 * (c) Copyright by MrThanhVinh168@gmail.com
 **/
using System;
using System.Collections.Generic;

namespace App.Scripts.Boards
{
    public interface IGameBoard : IBoard
    {
        void Build(int width, int height, int mines);
        CellData GetCellAt(int firstMoveX, int firstMoveY);
        void Open(int randomX, int randomY);
        List<CellData> GetNeighbors(int x, int y);
       
        void FirstMove(int randomX, int randomY, Random random);
        void Flag(int neighborX, int neighborY);
        void Build();
    }
}