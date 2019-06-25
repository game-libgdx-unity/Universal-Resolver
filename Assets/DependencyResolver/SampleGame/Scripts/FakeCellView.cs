using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityIoC;

public class FakeCellView : MonoBehaviour, IDataBinding<FakeCellData>
{
    public void OnNext(FakeCellData t)
    {
        Debug.Log("Fake cell id: " + t.id);
    }
}