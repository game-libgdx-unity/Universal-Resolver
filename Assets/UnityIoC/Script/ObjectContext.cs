using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityIoC
{
    public class ObjectContext
    {
        private AssemblyContext assemblyContext;
        private object resolveFrom;

        public ObjectContext(object resolveFrom, AssemblyContext assemblyContext = null)
        {
            this.assemblyContext = assemblyContext ?? AssemblyContext.GetDefaultInstance();
            this.resolveFrom = resolveFrom;
        }

        public object Resolve(Type typeToResolve, LifeCycle lifeCycle = LifeCycle.Default, params object[] parameters)
        {
            return assemblyContext.Container.ResolveObject(typeToResolve, resolveFrom, lifeCycle, parameters);
        }
        
        public T Resolve<T>(LifeCycle lifeCycle = LifeCycle.Default, params object[] parameters)
        {
            return (T) assemblyContext.Container.ResolveObject(typeof(T), resolveFrom, lifeCycle, parameters);
        }
    }
};