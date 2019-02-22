using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityIoC;
using Object = UnityEngine.Object;

[CreateAssetMenu(fileName = "default", menuName = "Binding", order = 1)]
public class BindingSetting :ScriptableObject
{
    [SerializeField] public List<BindingData> defaultSettings = new List<BindingData>();
}

[Serializable]
public class BindingData : ICloneable<BindingData>
{
    [SerializeField] public Type AbstractType;
    [SerializeField] public Type ImplementedType;
    [SerializeField] public List<Type> InjectInto = new List<Type>();
    [SerializeField] public bool EnableInjectInto;
    
    [SerializeField] public Object AbstractTypeHolder;
    [SerializeField] public Object ImplementedTypeHolder;
    [SerializeField] public List<Object> InjectIntoHolder = new List<Object>();
    [SerializeField] public LifeCycle LifeCycle;

    public BindingData Clone()
    {
        BindingData output = new BindingData();
        output.AbstractTypeHolder = AbstractTypeHolder;
        output.ImplementedTypeHolder = ImplementedTypeHolder;
        output.InjectIntoHolder = InjectIntoHolder;
        output.AbstractType = AbstractType;
        output.ImplementedType = ImplementedType;
        output.LifeCycle = LifeCycle;
        output.InjectInto = InjectInto;
        output.EnableInjectInto = EnableInjectInto;

        return output;
    }
}

[Serializable]
public class SceneBindingData : BindingData, ICloneable<SceneBindingData>
{
    [SerializeField] public string SceneName;
    [SerializeField] public Object SceneHolder;
    
    public new SceneBindingData Clone()
    {
        SceneBindingData output = new SceneBindingData();
        output.AbstractTypeHolder = AbstractTypeHolder;
        output.ImplementedTypeHolder = ImplementedTypeHolder;
        output.InjectIntoHolder = new List<Object>(InjectIntoHolder);
        output.AbstractType = AbstractType;
        output.ImplementedType = ImplementedType;
        output.LifeCycle = LifeCycle;
        output.InjectInto = new List<Type>(InjectInto);
        output.SceneName = SceneName;
        output.EnableInjectInto = EnableInjectInto;
        
        return output;
    }
}