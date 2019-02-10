/**
 * Author:    Vinh Vu Thanh
 * This class is a part of Unity IoC project that can be downloaded free at 
 * https://github.com/game-libgdx-unity/UnityEngine.IoC
 * (c) Copyright by MrThanhVinh168@gmail.com
 **/
using System;
using UnityEngine;

public interface IComponentResolvable
{
    /// <summary>
    /// Try to get the component out of a MonoBehaviour, return null if not found
    /// </summary>
    /// <param name="behaviour">the MonoBehaviour</param>
    /// <param name="type">typf of the component</param>
    /// <returns>the component</returns>
    Component GetComponent(MonoBehaviour behaviour, Type type);
}
public interface IComponentArrayResolvable
{
    /// <summary>
    /// Try to get the component out of a MonoBehaviour, return null if not found
    /// </summary>
    /// <param name="behaviour">the MonoBehaviour</param>
    /// <param name="type">typf of the component</param>
    /// <returns>the component</returns>
    Component[] GetComponents(MonoBehaviour behaviour, Type type);
}