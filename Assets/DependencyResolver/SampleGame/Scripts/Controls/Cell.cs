/**
 * Author:    Vinh Vu Thanh
 * This class is a part of Universal Resolver project that can be downloaded free at 
 * https://github.com/game-libgdx-unity/UnityEngine.IoC or http://u3d.as/1rQJ
 * (c) Copyright by MrThanhVinh168@gmail.com
 **/


using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityIoC;


/// <summary>
/// UI implementation for cells
/// </summary>
[SelectionBase]
public class Cell : MonoBehaviour
{
    
    //presentation layer
    [Inject] Text textUI;
    [Inject] Image background;
    [Inject] Outline outline;
    [Inject] CellData cellData;
    
    private void Start()
    {
        if (Context.ResolvedObjects.Count == 0)
        {
            return;
        }
        
        Debug.Assert(cellData != null);
        SetCellData(cellData);
        
        //find the last object
//        var cellData = Context.ResolvedObjects.Last(o => o.GetType() == typeof(CellData));
//        if (cellData != null)
//        {
//            SetCellData(cellData as CellData);
//        }
    }


    public void SetCellData(CellData data)
    {
        var cellData = data;
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
            if (isFlagged) cellData.CellType.Value = global::CellType.FLAGGED;
        });

        //when a cell is revealed
        cellData.IsRevealed.Subscribe(this, isRevealed =>
        {
            textUI.enabled = isRevealed;
            background.color = isRevealed ? Color.white : Color.gray;
            if (isRevealed)
            {
                cellData.IsMine.Subscribe(this, isMined =>
                {
                    if (isMined) cellData.CellType.Value = global::CellType.MINE;
                });
            }
        });

        //change UI when CellType change
        cellData.CellType.Subscribe(this, type =>
        {
            if (type == global::CellType.UNOPENED)
            {
                return;
            }

            textUI.color = Color.black;
            switch (type)
            {
                case global::CellType.EMPTY:
                    textUI.text = "";
                    break;
                case global::CellType.M1:
                    textUI.text = "1";
                    textUI.color = Color.blue;

                    break;
                case global::CellType.M2:
                    textUI.text = "2";
                    textUI.color = Color.cyan;

                    break;
                case global::CellType.M3:
                    textUI.text = "3";
                    textUI.color = Color.magenta;

                    break;
                case global::CellType.M4:
                    textUI.text = "4";
                    textUI.color = Color.blue;

                    break;
                case global::CellType.M5:
                    textUI.text = "5";

                    break;
                case global::CellType.M6:
                    textUI.text = "6";

                    break;
                case global::CellType.M7:
                    textUI.text = "7";

                    break;
                case global::CellType.M8:
                    textUI.text = "8";

                    break;
                case global::CellType.MINE:
                    textUI.text = "M";
                    textUI.color = Color.red;
                    textUI.enabled = true;

                    break;
                case global::CellType.FLAGGED:
                    textUI.text = "F";
                    textUI.color = Color.blue;
                    textUI.enabled = true;

                    break;
            }
        });
    }

    public void SetParent(Transform parent)
    {
        this.transform.SetParent(parent);
    }
}