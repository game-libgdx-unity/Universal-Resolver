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
    public partial class AssemblyContext
    {
        #region Variables & Constants

        private List<InjectAttribute> injectAttributes = new List<InjectAttribute>();

        public bool requirePreRegistered = true;
        public bool automaticBinding = false;

        public Type TargetType { get; set; }

        private DefaultContainer container;

        #endregion

        #region Constructors

        public AssemblyContext(object target, bool automaticBinding = false)
        {
            Initialize(target.GetType(), automaticBinding);
        }

        public AssemblyContext(Type typeInTargetedAssembly, bool automaticBinding = false)
        {
            Initialize(typeInTargetedAssembly, automaticBinding);
        }

        public AssemblyContext()
        {
            Initialize();
        }

        #endregion

        #region Private members

        private string CurrentSceneName
        {
            get { return SceneManager.GetActiveScene().name; }
        }

        private Assembly CurrentAssembly
        {
            get { return TargetType == null ? Assembly.GetExecutingAssembly() : TargetType.Assembly; }
        }

        private void InitialProcess()
        {
            if (container == null)
            {
                Debug.LogError("You need to call Initialize before call this method");
                return;
            }

            Debug.Log("Processing assembly {0}...", CurrentAssembly.GetName().Name);

            //try to load a default setting for context
            var BindingSetting =
                UnityEngine.Resources.Load<InjectIntoBindingSetting>(CurrentAssembly.GetName().Name);

            if (BindingSetting)
            {
                Debug.Log("Found binding setting for assembly");
                LoadBindingSetting(BindingSetting, false);
            }

            //try to get the default binding setting for current scene
            var sceneBindingSetting = UnityEngine.Resources.Load<InjectIntoBindingSetting>(
                string.Format("{0}_{1}", CurrentAssembly.GetName().Name, SceneManager.GetActiveScene().name)
            );

            if (sceneBindingSetting)
            {
                Debug.Log("Found binding setting for scene");
                LoadBindingSetting(sceneBindingSetting, true);
            }

            ProcessInjectAttributeForMonoBehaviour();
        }

        /// <summary>
        /// Load binding setting to inject for a type T in a scene
        /// </summary>
        /// <param name="bindingSetting">custom setting</param>
        /// <typeparam name="T">Type to inject</typeparam>
        public void LoadBindingSettingForType<T>(BindingSetting bindingSetting = null)
        {
            if (!bindingSetting)
            {
                Debug.Log("Load binding setting for {0} from resources folders", typeof(T).Name);

                //try to load setting by name format: type_scene
                bindingSetting = Resources.Load<BindingSetting>("{0}_{1}"
                    , typeof(T).Name
                    , CurrentSceneName);

                if (!bindingSetting)
                {
                    //try to load setting by name format: type
                    bindingSetting = Resources.Load<BindingSetting>(typeof(T).Name);
                    if (bindingSetting)
                    {
                        Debug.Log("Found default setting for type {0}", typeof(T).Name);
                    }
                }
                else
                {
                    Debug.Log("Found default setting for type {0} in scene {1}", typeof(T).Name
                        , CurrentSceneName);
                }
            }

            if (!bindingSetting)
            {
                Debug.Log("Binding setting for {0} must not be null!", typeof(T).Name);
                return;
            }

            foreach (var setting in bindingSetting.defaultSettings)
            {
                BindFromSetting(setting, typeof(T), true);
            }
        }

        /// <summary>
        /// load binding setting for current scene
        /// </summary>
        /// <param name="bindingSetting">custom setting</param>
        public void LoadBindingSettingForScene(InjectIntoBindingSetting bindingSetting = null)
        {
            if (!bindingSetting)
            {
                Debug.Log("Load binding setting for type from resources folders");

                //try to load setting by name format: type_scene
                bindingSetting = Resources.Load<InjectIntoBindingSetting>(CurrentSceneName);

                if (bindingSetting)
                {
                    Debug.Log("Found default setting for scene {0}", CurrentSceneName);
                }
            }

            if (!bindingSetting)
            {
                Debug.Log("Binding setting must not be null!");
                return;
            }

            //binding for default setting 
            if (bindingSetting.defaultSettings != null)
            {
                Debug.Log("Process binding from default setting");
                foreach (var setting in bindingSetting.defaultSettings)
                {
                    BindFromSetting(setting, true);
                }
            }
        }

        public void LoadBindingSetting(InjectIntoBindingSetting bindingSetting, bool overriden = true)
        {
            Debug.Log("{0} settings found: ", bindingSetting.defaultSettings.Count);
            //binding for default setting 
            if (bindingSetting.defaultSettings != null)
            {
                foreach (var setting in bindingSetting.defaultSettings)
                {
                    BindFromSetting(setting, overriden);
                }
            }
        }

        private void BindFromSetting(InjectIntoBindingData injectIntoBindingSetting, bool overriden = false)
        {
            if (injectIntoBindingSetting.ImplementedType == null)
            {
                injectIntoBindingSetting.ImplementedType =
                    GetTypeFromCurrentAssembly(injectIntoBindingSetting.ImplementedTypeHolder.name);
            }

            if (injectIntoBindingSetting.AbstractType == null)
            {
                injectIntoBindingSetting.AbstractType =
                    GetTypeFromCurrentAssembly(injectIntoBindingSetting.AbstractTypeHolder.name);
            }

            if (injectIntoBindingSetting.EnableInjectInto)
            {
                if (injectIntoBindingSetting.InjectInto == null)
                {
                    //create an empty new list for injectInto list
                    injectIntoBindingSetting.InjectInto =
                        GetTypeFromCurrentAssembly(injectIntoBindingSetting.InjectIntoHolder.name);
                }
            }

            Debug.Assert(injectIntoBindingSetting.ImplementedType != null, "bind data must not null");
            Debug.Assert(injectIntoBindingSetting.AbstractType != null, "bind data must not null");

            var lifeCycle = injectIntoBindingSetting.LifeCycle;

            Debug.Log("Bind from setting {0} for {1} by {2}",
                injectIntoBindingSetting.ImplementedType,
                injectIntoBindingSetting.AbstractType,
                lifeCycle.ToString());

            //try to unbind or stop binding if the overriden is set as true
            for (var i = 0; i < defaultContainer.registeredObjects.Count; i++)
            {
                var registeredObject = defaultContainer.registeredObjects[i];
            }

            if (defaultContainer.IsRegistered(injectIntoBindingSetting.AbstractType))
            {
                if (overriden)
                {
                    var registeredObject = defaultContainer.GetRegisteredObject(injectIntoBindingSetting.AbstractType);

                    if (registeredObject.ConcreteType == injectIntoBindingSetting.ImplementedType &&
                        registeredObject.TypeToResolve == injectIntoBindingSetting.InjectInto)
                    {
                        Debug.Log("Unbind {0} registered for {1}", injectIntoBindingSetting.ImplementedType,
                            injectIntoBindingSetting.AbstractType);
                        defaultContainer.registeredObjects.Remove(registeredObject);
                        defaultContainer.registeredTypes.Remove(injectIntoBindingSetting.AbstractType);
                    }
                }
            }

            defaultContainer.Bind(injectIntoBindingSetting);
        }

        private void BindFromSetting(BindingData bindingSetting, Type typeToInject, bool overriden = false)
        {
            if (bindingSetting.ImplementedType == null)
            {
                bindingSetting.ImplementedType =
                    GetTypeFromCurrentAssembly(bindingSetting.ImplementedTypeHolder.name);
            }

            if (bindingSetting.AbstractType == null)
            {
                bindingSetting.AbstractType =
                    GetTypeFromCurrentAssembly(bindingSetting.AbstractTypeHolder.name);
            }

            Debug.Assert(bindingSetting.ImplementedType != null, "bind data must not null");
            Debug.Assert(bindingSetting.AbstractType != null, "bind data must not null");

            var lifeCycle = bindingSetting.LifeCycle;

            if (overriden)
            {
            }

            Debug.Log("Bind from setting {0} for {1} by {2}",
                bindingSetting.ImplementedType,
                bindingSetting.AbstractType,
                lifeCycle.ToString());

            Bind(bindingSetting.AbstractType, bindingSetting.ImplementedType, lifeCycle);
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

            Debug.Log("Cannot get type {0} from assembly {1}", className, CurrentAssembly.GetName().Name);
            return null;
        }

        public void Dispose()
        {
            if (container != null)
            {
                container.Dispose();
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
                inject.container = Container;

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

                Debug.Log("IComponentResolvable attribute fails to resolve {0}", property.PropertyType);
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
                inject.container = Container;

                if (field.FieldType.IsArray)
                {
                    //check if field type is array for particular processing
                    var injectComponentArray = inject as IComponentArrayResolvable;

                    if (injectComponentArray == null)
                    {
                        throw new InvalidOperationException(
                            "You must apply injectAttribute implementing IComponentArrayResolvable field to resolve the array of components");
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


                Debug.Log("IComponentResolvable attribute fails to resolve {0}", field.FieldType);

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
                    Debug.Log("Unable to resolve component of {0} for {1}", type, behaviour.name);
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
                Debug.Log("Unable to resolve components of {0} for {1}, found {2} elements",
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

        public void Initialize(Type target = null, bool automaticBinding = false)
        {
            this.automaticBinding = automaticBinding;
            this.TargetType = target;
            container = new DefaultContainer(this);

            InitialProcess();
        }

        public DefaultContainer Container
        {
            get { return container; }
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

        public TTypeToResolve Resolve<TTypeToResolve>(object resolveFrom = null,
            LifeCycle lifeCycle = LifeCycle.Default,
            params object[] parameters)
        {
            return (TTypeToResolve) Resolve(typeof(TTypeToResolve), resolveFrom, lifeCycle, parameters);
        }

        public object Resolve(Type typeToResolve, object resolveFrom = null, LifeCycle lifeCycle = LifeCycle.Default,
            params object[] parameters)
        {
            return container.ResolveObject(typeToResolve, resolveFrom, lifeCycle, parameters);
        }

        #endregion

        #region Static members

        private static AssemblyContext _defaultInstance;

        public static AssemblyContext GetDefaultInstance(Type type = null, bool recreate = false)
        {
            if (_defaultInstance == null || recreate)
            {
                _defaultInstance = new AssemblyContext(type);
            }

            return _defaultInstance;
        }

        public static void DisposeDefaultInstance()
        {
            if (_defaultInstance != null)
            {
                _defaultInstance.Dispose();
                _defaultInstance = null;
            }
        }

        public static object ResolveObject(Type typeToResolve, LifeCycle lifeCycle = LifeCycle.Default,
            object resolveFrom = null, params object[] parameters)
        {
            return _defaultInstance.Container.ResolveObject(typeToResolve, resolveFrom, lifeCycle, parameters);
        }

        public static T ResolveObject<T>(LifeCycle lifeCycle = LifeCycle.Default, object resolveFrom = null,
            params object[] parameters)
        {
            return (T) _defaultInstance.Container.ResolveObject(typeof(T), resolveFrom, lifeCycle, parameters);
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