/**
 * Author:    Vinh Vu Thanh
 * This class is a part of Unity IoC project that can be downloaded free at 
 * https://github.com/game-libgdx-unity/UnityEngine.IoC
 * (c) Copyright by MrThanhVinh168@gmail.com
 **/
using System;
using Newtonsoft.Json;

public static class ObjectExtensions
{
    /// <summary>
    /// The string representation of null.
    /// </summary>
    private static readonly string Null = "null";

    /// <summary>
    /// To json.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>The Json of any object.</returns>
    public static string ToJson(this object value)
    {
        if (value == null) return Null;

        try
        {
            return JsonConvert.SerializeObject(value);
        }
        catch (Exception exception)
        {
            //log exception but dont throw one
            return exception.Message;
        }
    }
    /// <summary>
    /// From json.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>The Json of any object.</returns>
    public static T FromJson<T>(this object value)
    {
        if (value == null) return default(T);

        try
        {
            return JsonConvert.DeserializeObject<T>((string)value);
        }
        catch (Exception exception)
        {
            //log exception but dont throw one
            return default(T);
        }
    }
}