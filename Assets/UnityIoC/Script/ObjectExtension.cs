/**
 * Author:    Vinh Vu Thanh
 * This class is a part of Unity IoC project that can be downloaded free at 
 * https://github.com/game-libgdx-unity/UnityEngine.IoC
 * (c) Copyright by MrThanhVinh168@gmail.com
 **/

using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class ObjectExtension

{
    public static T BinaryClone<T>(this T objSource)

    {
        if (objSource == null)
        {
            return default(T);
        }

        using (MemoryStream stream = new MemoryStream())
        {
            BinaryFormatter formatter = new BinaryFormatter();

            formatter.Serialize(stream, objSource);

            stream.Position = 0;

            return (T) formatter.Deserialize(stream);
        }
    }
    
    public static T JsonClone<T>(this T source)
    {
        var serialized = JsonUtility.ToJson(source);
        return JsonUtility.FromJson<T>(serialized);
    }

    private static Array ConvertArrayTo(Type typeOfArray, object[] components)
    {
        var array = Array.CreateInstance(typeOfArray, components.Length);
        for (var i = 0; i < components.Length; i++)
        {
            var component = components[i];
            array.SetValue(component, i);
        }

        return array;
    }
    
    public static T DeepCloneObject<T>(this T objSource)

    {
        //Get the type of source object and create a new instance of that type

        Type typeSource = objSource.GetType();

        object objTarget = null;

        if (typeSource.IsArray)
        {
            var sourceArray = objSource as Array;
        }
        else
        {
            objTarget = Activator.CreateInstance(typeSource);
        }

        FieldInfo[] fieldInfos =
            typeSource.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);


        //Assign all source property to taget object 's fields

        foreach (FieldInfo field in fieldInfos)

        {
            //check whether property type is value type, enum or string type

            if (field.FieldType.IsValueType || field.FieldType.IsEnum ||
                field.FieldType.Equals(typeof(System.String)))

            {
                field.SetValue(objTarget, field.GetValue(objSource));
            }

            //else property type is object/complex types, so need to recursively call this method until the end of the tree is reached

            else

            {
                object objFieldValue = field.GetValue(objSource);

                if (objFieldValue == null)

                {
                    field.SetValue(objTarget, null);
                }

                else

                {
                    field.SetValue(objTarget, objFieldValue.DeepCloneObject());
                }
            }
        }

        return (T) objTarget;
    }
}

//Get all the properties of source object type

//        PropertyInfo[] propertyInfo =
//            typeSource.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);


//Assign all source property to taget object 's properties
//
//        foreach (PropertyInfo property in propertyInfo)
//
//        {
//            //Check whether property can be written to
//
//            if (property.CanWrite)
//
//            {
//                //check whether property type is value type, enum or string type
//
//                if (property.PropertyType.IsValueType || property.PropertyType.IsEnum ||
//                    property.PropertyType.Equals(typeof(System.String)))
//
//                {
//                    property.SetValue(objTarget, property.GetValue(objSource, null), null);
//                }
//
//                //else property type is object/complex types, so need to recursively call this method until the end of the tree is reached
//
//                else
//
//                {
//                    object objPropertyValue = property.GetValue(objSource, null);
//
//                    if (objPropertyValue == null)
//
//                    {
//                        property.SetValue(objTarget, null, null);
//                    }
//
//                    else
//
//                    {
//                        property.SetValue(objTarget, objPropertyValue.CloneObject(), null);
//                    }
//                }
//            }
//        } 
        
//Get all the properties of source object type