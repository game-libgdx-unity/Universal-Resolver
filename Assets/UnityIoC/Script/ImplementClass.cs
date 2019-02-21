using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityIoC;
using Object = UnityEngine.Object;

[CreateAssetMenu(fileName = "default", menuName = "Binding", order = 1)]
public class ImplementClass :ScriptableObject
{
    [SerializeField] public List<BindingData> defaultSettings = new List<BindingData>();
}

[Serializable]
public class BindingData : ICloneable<BindingData>
{
    [SerializeField] public Type AbstractType;
    [SerializeField] public Type ImplementedType;
    [SerializeField] public Type InjectInto;
    [SerializeField] public bool EnableInjectInto;
    
    [SerializeField] public Object AbstractTypeHolder;
    [SerializeField] public Object ImplementedTypeHolder;
    [SerializeField] public Object InjectIntoHolder;
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
        output.InjectIntoHolder = InjectIntoHolder;
        output.AbstractType = AbstractType;
        output.ImplementedType = ImplementedType;
        output.LifeCycle = LifeCycle;
        output.InjectInto = InjectInto;
        output.SceneName = SceneName;

        return output;
    }
}