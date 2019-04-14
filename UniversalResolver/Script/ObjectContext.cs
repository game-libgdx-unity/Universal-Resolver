using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityIoC
{
    public class ObjectContext
    {
        private Context context;
        private object resolveFrom;

        public ObjectContext(object resolveFrom, Context context = null, BindingSetting bindingData = null)
        {
            this.context = context ?? Context.GetDefaultInstance(resolveFrom.GetType());
            this.resolveFrom = resolveFrom;
            this.context.LoadBindingSettingForType(resolveFrom.GetType(), bindingData);
        }

        public object Resolve(Type typeToResolve, LifeCycle lifeCycle = LifeCycle.Default, params object[] parameters)
        {
            return context.DefaultContainer.ResolveObject(typeToResolve, lifeCycle, resolveFrom, parameters);
        }
        
        public T Resolve<T>(LifeCycle lifeCycle = LifeCycle.Default, params object[] parameters)
        {
            return (T) context.DefaultContainer.ResolveObject(typeof(T), lifeCycle, resolveFrom, parameters);
        }
    }
};