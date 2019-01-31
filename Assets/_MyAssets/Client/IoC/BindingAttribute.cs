using System;
using SimpleIoc;
using UnityEngine;

[Serializable]
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Constructor |
                AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
public class BindingAttribute : Attribute
{
    public Type TypeToResolve { get; private set; }
    public LifeCycle LifeCycle { get; set; }
    public Type[] InjectInto { get; set; }
    public Type[] ContainComponents { get; set; }
    public string[] GameObjectNames { get; set; }
    public string[] SceneNames { get; set; }
    
    public string[] Tags { get; set; }
    
    public int[] Layers { get; set; }
    
    internal BindingAttribute(Type typeToResolve, 
        LifeCycle lifeCycle = LifeCycle.Default, 
        Type[] injectInto = null, 
        Type[] containComponents = null, 
        string[] gameObjectNames = null,
        string[] sceneNames = null,
        string[] tags = null,
        int[] layers = null
        )
    {
        TypeToResolve = typeToResolve;
        GameObjectNames = gameObjectNames;
        SceneNames = sceneNames;
        Tags = tags;
        Layers = layers;
        LifeCycle = lifeCycle;
        InjectInto = injectInto;
        ContainComponents = containComponents;
    }    
    
    public BindingAttribute(
        LifeCycle lifeCycle = LifeCycle.Default)
        :this(null, lifeCycle)
    {
    }
    public BindingAttribute(
        Type typeToResolve, 
        LifeCycle lifeCycle = LifeCycle.Default, 
        params Type[] injectInto
        )
        :this(typeToResolve, lifeCycle, injectInto, null)
    {
    }
}