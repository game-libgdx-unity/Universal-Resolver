/**
 * Author:    Vinh Vu Thanh
 * This class is a part of Unity IoC project that can be downloaded free at 
 * https://github.com/game-libgdx-unity/UnityEngine.IoC
 * (c) Copyright by MrThanhVinh168@gmail.com
 **/
using System;
using UnityIoC;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Constructor |
                AttributeTargets.Field)]
public class InjectAttribute : Attribute
{
    public IContainer container { get; set; }
    public InjectAttribute(LifeCycle lifeCycle = LifeCycle.Default)
    {
        LifeCycle = lifeCycle;
    }

    public InjectAttribute():this(LifeCycle.Default)
    {
    }

    public LifeCycle LifeCycle { get; set; }
}

public class SingletonAttribute : InjectAttribute
{
    public SingletonAttribute()
        : base(LifeCycle.Singleton)
    {
    }
}
public class TransientAttribute : InjectAttribute
{
    public TransientAttribute()
        : base(LifeCycle.Transient)
    {
    }
}