using UnityEngine;
using UnityIoC;

[CreateAssetMenu]
public class FakeCellScriptableObject : ScriptableObject, IViewBinding<ScriptableDataCellView>, IBindByID
{
    public int id;

    public Color textColor;
    public Color backgroundColor;
    
    public object GetID()
    {
        return id;
    }
}