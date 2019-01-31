using System;
using System.Collections;
using System.Collections.Generic;
using UnityIoC;
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
public class ComponentAttribute : InjectAttribute
{
    public ComponentAttribute()
        : base()
    {
    }
}
public class FindObjectOfTypeAttribute : InjectAttribute
{
    public FindObjectOfTypeAttribute()
        : base()
    {
    }
}
public class FindGameObjectByNameAttribute : InjectAttribute
{
    
    public FindGameObjectByNameAttribute(string name)
        : base()
    {
        Name = name;
    }

    public string Name { get; set; }
}
public class FindGameObjectsByTagAttribute : InjectAttribute
{
    
    public FindGameObjectsByTagAttribute(string name)
        : base()
    {
        Name = name;
    }

    public string Name { get; set; }
}