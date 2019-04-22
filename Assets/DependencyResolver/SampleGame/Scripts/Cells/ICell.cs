/**
 * Author:    Vinh Vu Thanh
 * This class is a part of Universal Resolver project that can be downloaded free at 
 * https://github.com/game-libgdx-unity/UnityEngine.IoC
 * (c) Copyright by MrThanhVinh168@gmail.com
 **/

using UnityEngine;
using UnityEngine.EventSystems;

public interface ICell
{
    void SetParent(Transform parent);
    void SetCellData(CellData cellData);
}