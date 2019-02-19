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
    [SerializeField] public BindingData[] classObjectsToLoad;
}

[Serializable]
public class BindingData
{
    [SerializeField] public Object AbstractType;
    [SerializeField] public Object ImplementedType;
    [SerializeField] public LifeCycle LifeCycle;
    [SerializeField] public Object InjectInto;
}