using System;

namespace SimpleIoc
{
    public interface IContainer
    {
        void Bind(Type typeToResolve, object instance);     
        void Bind<TTypeToResolve, TConcrete>(LifeCycle lifeCycle);     
        void Bind(Type typeToResolve, Type concreteType, LifeCycle lifeCycle = LifeCycle.Default);
        void Bind<TConcrete>(LifeCycle lifeCycle);
        object Resolve(Type typeToResolve, LifeCycle lifeCycle = LifeCycle.Default, params object[] parameters);
        object ResolveObject(Type typeToResolve, object injectInto = null,
            LifeCycle preferredLifeCycle = LifeCycle.Default,
            params object[] parameters);

        void Unload();
    }
}

