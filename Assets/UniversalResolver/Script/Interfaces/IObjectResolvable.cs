using System;
using UnityEngine;

/// <summary>
/// Should be implemented in mono behaviour to get a non-component when processing mono-behaviour
/// </summary>
public interface IObjectResolvable
{
    /// <summary>
    /// Try to get the needed object out of a MonoBehaviour, return null if not found
    /// </summary>
    /// <param name="type">type of the component</param>
    /// <returns>the component</returns>
    object GetObject(Type type);
}

/// <summary>
/// Should be implemented in mono behaviour to get a non-component when processing mono-behaviour
/// </summary>
public interface IObjectResolvable<T>
{
    /// <summary>
    /// Try to get the needed object out of a MonoBehaviour, return null if not found
    /// </summary>
    /// <param name="type">type of the component</param>
    /// <returns>the component</returns>
    T GetObject();
}