using System;
using System.Collections;
using System.Collections.Generic;
using SimpleIoc;
using UnityEngine;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Constructor |
                AttributeTargets.Field)]
public class InjectAttribute : Attribute
{
    public InjectAttribute(LifeCycle lifeCycle, string prefabToCreateGameObject)
    {
        PrefabToCreateGameObject = prefabToCreateGameObject;
        LifeCycle = lifeCycle;
    }

    public string PrefabToCreateGameObject { get; set; }

    public InjectAttribute(LifeCycle lifeCycle = LifeCycle.Default)
    {
        this.LifeCycle = lifeCycle;
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