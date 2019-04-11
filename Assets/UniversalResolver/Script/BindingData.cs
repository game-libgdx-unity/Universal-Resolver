using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UnityIoC
{
    [Serializable]
    public class BindingData
    {
        [SerializeField] public Type AbstractType;
        [SerializeField] public Type ImplementedType;
        [SerializeField] public GameObject Prefab;
        [SerializeField] public LifeCycle LifeCycle;
    }
    
    
    [Serializable]
    public class BindingAsset : BindingData, ICloneable<BindingAsset>
    {
        [SerializeField] public Object AbstractTypeHolder;
        [SerializeField] public Object ImplementedTypeHolder;

        public BindingAsset Clone()
        {
            var output = new BindingAsset();
            output.AbstractTypeHolder = AbstractTypeHolder;
            output.ImplementedTypeHolder = ImplementedTypeHolder;
            output.AbstractType = AbstractType;
            output.ImplementedType = ImplementedType;
            output.Prefab = Prefab;
            output.LifeCycle = LifeCycle;
            return output;
        }
    }
}