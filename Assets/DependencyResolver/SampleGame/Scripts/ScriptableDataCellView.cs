using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityIoC;

public class ScriptableDataCellView : MonoBehaviour, IDataBinding< FakeCellScriptableObject>
{
    public void OnNext(FakeCellScriptableObject t)
    {
        Debug.Log("Prefab Id from scriptableObject: " + t.id);

        var backgroundColor = t.backgroundColor;
        backgroundColor.a = 1;
        GetComponent<Image>().color = backgroundColor;

        var textColor = t.textColor;
        textColor.a = 1;
        GetComponentInChildren<Text>().color = textColor;
    }
}