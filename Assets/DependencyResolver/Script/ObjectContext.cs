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
            if (!context.initialized)
            {
                return null;
            }

            return context.DefaultContainer.ResolveObject(typeToResolve, lifeCycle, resolveFrom, parameters);
        }

        public T Resolve<T>(LifeCycle lifeCycle = LifeCycle.Default, params object[] parameters)
        {
            if (!context.initialized)
            {
                return default(T);
            }

            return (T) context.DefaultContainer.ResolveObject(typeof(T), lifeCycle, resolveFrom, parameters);
        }
    }

    public class ObjectContext<TContext>
    {
        private Context context;

        public ObjectContext(Context context = null, BindingSetting bindingData = null)
        {
            this.context = context ?? Context.GetDefaultInstance(typeof(TContext));
            this.context.LoadBindingSettingForType(typeof(TContext), bindingData);
        }

        public object Resolve(Type typeToResolve, LifeCycle lifeCycle = LifeCycle.Default, params object[] parameters)
        {
            if (!context.initialized)
            {
                return null;
            }

            return context.DefaultContainer.ResolveObject(typeToResolve, lifeCycle, typeof(TContext), parameters);
        }

        public TResolve Resolve<TResolve>(LifeCycle lifeCycle = LifeCycle.Default, params object[] parameters)
        {
            if (!context.initialized)
            {
                return default(TResolve);
            }

            return (TResolve) context.DefaultContainer.ResolveObject(typeof(TResolve), lifeCycle, typeof(TContext), parameters);
        }
    }
};