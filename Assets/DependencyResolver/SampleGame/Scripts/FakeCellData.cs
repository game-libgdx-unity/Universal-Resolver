using System;
using System.Collections;
using System.Collections.Generic;
using UnityIoC;

public class FakeCellData : IViewBinding<FakeCellView>, IBindByID
{
    public int id;

    public FakeCellData(int id)
    {
        this.id = id;
    }

    public object GetID()
    {
        return id;
    }
}

public class FakeCellDataRx : IViewBinding<ScriptableDataCellView>, IBindByID
{
    public IntReactiveProperty id = new IntReactiveProperty();

    public FakeCellDataRx(int id)
    {
        this.id.Value = id;
    }

    public object GetID()
    {
        return id.Value;
    }
}