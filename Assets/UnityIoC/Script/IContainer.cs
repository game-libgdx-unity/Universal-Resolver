/**
 * Author:    Vinh Vu Thanh
 * This class is a part of Unity IoC project that can be downloaded free at 
 * https://github.com/game-libgdx-unity/UnityEngine.IoC
 * (c) Copyright by MrThanhVinh168@gmail.com
 **/
using System;

namespace UnityIoC
{
    public interface IContainer : IDisposable
    {
        void Bind(Type typeToResolve, object instance);     
        void Bind<TTypeToResolve, TConcrete>(LifeCycle lifeCycle);     
        void Bind(Type typeToResolve, Type concreteType, LifeCycle lifeCycle = LifeCycle.Default);
        void Bind<TConcrete>(LifeCycle lifeCycle);
        object Resolve(Type typeToResolve, LifeCycle lifeCycle = LifeCycle.Default, params object[] parameters);
        
        /// <summary>
        /// resolve an instance from a type
        /// </summary>
        /// <param name="typeToResolve"></param>
        /// <param name="resolveFrom"></param>
        /// <param name="preferredLifeCycle"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        object ResolveObject(Type typeToResolve, object resolveFrom = null,
            LifeCycle preferredLifeCycle = LifeCycle.Default,
            params object[] parameters);

        bool IsRegistered(Type abstraction);
    }
}

