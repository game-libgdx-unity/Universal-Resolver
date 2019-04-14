using System;
using UnityEngine;

/// <summary>
/// Be used when assemblyContext is processing inject attributes for all mono-behaviour
/// </summary>
public interface IComponentArrayResolvable
{
    /// <summary>
    /// Try to get the needed components out of a MonoBehaviour as array,
    /// return empty array if no element found
    /// </summary>
    /// <param name="behaviour">the MonoBehaviour</param>
    /// <param name="type">typf of the component</param>
    /// <returns>the component</returns>
    Component[] GetComponents(MonoBehaviour behaviour, Type type);
}