
#pragma warning disable 1587
/// \file
/// <summary>Reimplements a RPC Attribute, as it's no longer in all versions of the UnityEngine assembly.</summary>
#pragma warning restore 1587

/**
 * Author:    Vinh Vu Thanh
 * This class is a part of Unity IoC project that can be downloaded free at 
 * https://github.com/game-libgdx-unity/UnityEngine.IoC
 * (c) Copyright by MrThanhVinh168@gmail.com
 **/
using System;

/// <summary>Replacement for RPC attribute with different name. Used to flag methods as remote-callable.</summary>
public class PunRPC : Attribute
{
}