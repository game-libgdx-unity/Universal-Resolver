using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TMPro.EditorUtilities;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    void Start()
    {
        var json =
            "{\"testInt\": 5,\"anEnum\":1,\"aData\":{\"property\":7},\"property\": 10,\"property3\": 10,\"Property4\":{\"property\":5},\"Property5\":1}";

        UniRxData d = RxFromJson<UniRxData>(json);
        Debug.Log(d.testInt);
        Debug.Log(d.aData.property);
        Debug.Log(d.anEnum);
        Debug.Log(d.property.Value);
        Debug.Log(d.property3.Value);
        Debug.Log(d.Property4.Value.property);

        d.property2 = new UniRx.IntReactiveProperty(5);
//        d.aData = new Data(){property = 7};
        var j = RxToJson(d);
        Debug.Log(j);

        var d2 = RxFromJson<NonRxData>(j);
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

    private const string UniRxAssemblyName = "UniRx";

    public static T RxFromJson<T>(string json)
    {
        Type type = typeof(T);

        T output = Activator.CreateInstance<T>();
        var data = JObject.Parse(json);
        foreach (var property in data.Properties())
        {
            object value = null;
            var field = type.GetField(property.Name, BindingFlags.Public | BindingFlags.Instance);

            if (field.FieldType.Assembly.GetName().Name != UniRxAssemblyName)
            {
                value = JsonConvert.DeserializeObject(property.Value.ToString(), field.FieldType);
            }
            else if (field.FieldType.IsGenericType &&
                     field.FieldType.GetGenericTypeDefinition() == typeof(UniRx.ReactiveProperty<>))
            {
                var argType = field.FieldType.GetGenericArguments().FirstOrDefault();
                var isRawType = argType.IsValueType || argType is string;
                var isEnum = argType.IsEnum;

                value = isEnum
                    ? Activator.CreateInstance(field.FieldType, Enum.Parse(argType, property.Value.ToString()))
                    : isRawType
                        ? Activator.CreateInstance(field.FieldType, property.ToObject(argType))
                        : Activator.CreateInstance(field.FieldType,
                            JsonConvert.DeserializeObject(property.Value.ToString(), argType));
            }
            else if (field.FieldType == typeof(UniRx.StringReactiveProperty))
            {
                value = new UniRx.StringReactiveProperty((string) property.Value);
            }
            else if (field.FieldType == typeof(UniRx.FloatReactiveProperty))
            {
                value = new UniRx.FloatReactiveProperty((float) property.Value);
            }
            else if (field.FieldType == typeof(UniRx.IntReactiveProperty))
            {
                value = new UniRx.IntReactiveProperty((int) property.Value);
            }
            else if (field.FieldType == typeof(UniRx.BoolReactiveProperty))
            {
                value = new UniRx.BoolReactiveProperty((bool) property.Value);
            }
            else if (field.FieldType == typeof(UniRx.DateTimeReactiveProperty))
            {
                value = new UniRx.DateTimeReactiveProperty((DateTime) property.Value);
            }
            else if (field.FieldType == typeof(UniRx.DoubleReactiveProperty))
            {
                value = new UniRx.DoubleReactiveProperty((double) property.Value);
            }
            else if (field.FieldType == typeof(UniRx.LongReactiveProperty))
            {
                value = new UniRx.LongReactiveProperty((long) property.Value);
            }
            else if (field.FieldType == typeof(UniRx.Vector3ReactiveProperty))
            {
                value = new UniRx.Vector3ReactiveProperty(property.ToObject<Vector3>());
            }
            else if (field.FieldType == typeof(UniRx.Vector2ReactiveProperty))
            {
                value = new UniRx.Vector2ReactiveProperty(property.ToObject<Vector2>());
            }

            field.SetValue(output, value);
        }

        return output;
    }

    public static string RxToJson<T>(T obj)
    {
        var type = typeof(T);
        JObject jObject = new JObject();

        foreach (var field in type.GetFields()
            .Where(t => !t.GetCustomAttributes(typeof(NonSerializedAttribute), true).Any()))
        {
            if (field.FieldType.Assembly.GetName().Name != UniRxAssemblyName)
            {
                jObject.Add(field.Name, JsonConvert.SerializeObject(field.GetValue(obj), Formatting.None));
                continue;
            }

            var property = field.GetValue(obj);
            var value = field.FieldType.IsGenericType
                ? field.FieldType.GetField("value", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(property)
                : field.FieldType.BaseType.GetField("value", BindingFlags.NonPublic | BindingFlags.Instance)
                    .GetValue(property);

            jObject.Add(field.Name, JToken.FromObject(value));
        }

        return jObject.ToString(Formatting.None);
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