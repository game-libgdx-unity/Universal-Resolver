/**
 * Author:    Vinh Vu Thanh
 * This class is a part of Universal Resolver project that can be downloaded free at 
 * https://github.com/game-libgdx-unity/UnityEngine.IoC or http://u3d.as/1rQJ
 * (c) Copyright by MrThanhVinh168@gmail.com
 **/


using System;
using UnityEngine;
using UnityEngine.UI;
using UnityIoC;

/// <summary>
/// UI implementation for cells
/// </summary>
[SelectionBase]
public class CellView : MonoBehaviour, IDataBinding<CellData>
{
    //presentation layer
    [SerializeField] Text textUI;
    [SerializeField] Image background;
    [SerializeField] Outline outline;

//    [Inject("/MapCanvas")] private MapGenerator mapGenerator;
    public void OnNext(CellData data)
    {
        var cellData = data;

        Context.Delete<CellData>(data);


        Debug.Assert(cellData != null);
        outline.effectColor = Color.black;

        //when number of adjacent mines get changed
        cellData.AdjacentMines.Subscribe(this, mines =>
        {
            if (mines > 0) cellData.CellType.Value = (CellType) mines;
        });

        //when a cell is flagged
        cellData.IsFlagged.Subscribe(this, isFlagged =>
        {
            if (isFlagged) cellData.CellType.Value = CellType.FLAGGED;
        });

        //when a cell is revealed
        cellData.IsOpened.Subscribe(this, isOpened =>
        {
            textUI.enabled = isOpened;
            background.color = isOpened ? Color.white : Color.gray;
            if (isOpened)
            {
                //when the opened cell is determined that it's a mines
                cellData.IsMine.Subscribe(this, isMined =>
                {
                    if (isMined) cellData.CellType.Value = CellType.MINE;
                });
            }
        });

        //change UI when CellType change
        cellData.CellType.Subscribe(this, type =>
        {
            if (type == CellType.UNOPENED)
            {
                return;
            }

            textUI.color = Color.black;
            switch (type)
            {
                case CellType.EMPTY:
                    textUI.text = "";
                    break;
                case CellType.M1:
                    textUI.text = "1";
                    textUI.color = Color.blue;

                    break;
                case CellType.M2:
                    textUI.text = "2";
                    textUI.color = Color.cyan;

                    break;
                case CellType.M3:
                    textUI.text = "3";
                    textUI.color = Color.magenta;

                    break;
                case CellType.M4:
                    textUI.text = "4";
                    textUI.color = Color.blue;

                    break;
                case CellType.M5:
                    textUI.text = "5";

                    break;
                case CellType.M6:
                    textUI.text = "6";

                    break;
                case CellType.M7:
                    textUI.text = "7";

                    break;
                case CellType.M8:
                    textUI.text = "8";

                    break;
                case CellType.MINE:
                    textUI.text = "M";
                    textUI.color = Color.red;
                    textUI.enabled = true;

                    break;
                case CellType.FLAGGED:
                    textUI.text = "F";
                    textUI.color = Color.blue;
                    textUI.enabled = true;

                    break;
            }
        });
    }
}