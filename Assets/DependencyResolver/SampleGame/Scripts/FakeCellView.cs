using UnityEngine;
using UnityIoC;

public class FakeCellView : MonoBehaviour, IDataBinding<FakeCellData, FakeCellScriptableObject>
{
    public void OnNext(FakeCellData t)
    {
        Debug.Log("Prefab Id from FakeCellData: " + t.id);
    }

    public void OnNext(FakeCellScriptableObject t)
    {
        
        Debug.Log(t.id);
        Debug.Log(t.backgroundColor);
        Debug.Log(t.textColor);
        
            Debug.Log("Prefab Id from FakeCellScriptableObject: " + t.id);
    }
}