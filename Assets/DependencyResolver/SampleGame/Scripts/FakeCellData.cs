using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
