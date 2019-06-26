using System;
using UnityEngine;
using UniRx;

public class NewBehaviourScript : MonoBehaviour
{
    void Start()
    {
        var json =
            "{\"testInt\": 5,\"anEnum\":1,\"aData\":{\"property\":7},\"property\": 10,\"property3\": 10,\"Property4\":{\"property\":5},\"Property5\":1}";

        UniRxData d = RxJson.FromJson<UniRxData>(json);
        Debug.Log(d.testInt);
        Debug.Log(d.aData.property);
        Debug.Log(d.anEnum);
        Debug.Log(d.property.Value);
        Debug.Log(d.property3.Value);
        Debug.Log(d.Property4.Value.property);

        d.property2 = new UniRx.IntReactiveProperty(5);
//        d.aData = new Data(){property = 7};
        var j = RxJson.ToJson(d);
        Debug.Log(j);

        var d2 = RxJson.FromJson<NonRxData>(j);
        Debug.Log(d2.testInt);
        Debug.Log(d2.aData.property);
        Debug.Log(d2.anEnum);
        Debug.Log(d2.property);
        Debug.Log(d2.property3);
        Debug.Log(d2.Property4.property);

//   not work in Unity:
//        dynamic dynamicObj = JObject.Parse(json);
//        Debug.Log(dynamicObj.property);
    }
}


[Serializable]
public class Data
{
    public int property;
}

[Serializable]
public class UniRxData
{
    public int testInt;
    public Data aData;
    public TestEnum anEnum;

    public UniRx.IntReactiveProperty property;
    public UniRx.ReactiveProperty<int> property3;
    public UniRx.ReactiveProperty<Data> Property4;
    public UniRx.ReactiveProperty<TestEnum> Property5;

    [NonSerialized] public UniRx.IntReactiveProperty property2;
}

[Serializable]
public class NonRxData
{
    public int testInt;
    public Data aData;
    public TestEnum anEnum;

    public int property;
    public int property3;
    public Data Property4;
    public TestEnum Property5;

    [NonSerialized] public UniRx.IntReactiveProperty property2;
}

public enum TestEnum
{
    Default,
    One
}