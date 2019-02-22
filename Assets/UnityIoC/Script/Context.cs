/**
 * Author:    Vinh Vu Thanh
 * This class is a part of Unity IoC project that can be downloaded free at 
 * https://github.com/game-libgdx-unity/UnityEngine.IoC
 * (c) Copyright by MrThanhVinh168@gmail.com
 **/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace UnityIoC
{
    public partial class Context
    {
        #region Variables & Constants

        private List<BindingAttribute> bindingAttributes = new List<BindingAttribute>();
        private List<InjectAttribute> injectAttributes = new List<InjectAttribute>();

        public bool requirePreRegistered = true;

        public BindingSetting BindingSetting;

        public Type TargetType { get; set; }

        private IContainer container;

        #endregion

        #region Private members

        public Context(Type target)
        {
            Initialize(target);
        }

        public Context()
        {
            Initialize();
        }

        private void InitialProcess()
        {
            if (container == null)
            {
                Debug.LogError("You need to call Initialize before call this method");
                return;
            }

            //try to get the implement class setting
            if (BindingSetting == null)
            {
                //try to load a default setting for context
                BindingSetting = UnityEngine.Resources.Load<BindingSetting>(CurrentAssembly.GetName().Name + "_default");
                if (BindingSetting)
                {
                    Debug.LogFormat("Found binding setting");
                    LoadDefaultBindingSetting(BindingSetting);
                }
            }

            ProcessBindingAttribute();

            ProcessAutomaticBinding();

            ProcessInjectAttributeForMonoBehaviour();
        }

        public void LoadDefaultBindingSetting(BindingSetting bindingSetting = null)
        {
            //if the parameter is null, use the internal member.
            if (bindingSetting == null)
            {
                if (BindingSetting)
                {
                    LoadDefaultBindingSetting(BindingSetting);
                }
                else
                {
                    Debug.Log("No binding setting found");
                }

                return;
            }

            ProcessBindingSetting(bindingSetting, false);
        }

        public void LoadBindingSetting(string settingFileName)
        {
            LoadBindingSetting(UnityEngine.Resources.Load<BindingSetting>(settingFileName));
        }

        public void LoadBindingSetting(BindingSetting bindingSetting)
        {
            if (!bindingSetting)
            {
                Debug.LogError("input must not be null!");
                return;
            }

            ProcessBindingSetting(bindingSetting, true);
        }


        private void ProcessBindingSetting(BindingSetting bindingSetting, bool overriden = false)
        {
            //binding for default setting 
            if (bindingSetting.defaultSettings != null)
            {
                Debug.Log("Process binding from default setting");
                foreach (var setting in bindingSetting.defaultSettings)
                {
                    BindFromSetting(setting, overriden);
                }
            }
        }

        private void BindFromSetting(BindingData bindingSetting, bool overriden = false)
        {
            if (bindingSetting.ImplementedType == null)
            {
                bindingSetting.ImplementedType = GetTypeFromCurrentAssembly(bindingSetting.ImplementedTypeHolder.name);
            }

            if (bindingSetting.AbstractType == null)
            {
                bindingSetting.AbstractType = GetTypeFromCurrentAssembly(bindingSetting.AbstractTypeHolder.name);
            }

            //remove all null injectInto holder.
            bindingSetting.InjectIntoHolder.RemoveAll(i => !i);

            if (bindingSetting.EnableInjectInto)
            {
                if (bindingSetting.InjectInto == null)
                {
                    //create an empty new list for injectInto list
                    bindingSetting.InjectInto = new List<Type>();
                }

                var injectIntoCount = bindingSetting.InjectIntoHolder.Count;

                for (var i = 0; i < injectIntoCount; i++)
                {
                    bindingSetting.InjectInto.Add(GetTypeFromCurrentAssembly(bindingSetting.InjectIntoHolder[i].name));
                }
            }

            Debug.Assert(bindingSetting.ImplementedType != null, "bind data must not null");
            Debug.Assert(bindingSetting.AbstractType != null, "bind data must not null");

            var lifeCycle = bindingSetting.LifeCycle;

            Debug.LogFormat("Bind from setting {0} for {1} by {2}",
                bindingSetting.ImplementedType,
                bindingSetting.AbstractType,
                lifeCycle.ToString());

            //try to unbind or stop binding if the overriden is set as true
            while (defaultContainer.IsRegistered(bindingSetting.AbstractType))
            {
                if (overriden)
                {
                    var registeredObject = defaultContainer.GetRegisteredObject(bindingSetting.AbstractType);

                    if (registeredObject != null)
                    {
                        Debug.LogFormat("Unbind {0} registered for {1}", bindingSetting.ImplementedType,
                            bindingSetting.AbstractType);
                        defaultContainer.registeredObjects.Remove(registeredObject);
                        defaultContainer.registeredTypes.Remove(bindingSetting.AbstractType);
                    }
                }
                else
                {
                    Debug.LogFormat("type of {0} which is already registered", bindingSetting.AbstractType);
                    return;
                }
            }

            var injectIntoArray = bindingSetting.InjectInto.Where(i => i != null).ToArray();

            if (bindingSetting.EnableInjectInto && injectIntoArray.Length > 0)
            {
                bindingAttributes.RemoveAll(b =>
                    b.TypeToResolve.Name == bindingSetting.AbstractType.Name &&
                    b.ConcreteType.Name == bindingSetting.ImplementedType.Name &&
                    ArrayEqual(b.InjectInto, injectIntoArray)
                );
            }

            //bind from binding setting
            //first check injectInto to create binding attributes
            if (bindingSetting.EnableInjectInto && injectIntoArray.Length > 0)
            {
                //add to binding cache
                var bindingAttribute = new BindingAttribute
                {
                    TypeToResolve = bindingSetting.AbstractType,
                    ConcreteType = bindingSetting.ImplementedType,
                    InjectInto = injectIntoArray,
                    LifeCycle = bindingSetting.LifeCycle
                };

                bindingAttributes.Add(bindingAttribute);
            }

            Bind(bindingSetting.AbstractType, bindingSetting.ImplementedType, lifeCycle);
        }

        private bool ArrayEqual<T>(T[] a, T[] b)
        {
            return (a.Length == b.Length && a.Intersect(b).Count() == a.Length);
        }

        private Type GetTypeFromCurrentAssembly(string className)
        {
            foreach (var type in CurrentAssembly.GetTypes())
            {
                if (type.Name == className)
                {
                    return type;
                }
            }

            Debug.LogFormat("Cannot get type {0} from assembly {1}", className, CurrentAssembly.GetName(true));
            return null;
        }

        private void ProcessAutomaticBinding()
        {
            if (!AutomaticBinding)
            {
                return;
            }

            var concreteTypes = CurrentAssembly.GetTypes().Where(t => !(t.IsInterface || t.IsAbstract)).ToArray();
            var abstractions = CurrentAssembly.GetTypes().Where(t => t.IsInterface || t.IsAbstract).ToArray();

            foreach (var abstraction in abstractions)
            {
                //process for IContainer internally
                if (abstraction == typeof(IContainer))
                {
                    Bind(abstraction, container);
                    continue;
                }

                //binding attribute gives Type a higher priority than automatic binding with no attributes 
                //Only automatically bind for classes have no binding attributes  
                var concreteType = concreteTypes.FirstOrDefault(t =>
                    t.GetCustomAttributes(typeof(BindingAttribute), true).Length == 0 &&
                    (t.GetInterface(abstraction.Name) != null) || t.IsSubclassOf(abstraction));

                if (concreteType == null) continue;

                //won't bind existing registered typeToResolve
                if (defaultContainer.IsRegistered(abstraction))
                {
                    Debug.LogFormat("type of {0} which is already registered", abstraction);
                    continue;
                }

                // this is low priority than binding using attributes or Bind() methods.
                Debug.LogFormat("Automatically Bind {0} for {1}", concreteType, abstraction);
                Bind(abstraction, concreteType);
            }
        }

        public void Dispose()
        {
            if (container != null)
            {
                container.Dispose();
            }
        }

        private void ProcessBindingAttribute()
        {
            var myAssembly = TargetType == null ? Assembly.GetExecutingAssembly() : TargetType.Assembly;
            foreach (Type concreteType in myAssembly.GetTypes())
            {
                var bindingAttributes = concreteType.GetCustomAttributes(
                    typeof(BindingAttribute), true
                ).Cast<BindingAttribute>();

                foreach (var bindingAttribute in bindingAttributes)
                {
                    if (bindingAttribute != null)
                    {
                        var typeToResolve = bindingAttribute.TypeToResolve;
                        var lifeCycle = bindingAttribute.LifeCycle;
                        var prefabName = bindingAttribute.GameObjectNames;
                        var injectInto = bindingAttribute.InjectInto;

                        //set the conrete type back to BindingAttribute
                        bindingAttribute.ConcreteType = concreteType;

                        //add to binding cache
                        this.bindingAttributes.Add(bindingAttribute);

                        Debug.Log("Found binding " + concreteType + " for " +
                                  (typeToResolve == null ? "itself" : typeToResolve.ToString()) + " with lifeCycle " +
                                  lifeCycle);

                        //bind
                        if (typeToResolve != null)
                        {
                            Bind(typeToResolve, concreteType, lifeCycle);
                        }
                        else
                        {
                            Bind(concreteType, concreteType, lifeCycle);
                        }
                    }
                }
            }
        }

        private void ProcessInjectAttributeForMonoBehaviour()
        {
            var behaviours = Object.FindObjectsOfType<MonoBehaviour>();

            Debug.Log("Found behavior: " + behaviours.Length);

            foreach (var mono in behaviours)
            {
                if (mono)
                {
                    ProcessInjectAttribute(mono);
                }
            }
        }

        public void ProcessInjectAttribute(object mono)
        {
            ProcessMethod(mono);
            ProcessProperties(mono);
            ProcessVariables(mono);
        }


        private void ProcessMethod(object mono)
        {
            Type objectType = mono.GetType();
            var methods = objectType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(method => method.IsDefined(typeof(InjectAttribute), true))
                .ToArray();

            if (methods.Length <= 0) return;

            Debug.Log(string.Format("Found {0} method to process", methods.Length));

            foreach (var method in methods)
            {
                ProcessMethodInfo(mono, method);
            }
        }

        private void ProcessMethodInfo(object mono, MethodInfo method, InjectAttribute inject = null)
        {
            if (inject == null)
            {
                inject = method.GetCustomAttributes(typeof(InjectAttribute), true).FirstOrDefault() as InjectAttribute;
            }

            injectAttributes.Add(inject);

            var parameters = method.GetParameters();
            var paramObjects = Array.ConvertAll(parameters, p =>
            {
                Debug.Log("Para: " + p.Name + " " + p.ParameterType);
                return container.ResolveObject(p.ParameterType, mono,
                    inject == null ? LifeCycle.Default : inject.LifeCycle);
            });
            method.Invoke(mono, paramObjects);
        }

        private void ProcessProperties(object mono)
        {
            Type objectType = mono.GetType();
            var properties = objectType
                .GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(property => property.IsDefined(typeof(InjectAttribute), false))
                .ToArray();

            if (properties.Length <= 0) return;

            Debug.Log(string.Format("Found {0} property to process", properties.Length));

            foreach (var property in properties)
            {
                var method = property.GetSetMethod(true);
                var inject = property
                    .GetCustomAttributes(typeof(InjectAttribute), true)
                    .FirstOrDefault() as InjectAttribute;

                //pass container to injectAttribute
                // ReSharper disable once PossibleNullReferenceException
                inject.container = GetContainer();

                //resolve as [Component] attributes
                //try to resolve as monoBehaviour or as array
                if (property.PropertyType.IsArray)
                {
                    var components = GetComponentsFromGameObject(mono, property.PropertyType, inject);
                    if (components != null && components.Length > 0)
                    {
                        property.SetValue(mono,
                            ConvertComponentArrayTo(property.PropertyType.GetElementType(), components), null);
                        continue;
                    }
                }
                else
                {
                    var component = GetComponentFromGameObject(mono, property.PropertyType, inject);
                    if (component)
                    {
                        property.SetValue(mono, component, null);
                        continue;
                    }
                }

                Debug.LogFormat("IComponentResolvable attribute fails to resolve {0}", property.PropertyType);
                //resolve object as [Singleton], [Transient] or [AsComponent] if component attribute fails to resolve
                ProcessMethodInfo(mono, method, inject);
            }
        }

        private void ProcessVariables(object mono)
        {
            Type objectType = mono.GetType();
            var fieldInfos = objectType
                .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(fieldInfo => fieldInfo.IsDefined(typeof(InjectAttribute), true))
                .ToArray();

            if (fieldInfos.Length > 0)
                Debug.Log(string.Format("Found {0} fieldInfo to process", fieldInfos.Length));

            foreach (var field in fieldInfos)
            {
                var inject =
                    field.GetCustomAttributes(typeof(InjectAttribute), true).FirstOrDefault() as InjectAttribute;

                injectAttributes.Add(inject);

                //pass container to injectAttribute
                inject.container = GetContainer();

                if (field.FieldType.IsArray)
                {
                    //check if field type is array for particular processing
                    var injectComponentArray = inject as IComponentArrayResolvable;

                    if (injectComponentArray == null)
                    {
                        throw new InvalidOperationException(
                            "You must use apply injectAttribute for IComponentArrayResolvable field to resolve the array of components");
                    }

                    //try to resolve as monoBehaviour first
                    var components = GetComponentsFromGameObject(mono, field.FieldType, inject);
                    if (components != null && components.Length > 0)
                    {
                        var array = ConvertComponentArrayTo(field.FieldType.GetElementType(), components);

                        field.SetValue(mono, array);
                        continue;
                    }
                }
                else
                {
                    //try to resolve as monoBehaviour component
                    var component = GetComponentFromGameObject(mono, field.FieldType, inject);
                    if (component)
                    {
                        field.SetValue(mono, component);
                        continue;
                    }
                }


                Debug.LogFormat("IComponentResolvable attribute fails to resolve {0}", field.FieldType);

                //resolve object as [Singleton], [Transient] or [AsComponent] if component attribute fails to resolve
                field.SetValue(mono,
                    container.ResolveObject(field.FieldType, mono,
                        inject == null ? LifeCycle.Default : inject.LifeCycle));
            }
        }

        private static Array ConvertComponentArrayTo(Type typeOfArray, Component[] components)
        {
            var array = Array.CreateInstance(typeOfArray, components.Length);
            for (var i = 0; i < components.Length; i++)
            {
                var component = components[i];
                array.SetValue(component, i);
            }

            return array;
        }

        /// <summary>
        /// Try to resolve unity component, this should be used in other attribute process methods
        /// </summary>
        /// <param name="mono">object is expected as unity mono behaviour</param>
        /// <returns>the component</returns>
        private Component GetComponentFromGameObject(object mono, Type type, InjectAttribute injectAttribute)
        {
            //not supported for transient or singleton injections
            if (injectAttribute.LifeCycle == LifeCycle.Transient ||
                injectAttribute.LifeCycle == LifeCycle.Singleton)
            {
                return null;
            }

            var behaviour = mono as MonoBehaviour;

            if (behaviour == null) return null;

            //output component
            Component component = null;
            
            //resolve by inject component to the gameobject
            var injectComponent = injectAttribute as IComponentResolvable;

            //try get/add component with IInjectComponent interface
            if (injectComponent != null)
            {
                component = injectComponent.GetComponent(behaviour, type);

                //unable to get it from gameObject
                if (component == null)
                {
                    Debug.LogFormat("Unable to resolve component of {0} for {1}", type, behaviour.name);
                }
            }

            return component;
        }

        /// <summary>
        /// Try to resolve array of components, this should be used in other attribute process methods
        /// </summary>
        /// <param name="mono">object is expected as unity mono behaviour</param>
        /// <returns>true if you want to stop other attribute process methods</returns>
        private Component[] GetComponentsFromGameObject(object mono, Type type, InjectAttribute injectAttribute)
        {
            //not supported for transient or singleton injections
            if (injectAttribute.LifeCycle == LifeCycle.Transient ||
                injectAttribute.LifeCycle == LifeCycle.Singleton)
            {
                return null;
            }

            var behaviour = mono as MonoBehaviour;

            if (behaviour == null) return null;

            //output components
            Component[] components = null;

            //resolve by inject component to the gameobject
            var injectComponentArray = injectAttribute as IComponentArrayResolvable;

            if (injectComponentArray == null)
            {
                throw new InvalidOperationException(
                    "You must use apply injectAttribute with IComponentArrayResolvable to resolve array of components");
                //unable to get it from gameObject
            }

            components = injectComponentArray.GetComponents(behaviour, type.GetElementType());

            //unable to get it from gameObject
            if (components == null || components.Length == 0)
            {
                Debug.LogFormat("Unable to resolve components of {0} for {1}, found {2} elements",
                    type.GetElementType(), behaviour.name, components != null ? components.Length : 0);
            }

            return components;
        }

        #endregion

        #region Public members

        private DefaultContainer defaultContainer
        {
            get { return (DefaultContainer) container; }
        }

        public void Initialize(Type target = null)
        {
            TargetType = target;
            container = new DefaultContainer(this);

            InitialProcess();
        }

        public IContainer GetContainer()
        {
            return container;
        }

        public void Bind<TTypeToResolve, TConcrete>() where TConcrete : TTypeToResolve
        {
            container.Bind<TTypeToResolve, TConcrete>(LifeCycle.Default);
        }

        public void Bind<TTypeToResolve, TConcrete>(LifeCycle lifeCycle) where TConcrete : TTypeToResolve
        {
            container.Bind<TTypeToResolve, TConcrete>(lifeCycle);
        }

        public void Bind(Type typeToResolve, object instance)
        {
            container.Bind(typeToResolve, instance);
        }

        public void Bind(Type typeToResolve, Type concreteType, LifeCycle lifeCycle = LifeCycle.Default)
        {
            container.Bind(typeToResolve, concreteType, lifeCycle);
        }

        public void Bind<TTypeToResolve>(object instance)
        {
            Bind(typeof(TTypeToResolve), instance);
        }

        public void Bind<TTypeToResolve>(LifeCycle lifeCycle)
        {
            container.Bind<TTypeToResolve>(lifeCycle);
        }

        public TTypeToResolve Resolve<TTypeToResolve>(LifeCycle lifeCycle = LifeCycle.Default,
            params object[] parameters)
        {
            return (TTypeToResolve) Resolve(typeof(TTypeToResolve), lifeCycle, parameters);
        }

        public object Resolve(Type typeToResolve, LifeCycle lifeCycle = LifeCycle.Default, params object[] parameters)
        {
            return container.Resolve(typeToResolve, lifeCycle, parameters);
        }

        #endregion

        #region Static members

        private static Context _defaultInstance;

        public static bool AutomaticBinding = false;

        private Assembly CurrentAssembly
        {
            get { return TargetType == null ? Assembly.GetExecutingAssembly() : TargetType.Assembly; }
        }

        public static Context DefaultInstance
        {
            get
            {
                if (_defaultInstance == null)
                {
                    _defaultInstance = new Context();
                }

                return _defaultInstance;
            }
        }

        public static Context GetDefaultInstance(Type type = null, bool recreate = false)
        {
            if (_defaultInstance == null || recreate)
            {
                _defaultInstance = new Context(type);
            }

            return _defaultInstance;
        }

        public static void DeleteDefaultInstance()
        {
            if (_defaultInstance != null)
            {
                _defaultInstance.Dispose();
                _defaultInstance = null;
            }
        }

        public static void SetPropertyValue(object inputObject, string propertyName, object propertyVal)
        {
            Type type = inputObject.GetType();
            PropertyInfo propertyInfo = type.GetProperty(propertyName);

            //Convert.ChangeType does not handle conversion to nullable types
            //if the property type is nullable, we need to get the underlying type of the property
            var targetType = IsNullableType(propertyInfo.PropertyType)
                ? Nullable.GetUnderlyingType(propertyInfo.PropertyType)
                : propertyInfo.PropertyType;

            //Returns an System.Object with the specified System.Type and whose value is
            //equivalent to the specified object.
            propertyVal = Convert.ChangeType(propertyVal, targetType);
            //Set the value of the property
            propertyInfo.SetValue(inputObject, propertyVal, null);
        }

        public static bool IsNullableType(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>));
        }

        #endregion
    }
}