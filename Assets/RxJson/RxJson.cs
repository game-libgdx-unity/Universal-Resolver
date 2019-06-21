using System;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

static internal class RxJson
{
    public static T RxFromJson<T>(string json)
    {
        Type type = typeof(T);

        T output = Activator.CreateInstance<T>();
        var data = JObject.Parse(json);
        foreach (var property in data.Properties())
        {
            object value = null;
            var field = type.GetField(property.Name, BindingFlags.Public | BindingFlags.Instance);

            if (field.FieldType.Assembly.GetName().Name != NewBehaviourScript.UniRxAssemblyName)
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
}