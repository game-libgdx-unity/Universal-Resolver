using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UnityIoC
{
    [Serializable]
    public class InjectIntoBindingData : BindingData
    {
        [SerializeField] public Type InjectInto;
        [SerializeField] public bool EnableInjectInto;
    }

    [Serializable]
    public class InjectIntoBindingAsset : InjectIntoBindingData, ICloneable<InjectIntoBindingAsset>
    {
        [SerializeField] public List<Object> InjectIntoHolder = new List<Object>();
        [SerializeField] public Object AbstractTypeHolder;
        [SerializeField] public Object ImplementedTypeHolder;

        public InjectIntoBindingAsset Clone()
        {
            InjectIntoBindingAsset output = new InjectIntoBindingAsset();
            output.AbstractTypeHolder = AbstractTypeHolder;
            output.InjectIntoHolder = InjectIntoHolder;
            output.AbstractType = AbstractType;
            output.ImplementedType = ImplementedType;
            output.LifeCycle = LifeCycle;
            output.Prefab = Prefab;
            output.ImplementedTypeHolder = ImplementedTypeHolder;
            output.InjectInto = InjectInto;
            output.EnableInjectInto = EnableInjectInto;
            return output;
        }
    }
}