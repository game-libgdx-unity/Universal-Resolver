/**
 * Author:    Vinh Vu Thanh
 * This class is a part of Unity IoC project that can be downloaded free at 
 * https://github.com/game-libgdx-unity/UnityEngine.IoC
 * (c) Copyright by MrThanhVinh168@gmail.com
 **/
using System;
using UnityIoC;

namespace UnityIoC
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Constructor |
                    AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class BindingAttribute : Attribute
    {
        public Type ConcreteType { get; set; }
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
        
        public BindingAttribute(
            params Type[] injectInto
        )
            :this(null, LifeCycle.Default, injectInto, null)
        {
        }
    }
}