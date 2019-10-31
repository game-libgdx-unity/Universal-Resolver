/**
 * Author:    Vinh Vu Thanh
 * This class is a part of Universal Resolver project that can be downloaded free at 
 * https://github.com/game-libgdx-unity/UnityEngine.IoC
 * (c) Copyright by MrThanhVinh168@gmail.com
 **/

using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityIoC;
using Object = UnityEngine.Object;

public static class ObjectExtension
{
    public static object WithId(this object obj, string id)
    {
        var context = Context.GetDefaultInstance(obj.GetType());
        Context.BindByName(obj, id);
        return obj;
    }
    
    public static object WithTags(this object obj, params string[] tags)
    {
        var context = Context.GetDefaultInstance(obj.GetType());
        Context.BindByTags(obj, tags);
        return obj;
    }
    
    public static T ResolveComponent<T>(this GameObject gameObject) where T : MonoBehaviour
    {
        var context = Context.GetDefaultInstance(typeof(T));
        return (T) context.DefaultContainer.ResolveObject(typeof(T), LifeCycle.Transient, gameObject);
    }
    
    public static T ResolveComponent<T>(this Component component) where T : MonoBehaviour
    {
        var context = Context.GetDefaultInstance(typeof(T));
        return (T) context.DefaultContainer.ResolveObject(typeof(T), LifeCycle.Transient, component);
    }

    public static T DefaultValue<T>(this T t)
    {
        return default(T);
    }

    public static T CopyComponent<T>(this T original, GameObject destination) where T : Component
    {
        Type type = original.GetType();
        var dst = destination.GetComponent(type) as T;
        if (!dst) dst = destination.AddComponent(type) as T;

        //Declare Binding Flags
        BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default
                             | BindingFlags.DeclaredOnly | BindingFlags.FlattenHierarchy;

        var fields = type.GetFields(flags);
        foreach (var field in fields)
        {
            if (field.IsStatic) continue;
            field.SetValue(dst, field.GetValue(original));
        }

        var props = type.GetProperties(flags);
        foreach (var prop in props)
        {
            if (!prop.CanWrite || !prop.CanWrite || prop.Name == "name") continue;
            prop.SetValue(dst, prop.GetValue(original, null), null);
        }

        return dst as T;
    }


    public static T Clone<T>(this Object source) where T : Object
    {
        if (source == null)
        {
            return default(T);
        }

        if (typeof(T).IsSubclassOf(typeof(Object)))
        {
            var clone = Object.Instantiate(source as Object);
            return (T) clone;
        }

        var serialized = JsonUtility.ToJson(source);
        return JsonUtility.FromJson<T>(serialized);
    }


    public static object Clone(this object source)
    {
        if (source == null)
        {
            return null;
        }

        if (source.GetType().IsSubclassOf(typeof(Object)))
        {
            return ((Object) source).Clone<Object>();
        }

        return JsonClone(source);
    }

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
        if (source == null)
        {
            return default(T);
        }

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
        if (objSource == null)
        {
            return default(T);
        }

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

    public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
    {
        return gameObject.GetComponent<T>() ?? gameObject.AddComponent<T>();
    }

    public static T GetOrAddComponent<T>(this Component component) where T : Component
    {
        return component.gameObject.GetOrAddComponent<T>();
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