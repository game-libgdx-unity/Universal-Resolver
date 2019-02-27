using System;
using UnityEngine;
using UnityIoC;
using Object = UnityEngine.Object;

namespace UnityIoC
{
    [Serializable]
    public class BindingData : ICloneable<BindingData>
    {
        [SerializeField] public Type AbstractType;
        [SerializeField] public Type ImplementedType;
        [SerializeField] public Object AbstractTypeHolder;
        [SerializeField] public Object ImplementedTypeHolder;
        [SerializeField] public LifeCycle LifeCycle;

        public BindingData Clone()
        {
            var output = new BindingData();
            output.AbstractTypeHolder = AbstractTypeHolder;
            output.ImplementedTypeHolder = ImplementedTypeHolder;
            output.AbstractType = AbstractType;
            output.ImplementedType = ImplementedType;
            output.LifeCycle = LifeCycle;

            return output;
        }
    }
}