/**
 * Author:    Vinh Vu Thanh
 * This class is a part of Universal Resolver project that can be downloaded free at 
 * https://github.com/game-libgdx-unity/UnityEngine.IoC
 * (c) Copyright by MrThanhVinh168@gmail.com
 **/
using System;
using System.Collections.Generic;

namespace UnityIoC
{
    public interface IContainer : IDisposable
    {
        Context.RegisteredObject Bind(InjectIntoBindingData data);

        object ResolveObject(Type abstractType,
            LifeCycle preferredLifeCycle = LifeCycle.Default,
            object resolveFrom = null,
            params object[] parameters);
        IEnumerable<Context.RegisteredObject> GetRegisteredObject(Type typeToResolve);
    }
}

