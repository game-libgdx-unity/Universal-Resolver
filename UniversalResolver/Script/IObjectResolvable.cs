using System;
using UnityEngine;

/// <summary>
/// Be used when assemblyContext is processing inject attributes for all mono-behaviour
/// </summary>
public interface IObjectResolvable
{
    /// <summary>
    /// Try to get the needed object out of a MonoBehaviour, return null if not found
    /// </summary>
    /// <param name="behaviour">the MonoBehaviour</param>
    /// <param name="type">type of the component</param>
    /// <returns>the component</returns>
    object GetObject(MonoBehaviour behaviour, Type type);
}