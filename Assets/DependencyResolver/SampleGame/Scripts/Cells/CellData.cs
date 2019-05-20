using UnityIoC;

public class CellData : IViewBinding<Cell>
{
    public int ID { get; set; }
    public int X { get; set; }
    public int Y { get; set; }

    public Observable<CellType> CellType = new Observable<CellType>();
    public IntReactiveProperty AdjacentMines = new IntReactiveProperty();
    public BoolReactiveProperty IsRevealed = new BoolReactiveProperty();
    public BoolReactiveProperty IsMine = new BoolReactiveProperty();
    public BoolReactiveProperty IsFlagged = new BoolReactiveProperty();

    public CellData(int id, int x, int y)
    {
        ID = id;
        X = x;
        Y = y;
    }
}