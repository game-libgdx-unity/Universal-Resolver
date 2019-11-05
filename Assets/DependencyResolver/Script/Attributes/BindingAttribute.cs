///**
// * Author:    Vinh Vu Thanh
// * This class is a part of Universal Resolver project that can be downloaded free at 
// * https://github.com/game-libgdx-unity/UnityEngine.IoC
// * (c) Copyright by MrThanhVinh168@gmail.com
// **/
using System;
using UnityIoC;

namespace UnityIoC
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Constructor |
                    AttributeTargets.Field, AllowMultiple = true)]
    public class BindingAttribute : Attribute
    {
        public Type ConcreteType { get; set; }
        public Type TypeToResolve { get; set; }
        public LifeCycle LifeCycle { get; set; }
        public Type[] InjectInto { get; set; }
    
        public string[] Tags { get; set; }
    
        public int[] Layers { get; set; }

        public BindingAttribute(){}
        
        public BindingAttribute(Type typeToResolve = null, 
            IBindingCondition condition = null,
            LifeCycle lifeCycle = LifeCycle.Default, 
            params Type[] injectInto
            )
        {
            TypeToResolve = typeToResolve;
            LifeCycle = lifeCycle;
            InjectInto = injectInto;
        }    
    }

    public interface IBindingCondition
    {
        bool ShouldResolveByThisImplement(Context.ResolveInput input);
    }
}