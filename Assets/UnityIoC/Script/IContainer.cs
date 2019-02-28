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
        void Bind(InjectIntoBindingData data);

        bool IsRegistered(Type abstraction);
        AssemblyContext.RegisteredObject GetRegisteredObject(Type typeToResolve);
    }
}

