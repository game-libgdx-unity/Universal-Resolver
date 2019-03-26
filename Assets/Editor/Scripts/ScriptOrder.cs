/**
 * Author:    Vinh Vu Thanh
 * This class is a part of Universal Resolver project that can be downloaded free at 
 * https://github.com/game-libgdx-unity/UnityEngine.IoC
 * (c) Copyright by MrThanhVinh168@gmail.com
 **/
using System;

[AttributeUsage(AttributeTargets.Class)]
public class ScriptOrder : Attribute
{
    public ScriptOrder(int order)
    {
        this.order = order;
    }

    public int order;
}