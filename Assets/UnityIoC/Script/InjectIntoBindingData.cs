using System;
using System.Collections.Generic;
using UnityEngine;
using UnityIoC;
using Object = UnityEngine.Object;

namespace UnityIoC
{
    [Serializable]
    public class InjectIntoBindingData : ICloneable<InjectIntoBindingData>
    {
        [SerializeField] public Type AbstractType;
        [SerializeField] public Type ImplementedType;
        [SerializeField] public Object AbstractTypeHolder;
        [SerializeField] public Object ImplementedTypeHolder;
        [SerializeField] public LifeCycle LifeCycle;


        [SerializeField] public Type InjectInto;
        [SerializeField] public Object InjectIntoHolder;
        [SerializeField] public bool EnableInjectInto;

        public InjectIntoBindingData Clone()
        {
            InjectIntoBindingData output = new InjectIntoBindingData();
            output.AbstractTypeHolder = AbstractTypeHolder;
            output.InjectIntoHolder = InjectIntoHolder;
            output.AbstractType = AbstractType;
            output.ImplementedType = ImplementedType;
            output.LifeCycle = LifeCycle;

            output.ImplementedTypeHolder = ImplementedTypeHolder;
            output.InjectInto = InjectInto;
            output.EnableInjectInto = EnableInjectInto;

            return output;
        }
    }
}