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

        public ObjectContext(object resolveFrom, AssemblyContext assemblyContext = null, BindingSetting bindingData = null)
        {
            this.assemblyContext = assemblyContext ?? AssemblyContext.GetDefaultInstance(resolveFrom.GetType());
            this.resolveFrom = resolveFrom;
            this.assemblyContext.LoadBindingSettingForType(resolveFrom.GetType(), bindingData);
        }

        public object Resolve(Type typeToResolve, LifeCycle lifeCycle = LifeCycle.Default, params object[] parameters)
        {
            return assemblyContext.Container.ResolveObject(typeToResolve, lifeCycle, resolveFrom, parameters);
        }
        
        public T Resolve<T>(LifeCycle lifeCycle = LifeCycle.Default, params object[] parameters)
        {
            return (T) assemblyContext.Container.ResolveObject(typeof(T), lifeCycle, resolveFrom, parameters);
        }
    }
};