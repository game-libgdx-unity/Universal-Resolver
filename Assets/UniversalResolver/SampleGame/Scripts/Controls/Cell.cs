/**
 * Author:    Vinh Vu Thanh
 * This class is a part of Universal Resolver project that can be downloaded free at 
 * https://github.com/game-libgdx-unity/UnityEngine.IoC or http://u3d.as/1rQJ
 * (c) Copyright by MrThanhVinh168@gmail.com
 **/


using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// UI implementation for cells
/// </summary>
[SelectionBase]
public class Cell : MonoBehaviour, ICell
{
    private Observable<CellType> CellType = new Observable<CellType>();

    private CellData cellData { get; set; }
    [Children] Text textUI;
    [Component] Image background;
    [Component] Outline outline;

    public void SetCellData(CellData data)
    {
        Debug.Assert(data != null);
        this.cellData = data;

        outline.effectColor = Color.black;

        //when number of adjacent mines get changed
        cellData.AdjacentMines.SubscribeToComponent(this, mines =>
        {
            if (mines > 0) CellType.Value = (CellType) mines;
        });

        //when a cell is flagged
        cellData.IsFlagged.SubscribeToComponent(this, isFlagged =>
        {
            if (isFlagged) CellType.Value = global::CellType.FLAGGED;
        });

        //when a cell is revealed
        cellData.IsRevealed.SubscribeToComponent(this, isRevealed =>
        {
            textUI.enabled = isRevealed;
            background.color = isRevealed ? Color.white : Color.gray;
            if (isRevealed)
            {
                cellData.IsMine.SubscribeToComponent(this, isMined =>
                {
                    if (isMined) CellType.Value = global::CellType.MINE;
                });
            }
        });

        //change UI when CellType change
        CellType.SubscribeToComponent(this, type =>
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