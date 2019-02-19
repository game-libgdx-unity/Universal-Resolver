using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityIoC;
using Object = UnityEngine.Object;

[CreateAssetMenu(fileName = "TestConfiguration", menuName = "Test/TestConfiguration", order = 1)]
public class ImplementClass :ScriptableObject
{
    [SerializeField] public List<BindingData> defaultSetting;
    [SerializeField] public Dictionary<Scene,  BindingData[]>[] sceneSettings;
    [SerializeField] public Scene scene;
}

[Serializable]
public class BindingData : UnityIoC.ICloneable<BindingData>
{
    [SerializeField] public Object AbstractType;
    [SerializeField] public Object ImplementedType;
    [SerializeField] public LifeCycle LifeCycle;
    [SerializeField] public Object InjectInto;
    
    public BindingData Clone()
    {
        BindingData output = new BindingData();
        output.AbstractType = AbstractType;
        output.ImplementedType = ImplementedType;
        output.LifeCycle = LifeCycle;
        output.InjectInto = InjectInto;

        return output;
    }

    object ICloneable.Clone()
    {
        return Clone();
    }
}