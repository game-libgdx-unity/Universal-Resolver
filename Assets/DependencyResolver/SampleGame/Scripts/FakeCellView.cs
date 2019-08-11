using UnityEngine;
using UnityIoC;

public class FakeCellView : MonoBehaviour, IDataBinding<FakeCellData>
{
    public void OnNext(FakeCellData t)
    {
        Debug.Log("Prefab Id from FakeCellData: " + t.id);
    }
}