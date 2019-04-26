/**
 * Author:    Vinh Vu Thanh
 * This class is a part of Universal Resolver project that can be downloaded free at 
 * https://github.com/game-libgdx-unity/UnityEngine.IoC
 * (c) Copyright by MrThanhVinh168@gmail.com
 **/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace UnityIoC
{
    public partial class Context

        #region Variables & Constants

    {
        /// <summary>
        /// This is the name of the default assembly that unity generated to compile your code
        /// </summary>
        private const string DefaultAssemblyName = "Assembly-CSharp";

        /// <summary>
        /// Custom logger
        /// </summary>
        private readonly Logger debug = new Logger(typeof(Context));

        /// <summary>
        /// cached inject attributes
        /// </summary>
        private List<InjectBaseAttribute> injectAttributes = new List<InjectBaseAttribute>();

        /// <summary>
        /// cached objects from scene
        /// </summary>
        private Dictionary<Type, Component> monoScripts = new Dictionary<Type, Component>();

        /// <summary>
        /// Automatic binding a external setting file with the same name as the assembly's then process all mono-behaviours in
        /// scene by [inject] attributes
        /// </summary>
        private bool automaticBinding = false;

        /// <summary>
        /// A targeted Type that context will retrieve its assembly for initializations.
        /// </summary>
        private Type targetType;

        /// <summary>
        /// If this context is available to use.
        /// </summary>
        internal bool initialized;

        /// <summary>
        /// a container of references
        /// </summary>
        private Container container;

        /// <summary>
        /// Name of an assembly that will be loaded when initializing the context in case TargetType is null
        /// </summary>
        private string assemblyName = DefaultAssemblyName;

        #endregion

        #region Constructors

        /// <summary>
        /// Pass a object belong to the assembly you want to process
        /// </summary>
        /// <param name="target">the object</param>
        /// <param name="autoFindBindSetting">if true, will load bindingsetting and process all game object for inject attribute</param>
        public Context(object target, bool autoFindBindSetting = true)
        {
            Initialize(target.GetType(), autoFindBindSetting);
        }

        /// <summary>
        /// Pass a type belong to the assembly you want to process
        /// </summary>
        /// <param name="target">the object</param>
        /// <param name="autoFindBindSetting">if true, will load bindingsetting and process all game object for inject attribute</param>
        public Context(Type typeInTargetedAssembly, bool autoFindBindSetting = true)
        {
            Initialize(typeInTargetedAssembly, autoFindBindSetting);
        }


        /// <summary>
        /// Use the default assembly to process
        /// </summary>
        /// <param name="target">the object</param>
        /// <param name="autoFindBindSetting">if true, will load bindingsetting and process all game object for inject attribute</param>
        public Context()
        {
            Initialize(null, false);
        }

        #endregion

        #region Private members

        /// <summary>
        /// Process automatically bindingSetting, then process inject attribute in every game object
        /// </summary>
        private void InitialProcess()
        {
            if (container == null)
            {
                debug.LogError("You need to call Initialize before calling this method");
                return;
            }

            if (!automaticBinding)
            {
                return;
            }

            debug.Log("Processing assembly {0}...", CurrentAssembly.GetName().Name);

            //try to load a default InjectIntoBindingSetting setting for context
            var injectIntoBindingSetting =
                UnityEngine.Resources.Load<InjectIntoBindingSetting>(CurrentAssembly.GetName().Name);

            if (injectIntoBindingSetting)
            {
                debug.Log("Found InjectIntoBindingSetting for assembly");
                LoadBindingSetting(injectIntoBindingSetting);
            }

            //try to get the default InjectIntoBindingSetting setting for current scene
            var sceneInjectIntoBindingSetting = UnityEngine.Resources.Load<InjectIntoBindingSetting>(
                string.Format("{0}_{1}", CurrentAssembly.GetName().Name, SceneManager.GetActiveScene().name)
            );

            if (sceneInjectIntoBindingSetting)
            {
                debug.Log("Found InjectIntoBindingSetting for scene");
                LoadBindingSetting(sceneInjectIntoBindingSetting);
            }

            //try to load a default BindingSetting setting for context
            var bindingSetting =
                UnityEngine.Resources.Load<BindingSetting>(CurrentAssembly.GetName().Name);

            if (bindingSetting)
            {
                debug.Log("Found binding setting for assembly");
                LoadBindingSetting(bindingSetting);
            }

            //try to get the default BindingSetting setting for current scene
            var sceneBindingSetting = UnityEngine.Resources.Load<BindingSetting>(
                string.Format("{0}_{1}", CurrentAssembly.GetName().Name, SceneManager.GetActiveScene().name)
            );

            if (sceneBindingSetting)
            {
                debug.Log("Found binding setting for scene");
                LoadBindingSetting(sceneBindingSetting);
            }


            ProcessInjectAttributeForMonoBehaviours();
        }

        /// <summary>
        /// Read binding data to create registerObjects
        /// </summary>
        /// <param name="bindingSetting"></param>
        /// <param name="InjectIntoType"></param>
        private void BindFromSetting(BindingAsset bindingSetting, Type InjectIntoType = null)
        {
            GameObject prefab = null;
            if (bindingSetting.AbstractType == null)
            {
                bindingSetting.AbstractType =
                    GetTypeFromCurrentAssembly(bindingSetting.AbstractTypeHolder.name);

                if (bindingSetting.AbstractType == null)
                {
                    debug.LogError("bindingSetting.AbstractType should not null!");
                    return;
                }
            }

            if (bindingSetting.ImplementedType == null)
            {
                if (bindingSetting.ImplementedTypeHolder is GameObject)
                {
                    prefab = bindingSetting.ImplementedTypeHolder as GameObject;
                    if (!bindingSetting.AbstractType.IsAbstract)
                    {
                        bindingSetting.ImplementedType = bindingSetting.AbstractType;
                    }
                    else
                    {
                        bindingSetting.ImplementedType = prefab.GetComponent(bindingSetting.AbstractType).GetType();
                    }
                }
                else
                {
                    bindingSetting.ImplementedType =
                        GetTypeFromCurrentAssembly(bindingSetting.ImplementedTypeHolder.name);
                }

                if (bindingSetting.ImplementedType == null)
                {
                    debug.LogError("bindingSetting.ImplementedType should not null!");
                    return;
                }
            }

            var lifeCycle = bindingSetting.LifeCycle;
            if (prefab != null)
            {
                lifeCycle = LifeCycle.Singleton | LifeCycle.Prefab;
            }


            debug.Log("Bind from setting {0} for {1} by {2} when inject into {3}",
                bindingSetting.ImplementedType,
                bindingSetting.AbstractType,
                lifeCycle.ToString(),
                InjectIntoType != null ? InjectIntoType.Name : "Null");


            //bind it with inject into type
            var injectIntoBindingSetting = new InjectIntoBindingData();

            injectIntoBindingSetting.Prefab = prefab;
            injectIntoBindingSetting.ImplementedType = bindingSetting.ImplementedType;
            injectIntoBindingSetting.AbstractType = bindingSetting.AbstractType;

            injectIntoBindingSetting.LifeCycle = lifeCycle;
            injectIntoBindingSetting.EnableInjectInto = InjectIntoType != null;
            injectIntoBindingSetting.InjectInto = InjectIntoType;

            DefaultContainer.Bind(injectIntoBindingSetting);
        }

        /// <summary>
        /// Read binding data to create registerObjects
        /// </summary>
        /// <param name="bindingSetting"></param>
        private void BindFromSetting(InjectIntoBindingAsset bindingSetting)
        {
            GameObject prefab = null;
            if (bindingSetting.AbstractType == null)
            {
                bindingSetting.AbstractType =
                    GetTypeFromCurrentAssembly(bindingSetting.AbstractTypeHolder.name);

                if (bindingSetting.AbstractType == null)
                {
                    debug.LogError("bindingSetting.AbstractType should not null!");
                    return;
                }
            }

            if (bindingSetting.ImplementedType == null)
            {
                if (bindingSetting.ImplementedTypeHolder is GameObject)
                {
                    prefab = bindingSetting.ImplementedTypeHolder as GameObject;
                    if (!bindingSetting.AbstractType.IsAbstract)
                    {
                        bindingSetting.ImplementedType = bindingSetting.AbstractType;
                    }
                    else
                    {
                        bindingSetting.ImplementedType = prefab.GetComponent(bindingSetting.AbstractType).GetType();
                    }
                }
                else
                    bindingSetting.ImplementedType =
                        GetTypeFromCurrentAssembly(bindingSetting.ImplementedTypeHolder.name);

                if (bindingSetting.ImplementedType == null)
                {
                    debug.LogError("bindingSetting.ImplementedType should not null!");
                    return;
                }
            }

            //bind it with inject into type
            var injectIntoBindingSetting = new InjectIntoBindingData();
            injectIntoBindingSetting.Prefab = prefab;
            injectIntoBindingSetting.ImplementedType = bindingSetting.ImplementedType;
            injectIntoBindingSetting.AbstractType = bindingSetting.AbstractType;
            injectIntoBindingSetting.LifeCycle = bindingSetting.LifeCycle;
            injectIntoBindingSetting.EnableInjectInto =
                bindingSetting.EnableInjectInto && bindingSetting.InjectIntoHolder.Count > 0;

            //binding with inject into
            if (injectIntoBindingSetting.EnableInjectInto)
            {
                foreach (var injectIntoHolder in bindingSetting.InjectIntoHolder)
                {
                    if (injectIntoHolder != null)
                    {
                        var injectIntoType = GetTypeFromCurrentAssembly(injectIntoHolder.name);

                        if (injectIntoType != null)
                        {
                            debug.Log("Bind from setting {0} for {1} by {2} when injectInto {3}",
                                bindingSetting.ImplementedType,
                                bindingSetting.AbstractType,
                                bindingSetting.LifeCycle.ToString(),
                                injectIntoHolder.name);

                            injectIntoBindingSetting.InjectInto = injectIntoType;
                            DefaultContainer.Bind(injectIntoBindingSetting);
                        }
                    }
                }
            }
            //binding without inject into
            else
            {
                DefaultContainer.Bind(injectIntoBindingSetting);
            }
        }


        /// <summary>
        /// Get a type from an assembly by its name
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        private Type GetTypeFromCurrentAssembly(string className)
        {
            foreach (var type in CurrentAssembly.GetTypes())
            {
                if (type.Name == className)
                {
                    return type;
                }
            }

            debug.Log("Cannot get type {0} from assembly {1}", className, CurrentAssembly.GetName().Name);
            return null;
        }

        public void Dispose()
        {
            initialized = false;
            monoScripts.Clear();

            if (allBehaviours != null)
                Array.Clear(allBehaviours, 0, allBehaviours.Length);

            injectAttributes.Clear();
            targetType = null;

            if (container != null)
            {
                container.Dispose();
            }
        }

        /// <summary>
        /// Process an object info for resolving any inject attribute from fields, properties, methods
        /// </summary>
        /// <param name="mono"></param>
        /// <param name="ignoreMonobehaviour"></param>
        public void ProcessInjectAttribute(object mono, bool ignoreMonobehaviour = false)
        {
            if (mono == null)
            {
                return;
            }

            var gameObj = mono as GameObject;
            if (gameObj)
            {
                foreach (var component in gameObj.GetComponents(typeof(MonoBehaviour)))
                {
                    ProcessMethod(component, ignoreMonobehaviour);
                    ProcessProperties(component, ignoreMonobehaviour);
                    ProcessVariables(component, ignoreMonobehaviour);
                }

                return;
            }

            var asObject = mono as Object;
            var asBehaviour = mono as MonoBehaviour;
            if (asObject == null || asBehaviour != null)
            {
                ProcessMethod(mono, ignoreMonobehaviour);
                ProcessProperties(mono, ignoreMonobehaviour);
                ProcessVariables(mono, ignoreMonobehaviour);
            }
        }

        /// <summary>
        /// Process a method for resolving any inject attribute
        /// </summary>
        /// <param name="mono"></param>
        /// <param name="ignoreMonobehaviour"></param>
        private void ProcessMethod(object mono, bool ignoreMonobehaviour)
        {
            Type objectType = mono.GetType();
            var methods = objectType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(method => method.IsDefined(typeof(InjectBaseAttribute), true))
                .ToArray();

            if (methods.Length <= 0) return;

            debug.Log(string.Format("Found {0} method to process", methods.Length));

            foreach (var method in methods)
            {
                ProcessMethodInfo(mono, method, ignoreMonobehaviour);
            }
        }

        /// <summary>
        /// Process a method info for resolving any inject attribute
        /// </summary>
        /// <param name="mono"></param>
        /// <param name="method"></param>
        /// <param name="ignoreMonobehaviour"></param>
        /// <param name="inject"></param>
        private void ProcessMethodInfo(object mono, MethodInfo method, bool ignoreMonobehaviour,
            InjectBaseAttribute inject = null)
        {
            if (inject == null)
            {
                inject =
                    method.GetCustomAttributes(typeof(InjectBaseAttribute), true).FirstOrDefault() as
                        InjectBaseAttribute;
            }

            injectAttributes.Add(inject);

            var parameters = method.GetParameters();

            //ignore process for monobehaviour
            if (ignoreMonobehaviour)
            {
                if (parameters.Any(p => p.ParameterType.IsSubclassOf(typeof(Component))))
                {
                    return;
                }
            }

            var paramObjects = Array.ConvertAll(parameters, p =>
            {
                debug.Log("Para: " + p.Name + " " + p.ParameterType);
                return container.ResolveObject(p.ParameterType,
                    inject == null ? LifeCycle.Default : inject.LifeCycle, mono.GetType());
            });
            method.Invoke(mono, paramObjects);
        }

        /// <summary>
        /// Process a property info for resolving any inject attribute
        /// </summary>
        /// <param name="mono"></param>
        /// <param name="ignoreMonobehaviour"></param>
        private void ProcessProperties(object mono, bool ignoreMonobehaviour)
        {
            Type objectType = mono.GetType();
            var properties = objectType
                .GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(property => property.IsDefined(typeof(InjectBaseAttribute), false))
                .ToArray();

            if (properties.Length <= 0) return;

            debug.Log(string.Format("Found {0} property to process", properties.Length));

            foreach (var property in properties)
            {
                if (ignoreMonobehaviour)
                {
                    if (property.PropertyType.IsSubclassOf(typeof(Component)))
                    {
                        continue;
                    }
                }

                var inject = property
                    .GetCustomAttributes(typeof(InjectBaseAttribute), true)
                    .FirstOrDefault() as InjectBaseAttribute;

                if (inject == null)
                {
                    continue;
                }

                //only process a property if the property's value is not set yet
//                var value = property.GetValue(mono, null);
//                if (value != value.DefaultValue())
//                {
//                    debug.Log(string.Format("Don't set value for property {0} due to non-default value",
//                        property.Name));
//
//                    //check to bind this instance
//                    if (inject.LifeCycle == LifeCycle.Singleton ||
//                        (inject.LifeCycle & LifeCycle.Singleton) == LifeCycle.Singleton)
//                    {
//                        container.Bind(property.PropertyType, value);
//                    }
//
//                    continue;
//                }

                var setMethod = property.GetSetMethod(true);

                //pass container to injectAttribute
                // ReSharper disable once PossibleNullReferenceException
                inject.container = DefaultContainer;

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
                    object component = null;

                    //try get from cache if conditions are met
                    component = TryGetObjectFromCache(inject, property.PropertyType);

                    //try to use IObjectResolvable to resolve objects
                    if (component == null && !property.PropertyType.IsSubclassOf(typeof(Component)))
                    {
                        component = GetObjectFromGameObject(mono, property.PropertyType);
                    }

                    //try to use IComponentResolvable to resolve objects
                    if (component == null)
                    {
                        component = GetComponent(mono, property.PropertyType, inject);
                    }

                    if (component != null)
                    {
                        if (inject.LifeCycle == LifeCycle.Singleton ||
                            (inject.LifeCycle & LifeCycle.Singleton) == LifeCycle.Singleton)
                        {
                            container.Bind(property.PropertyType, component);
                        }

                        property.SetValue(mono, component, null);
                        continue;
                    }
                }

                debug.Log("IComponentResolvable attribute fails to resolve {0}", property.PropertyType);
                //resolve object as [Singleton], [Transient] or [AsComponent] if component attribute fails to resolve
                ProcessMethodInfo(mono, setMethod, ignoreMonobehaviour, inject);
            }
        }

        private static object TryGetObjectFromCache(InjectBaseAttribute inject, Type type)
        {
            if ((inject.LifeCycle == LifeCycle.Cache ||
                 (inject.LifeCycle & LifeCycle.Cache) == LifeCycle.Cache) &&
                Context.ResolvedObjects.Count > 0)
            {
                return Context.ResolvedObjects.FindLast(o => type.IsInterface && type.IsAssignableFrom(o.GetType()) ||
                                                             !type.IsInterface && o.GetType() == type);
            }

            return null;
        }


        private void ProcessVariables(object mono, bool ignoreMonobehaviour)
        {
            Type objectType = mono.GetType();
            var fieldInfos = objectType
                .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(fieldInfo => fieldInfo.IsDefined(typeof(InjectBaseAttribute), true))
                .ToArray();

            if (fieldInfos.Length > 0)
                debug.Log(string.Format("Found {0} fieldInfo to process", fieldInfos.Length));

            foreach (var field in fieldInfos)
            {
                var type = field.FieldType;
                if (ignoreMonobehaviour)
                {
                    if (type.IsSubclassOf(typeof(Component)))
                    {
                        continue;
                    }
                }

                var inject =
                    field.GetCustomAttributes(typeof(InjectBaseAttribute), true)
                        .FirstOrDefault() as InjectBaseAttribute;

                if (inject == null)
                {
                    continue;
                }

                //only process a field if the field's value is not set yet

//                var value = field.GetValue(mono);
//                bool defaultValue = false;
//
//                if (field.FieldType.IsArray)
//                {
//                    if (value != null)
//                    {
//                        defaultValue = value != value.DefaultValue();
//
//                        var array = value as Array;
//                        if (array.Length == 0)
//                        {
//                            defaultValue = true;
//                        }
//                    }
//                }
//                else
//                {
//                    defaultValue = value != value.DefaultValue();
//                }
//
//                if (defaultValue)
//                {
//                    debug.Log(string.Format("Don't set value for field {0} due to non-default value", field.Name));
//
//                    //check to bind this instance
//                    if (inject.LifeCycle == LifeCycle.Singleton ||
//                        (inject.LifeCycle & LifeCycle.Singleton) == LifeCycle.Singleton)
//                    {
//                        debug.Log(string.Format("Bind instance for field {0}", field.Name));
//                        container.Bind(field.FieldType, value);
//                    }
//
//                    continue;
//                }

                injectAttributes.Add(inject);

                debug.Log("Processing field {0}", field.Name);

                //pass container to injectAttribute
                inject.container = DefaultContainer;

                if (type.IsArray)
                {
                    //check if field type is array for particular processing
                    var injectComponentArray = inject as IComponentArrayResolvable;

                    if (injectComponentArray == null)
                    {
                        throw new InvalidOperationException(
                            "You must apply injectAttribute implementing IComponentArrayResolvable field to resolve the array of components");
                    }

                    //try to resolve as monoBehaviour
                    var components = GetComponentsFromGameObject(mono, type, inject);
                    if (components != null && components.Length > 0)
                    {
                        var array = ConvertComponentArrayTo(type.GetElementType(), components);
                        field.SetValue(mono, array);
                        continue;
                    }
                }
                else
                {
                    object component = null;

                    //try get from cache if conditions are met
                    component = TryGetObjectFromCache(inject, type);

                    //try to use IObjectResolvable to resolve objects
                    if (component == null && !type.IsSubclassOf(typeof(Component)))
                    {
                        component = GetObjectFromGameObject(mono, type);
                    }

                    //try to use IComponentResolvable to resolve objects
                    if (component == null)
                    {
                        component = GetComponent(mono, type, inject);
                    }

                    if (component != null)
                    {
                        //if the life cycle is singleton, bind the instance of the Type with this component
                        if (inject.LifeCycle == LifeCycle.Singleton ||
                            (inject.LifeCycle & LifeCycle.Singleton) == LifeCycle.Singleton)
                        {
                            container.Bind(type, component);
                        }

                        field.SetValue(mono, component);

                        continue;
                    }
                }


                debug.Log("IComponentResolvable attribute fails to resolve {0}", type);

                //resolve object as [Singleton], [Transient] or [AsComponent] if component attribute fails to resolve
                field.SetValue(mono,
                    container.ResolveObject(
                        type,
                        inject.LifeCycle,
                        mono));
            }
        }

        private Array ConvertComponentArrayTo(Type typeOfArray, Component[] components)
        {
            var array = Array.CreateInstance(typeOfArray, components.Length);
            for (var i = 0; i < components.Length; i++)
            {
                var component = components[i];

                if (component)
                {
                    array.SetValue(component, i);
                }
            }

            return array;
        }


        /// <summary>
        /// Try to resolve non-component object, this should be used in other attribute process methods
        /// </summary>
        /// <param name="mono">object is expected as behaviour</param>
        /// <returns>the object that is not component</returns>
        private object GetObjectFromGameObject(object mono, Type type)
        {
            if (!mono.GetType().IsSubclassOf(typeof(Component)))
            {
                return null;
            }

            object objectFromGameObject = null;
            var comp = mono as Component;

            //get all mono behaviour components
            var objectGenericResolvableAsComponents = comp.GetComponents(typeof(MonoBehaviour));
            foreach (var objectObtainable in objectGenericResolvableAsComponents)
            {
                bool containsGenericObjectObtainableInterface = objectObtainable.GetType().GetInterfaces()
                    .Where(i => i.IsGenericType)
                    .Any(i => i.GetGenericTypeDefinition() == typeof(IObjectResolvable<>));

                if (containsGenericObjectObtainableInterface)
                {
                    var method = objectObtainable.GetType().GetMethods()
                        .FirstOrDefault(m => m.Name == "GetObject" && m.GetParameters().Length == 0);
                    if (method != null)
                    {
                        objectFromGameObject = method.Invoke(objectObtainable, null);
                        if (objectFromGameObject != null)
                        {
                            //quick return
                            return objectFromGameObject;
                        }
                    }
                }
            }

            var objectResolvableAsComponents = comp.GetComponents(typeof(IObjectResolvable));
            foreach (var objectObtainable in objectResolvableAsComponents)
            {
                var objectResolvable = objectObtainable as IObjectResolvable;
                objectFromGameObject = objectResolvable.GetObject(type);
                if (objectFromGameObject != null && type.IsAssignableFrom(objectFromGameObject.GetType()))
                {
                    //quick return
                    return objectFromGameObject;
                }
            }

            return objectFromGameObject;
        }

        /// <summary>
        /// Try to resolve unity component, this should be used in other attribute process methods
        /// </summary>
        /// <param name="mono">object is expected as unity mono behaviour</param>
        /// <returns>the component</returns>
        private Component GetComponent(object mono, Type type, InjectBaseAttribute injectAttribute)
        {
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
                if (component != null)
                {
                    /////////////////////////////////
//                    ProcessInjectAttribute(component);

                    if (injectAttribute.LifeCycle == LifeCycle.Prefab)
                    {
                        component.gameObject.SetActive(false);
                    }
                }
            }

            return component;
        }

        /// <summary>
        /// Try to resolve array of components, this should be used in other attribute process methods
        /// </summary>
        /// <param name="mono">object is expected as unity mono behaviour</param>
        /// <returns>true if you want to stop other attribute process methods</returns>
        private Component[] GetComponentsFromGameObject(object mono, Type type, InjectBaseAttribute injectAttribute)
        {
            //not supported for transient or singleton injections
//            if (injectAttribute.LifeCycle == LifeCycle.Transient ||
//                injectAttribute.LifeCycle == LifeCycle.Singleton)
//            {
//                return null;
//            }

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
                debug.Log("Unable to resolve components of {0} for {1}, found {2} elements",
                    type.GetElementType(), behaviour.name, components != null ? components.Length : 0);
            }
            else
            {
                foreach (var component in components)
                {
                    ProcessInjectAttribute(component);
                }
            }

            return components;
        }


        private void Initialize(Type target = null, bool automaticBinding = false)
        {
            if (!Initialized)
            {
                defaultInstance = this;
            }

            this.automaticBinding = automaticBinding;
            this.targetType = target;
            container = new Container(this);

            InitialProcess();

            initialized = true;
        }

        #endregion

        #region Public members

        /// <summary>
        /// Get current scene name
        /// </summary>
        public string CurrentSceneName
        {
            get { return SceneManager.GetActiveScene().name; }
        }

        private Assembly _currentAssembly;

        /// <summary>
        /// Shortcut to get current assembly or just return the default one of unity
        /// </summary>
        public Assembly CurrentAssembly
        {
            get
            {
                if (_currentAssembly == null)
                {
                    if (targetType == null)
                    {
                        try
                        {
                            if (string.IsNullOrEmpty(assemblyName))
                                _currentAssembly = Assembly.Load(DefaultAssemblyName);
                            else
                                _currentAssembly = Assembly.Load(assemblyName);
                        }
                        catch (Exception ex)
                        {
                            _currentAssembly = Assembly.GetExecutingAssembly();
                        }
                    }
                    else
                        _currentAssembly = targetType.Assembly;
                }

                return _currentAssembly;
            }
        }

        public void ResolveAction<T>(Action<T> action, LifeCycle lifeCycle = LifeCycle.Default,
            object resultFrom = null)
        {
            var arg = (T) ResolveObject(typeof(T), lifeCycle, resultFrom);
            action(arg);
        }

        public void ResolveAction<T1, T2>(Action<T1, T2> action,
            LifeCycle lifeCycle1 = LifeCycle.Default,
            LifeCycle lifeCycle2 = LifeCycle.Default,
            object resultFrom1 = null, object resultFrom2 = null)
        {
            var arg1 = (T1) ResolveObject(typeof(T1), lifeCycle1, resultFrom1);
            var arg2 = (T2) ResolveObject(typeof(T1), lifeCycle2, resultFrom2);
            action(arg1, arg2);
        }

        public Result ResolveFunc<Input, Result>(Func<Input, Result> func, LifeCycle lifeCycle = LifeCycle.Default,
            object resultFrom = null)
        {
            var arg = (Input) ResolveObject(typeof(Input), lifeCycle, resultFrom);
            return func(arg);
        }

        public Result ResolveFunc<Input1, Input2, Result>(Func<Input1, Input2, Result> func,
            LifeCycle lifeCycle1 = LifeCycle.Default,
            LifeCycle lifeCycle2 = LifeCycle.Default,
            object resultFrom1 = null,
            object resultFrom2 = null)
        {
            var arg1 = (Input1) ResolveObject(typeof(Input1), lifeCycle1, resultFrom1);
            var arg2 = (Input2) ResolveObject(typeof(Input2), lifeCycle2, resultFrom2);
            return func(arg1, arg2);
        }


        /// <summary>
        /// Process inject attributes in every mono behaviour in scene
        /// </summary>
        public void ProcessInjectAttributeForMonoBehaviours(bool ignoreComponents = false)
        {
            if (!Application.isPlaying)
            {
                return;
            }

            allBehaviours = Resources.FindObjectsOfTypeAll<MonoBehaviour>().Where(m => m).ToArray();

            var ignoredUnityEngineScripts = allBehaviours.Where(m =>
                {
                    var type = m.GetType();
                    var ns = type.Namespace;
                    var ignored = type.GetCustomAttributes(typeof(IgnoreProcessingAttribute), true).Any();
                    return !ignored && (ns == null || !ns.StartsWith("UnityEngine"));
                })
                .ToArray();

            var sortableBehaviours = Array.FindAll(ignoredUnityEngineScripts,
                b => b.GetType().GetCustomAttributes(typeof(ProcessingOrderAttribute), true).Any());

            var nonSortableBehaviours = ignoredUnityEngineScripts.Except(sortableBehaviours);

            if (sortableBehaviours.Any())
            {
                debug.Log("Found sortableBehaviours behavior: " + sortableBehaviours.Count());

                Array.Sort(sortableBehaviours, MonobehaviourComparer.Default);

                foreach (var mono in sortableBehaviours)
                {
                    if (mono)
                    {
                        debug.Log("Process on object " + mono.GetType().Name);

                        ProcessInjectAttribute(mono);
                    }
                }
            }

            foreach (var mono in nonSortableBehaviours)
            {
                if (mono)
                {
                    debug.Log("Process on object " + mono.GetType().Name);

                    ProcessInjectAttribute(mono);
                }
            }

            //process for IRunBeforeUpdate interface
            var runBeforeUpdateComp = allBehaviours.Where(m => m is IRunBeforeUpdate).ToArray();
            foreach (var mono in runBeforeUpdateComp)
            {
                if (mono.GetType().GetCustomAttributes(typeof(IgnoreProcessingAttribute), true).Any())
                {
                    if (mono)
                    {
                        debug.Log("Process on object " + mono.GetType().Name);
                        ProcessInjectAttribute(mono);
                    }
                }

                mono.GetOrAddComponent<RunBeforeUpdate>();
            }
        }

        /// <summary>
        /// Process an unity Object for resolving every inject attributes inside
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="parent"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T CreateInstance<T>(T obj, Transform parent = null) where T : Object
        {
            T clone = null;
            Component clonedComp = null;
            GameObject clonedGameObj = null;

            if (obj is GameObject)
            {
                clone = Object.Instantiate(obj, parent);
                clonedGameObj = clone as GameObject;
            }
            else
            {
                var comp = obj as Component;
                if (comp)
                {
                    clone = Object.Instantiate(comp.gameObject, parent).GetComponent(typeof(T)) as T;
                }
                else
                {
                    clone = Object.Instantiate(obj, parent);
                }

                clonedComp = clone as Component;
                if (clonedComp)
                {
                    foreach (var mono in clonedComp.gameObject.GetComponents(typeof(MonoBehaviour)))
                    {
                        ProcessInjectAttribute(mono);
                    }

                    //activate component before returning it
                    clonedComp.gameObject.SetActive(true);
                    return clonedComp as T;
                }
            }

            ProcessInjectAttribute(clone);

            //activate gameObject before returning it
            if (clonedGameObj)
            {
                clonedGameObj.SetActive(true);
            }

            return clone;
        }


        /// <summary>
        /// Read binding data to create registerObjects
        /// </summary>
        /// <param name="bindingSetting"></param>
        public void LoadBindingSetting(BindingSetting bindingSetting)
        {
            debug.Log("From BindingSetting, {0} settings found: ", bindingSetting.defaultSettings.Length);
            //binding for default setting 
            if (bindingSetting.defaultSettings.Length > 0)
            {
                foreach (var setting in bindingSetting.defaultSettings)
                {
                    BindFromSetting(setting);
                }

                if (bindingSetting.assemblyHolder != null)
                {
                    assemblyName = bindingSetting.assemblyHolder.name;
                }

                if (bindingSetting.autoProcessSceneObjects)
                {
                    ProcessInjectAttributeForMonoBehaviours(bindingSetting.ignoreGameComponent);
                }
            }
        }


        /// <summary>
        /// Load binding setting to inject for a type T in a scene
        /// </summary>
        /// <param name="bindingSetting">custom setting</param>
        /// <typeparam name="T">Type to inject</typeparam>
        public void LoadBindingSettingForType(Type type, BindingSetting bindingSetting = null)
        {
            if (!bindingSetting)
            {
                debug.Log("Try to load binding setting for {0} from resources folders", type.Name);

                //try to load setting by name format: type_scene
                bindingSetting = MyResources.Load<BindingSetting>("{0}_{1}"
                    , type.Name
                    , CurrentSceneName);

                if (!bindingSetting)
                {
                    //try to load setting by name format: type
                    bindingSetting = MyResources.Load<BindingSetting>(type.Name);
                    if (bindingSetting)
                    {
                        debug.Log("Found default setting for type {0}", type.Name);
                    }
                }
                else
                {
                    debug.Log("Found default setting for type {0} in scene {1}", type.Name
                        , CurrentSceneName);
                }
            }

            if (!bindingSetting || bindingSetting.defaultSettings.Length == 0)
            {
                debug.Log("Not found default Binding setting for {0}!", type.Name);
                return;
            }

            foreach (var setting in bindingSetting.defaultSettings)
            {
                BindFromSetting(setting, type);
            }

            if (bindingSetting.assemblyHolder != null)
            {
                assemblyName = bindingSetting.assemblyHolder.name;
            }

            if (bindingSetting.autoProcessSceneObjects)
            {
                ProcessInjectAttributeForMonoBehaviours(bindingSetting.ignoreGameComponent);
            }
        }

        /// <summary>
        /// Load binding setting to inject for a type T in a scene
        /// </summary>
        /// <param name="bindingSetting">custom setting</param>
        /// <typeparam name="T">Type to inject</typeparam>
        public void LoadBindingSettingForType<T>(BindingSetting bindingSetting = null)
        {
            LoadBindingSettingForType(typeof(T), bindingSetting);
        }

        /// <summary>
        /// load binding setting for current scene
        /// </summary>
        /// <param name="bindingSetting">custom setting</param>
        public void LoadBindingSettingForScene(InjectIntoBindingSetting bindingSetting = null)
        {
            if (!bindingSetting)
            {
                debug.Log("Load binding setting for type from resources folders");

                //try to load setting by name format: type_scene
                bindingSetting = MyResources.Load<InjectIntoBindingSetting>(CurrentSceneName);

                if (bindingSetting)
                {
                    debug.Log("Found default setting for scene {0}", CurrentSceneName);
                }
            }

            if (!bindingSetting)
            {
                debug.Log("Binding setting should not be null!");
                return;
            }

            //binding for default setting 
            if (bindingSetting.defaultSettings != null)
            {
                debug.Log("Process binding from default setting");
                foreach (var setting in bindingSetting.defaultSettings)
                {
                    BindFromSetting(setting);
                }
            }

            if (bindingSetting.autoProcessSceneObjects)
            {
                ProcessInjectAttributeForMonoBehaviours(bindingSetting.ignoreGameComponent);
            }
        }

        public void LoadBindingSetting(string settingName)
        {
            LoadBindingSetting(UnityIoC.MyResources.Load<InjectIntoBindingSetting>(settingName));
        }

        public void LoadBindingSetting(InjectIntoBindingSetting bindingSetting)
        {
            debug.Log("From InjectIntoBindingSetting, {0} settings found: ", bindingSetting.defaultSettings.Count);
            //binding for default setting 
            if (bindingSetting.defaultSettings.Count > 0)
            {
                foreach (var setting in bindingSetting.defaultSettings)
                {
                    BindFromSetting(setting);
                }

                if (bindingSetting.assemblyHolder != null)
                {
                    assemblyName = bindingSetting.assemblyHolder.name;
                }

                if (bindingSetting.autoProcessSceneObjects)
                {
                    ProcessInjectAttributeForMonoBehaviours(bindingSetting.ignoreGameComponent);
                }
            }
        }

        public Container DefaultContainer
        {
            get { return container; }
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

        public TTypeToResolve ResolveObject<TTypeToResolve>(
            LifeCycle lifeCycle = LifeCycle.Default,
            object resolveFrom = null,
            params object[] parameters)
        {
            return (TTypeToResolve) ResolveObject(typeof(TTypeToResolve), lifeCycle, resolveFrom, parameters);
        }

        public object ResolveObject(
            Type typeToResolve,
            LifeCycle lifeCycle = LifeCycle.Default,
            object resolveFrom = null,
            params object[] parameters)
        {
            return container.ResolveObject(typeToResolve, lifeCycle, resolveFrom, parameters);
        }

        #endregion

        #region Static members

        ///<summary>
        /// cache of resolved objects
        /// </summary>
        /// 
        public static List<object> ResolvedObjects = new List<object>();

        /// <summary>
        /// cached all monobehaviours
        /// </summary>
        public static MonoBehaviour[] allBehaviours;

        /// <summary>
        /// subject to resolving object
        /// </summary>
        public static Observable<object> OnResolved
        {
            get
            {
                if (_onResolved == null)
                {
                    _onResolved = new Observable<object>();
                    _onResolved.Subscribe(obj =>
                        {
                            if (obj != null) ResolvedObjects.Add(obj);
                        }
                    );
                }

                return _onResolved;
            }
            private set { _onResolved = value; }
        }

        private static Observable<object> _onResolved;

        public static Observable<T> OnResolvedAs<T>()
        {
            var output = new Observable<T>();
            OnResolved.Subscribe(o =>
            {
                if (o.GetType() == typeof(T))
                {
                    output.Value = (T) o;
                }
            });
            return output;
        }

        /// <summary>
        /// Call POST Method to REST api.
        /// </summary>
        public static IEnumerator Post<T>(
            string link,
            object request,
            Action<T> result = null,
            Action<string> error = null)
        {
            string jsonString = request == null ? "{}" : JsonUtility.ToJson(request);

            Dictionary<string, string> header = new Dictionary<string, string>
            {
                {"Content-Type", "application/json"}
            };

            byte[] body = Encoding.UTF8.GetBytes(jsonString);

            WWW www = new WWW(link, body, header);

            yield return www;

            if (www.error != null)
            {
                Debug.Log("API Call Failed: " + www.error);
                if (error != null)
                {
                    error(www.error);
                }
            }
            else
            {
                Debug.Log("API Call successful.");
                var fromJson = ResolveFromJson<T>(www.text);
                if (result != null && fromJson != null)
                {
                    result(fromJson);
                }
            }
        }

        /// <summary>
        /// Call GET Method to REST api.
        /// </summary>
        public static IEnumerator Get<T>(
            string link,
            Action<T> result = null,
            Action<string> error = null)
        {
            UnityWebRequest www = UnityWebRequest.Get(link);
            var async = www.SendWebRequest();

            while (!async.isDone)
            {
                yield return null;
            }

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                if (error != null)
                {
                    error(www.error);
                }
            }
            else
            {
                // Show results as text
                Debug.Log(www.downloadHandler.text);
                T t = ResolveFromJson<T>(www.downloadHandler.text);
                if (result != null && t != null)
                {
                    result(t);
                }
            }
        }

        public static T ResolveFromJson<T>(string json)
        {
            if (!Context.Initialized)
            {
                Context.GetDefaultInstance(typeof(T));
            }

            var obj = JsonUtility.FromJson<T>(json);
            if (obj != null)
            {
                ResolvedObjects.Add(obj);
                OnResolved.Value = obj;
                return obj;
            }

            return Resolve<T>();
        }

        public static T ResolveFromPool<T>(
            Transform parentObject = null,
            int preload = 0,
            object resolveFrom = null,
            params object[] parameters) where T : Component
        {
            if (!Context.Initialized)
            {
                Context.GetDefaultInstance(typeof(T));
            }

            if (preload > 0 && preload > Pool<T>.List.Count)
            {
                PreloadFromPool<T>(preload, parentObject, resolveFrom, parameters);
            }

            var instanceFromPool = Pool<T>.List.GetInstanceFromPool<T>(parentObject, resolveFrom, parameters);
            instanceFromPool.gameObject.SetActive(true);

            return instanceFromPool;
        }

        public static void PreloadFromPool<T>(int preload, Transform parentObject = null, object resolveFrom = null,
            params object[] parameters)
            where T : Component
        {
            if (!Context.Initialized)
            {
                Context.GetDefaultInstance(typeof(T));
            }

            Pool<T>.List.Preload(preload, parentObject, resolveFrom, parameters);
        }


        public static List<T> GetFromPool<T>() where T : Component
        {
            return Pool<T>.List;
        }

        public static ObjectContext FromObject(object obj, BindingSetting data = null)
        {
            return new ObjectContext(obj, GetDefaultInstance(obj), data);
        }

        public static ObjectContext<T> FromObject<T>(BindingSetting data = null)
        {
            return new ObjectContext<T>(GetDefaultInstance(typeof(T)), data);
        }

        private static Context defaultInstance;

//        private static Dictionary<Type, Context> _instances = new Dictionary<Type, Context>();

        public static bool Initialized
        {
            get { return defaultInstance != null && defaultInstance.initialized; }
        }

        public static Context DefaultInstance
        {
            get { return GetDefaultInstance(); }
            set { defaultInstance = value; }
        }

        /// <summary>
        /// Clone an object from an existing one
        /// </summary>
        /// <param name="origin"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Instantiate<T>(T origin) where T : Component
        {
            return DefaultInstance.CreateInstance(origin);
        }

        /// <summary>
        /// Clone an object from an existing one
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="parent"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Instantiate<T>(T origin, Object parent) where T : Component
        {
            return DefaultInstance.CreateInstance(origin, parent as Transform);
        }

        /// <summary>
        /// Clone an object from an existing one
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static GameObject Instantiate(GameObject origin, Object parent)
        {
            return DefaultInstance.CreateInstance(origin, parent as Transform);
        }

        /// <summary>
        /// Get default-static general purposes context
        /// </summary>
        /// <param name="context"></param>
        /// <param name="automaticBind"></param>
        /// <param name="recreate"></param>
        /// <returns></returns>
        public static Context GetDefaultInstance(object context, bool automaticBind = true,
            bool recreate = false)
        {
            return GetDefaultInstance(context.GetType(), automaticBind, recreate);
        }

        public static Context GetDefaultInstance(Type type = null, bool automaticBind = true,
            bool recreate = false)
        {
            if (defaultInstance == null || recreate)
            {
                defaultInstance = new Context(type, automaticBind);
            }

            return defaultInstance;
        }

        public static void DisposeDefaultInstance()
        {
            //remove cache of resolved objects
            ResolvedObjects.Clear();

            //recycle the observable
            if (!OnResolved.IsDisposed)
            {
                OnResolved.Dispose();
                OnResolved = null;
            }

            //recycle the defaultInstance
            if (defaultInstance != null)
            {
                defaultInstance.Dispose();
                defaultInstance = null;
            }
        }

        /// <summary>
        /// Create a new brand object as [transient], existing object as [singleton] or getting [component] from inside gameObject
        /// </summary>
        /// <param name="typeToResolve"></param>
        /// <param name="lifeCycle"></param>
        /// <param name="resolveFrom"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static object Resolve(
            Type typeToResolve,
            LifeCycle lifeCycle = LifeCycle.Default,
            object resolveFrom = null,
            params object[] parameters)
        {
            var resolveObject = GetDefaultInstance(typeToResolve)
                .ResolveObject(typeToResolve, lifeCycle, resolveFrom, parameters);

            if (resolveObject != null)
            {
                //trigger the subject
                OnResolved.Value = resolveObject;
            }


            return resolveObject;
        }

        /// <summary>
        /// Resolve objects with parameters for constructors
        /// </summary>
        /// <param name="parameters"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Resolve<T>(
            params object[] parameters)
        {
            return (T) Resolve(typeof(T), LifeCycle.Transient, null, parameters);
        }

        /// <summary>
        /// Create a new brand object as [transient], existing object as [singleton] or [component] which has been gotten from inside gameObject
        /// </summary>
        public static T Resolve<T>(
            LifeCycle lifeCycle = LifeCycle.Default,
            object resolveFrom = null,
            params object[] parameters)
        {
            return (T) Resolve(typeof(T), lifeCycle, resolveFrom, parameters);
        }

        /// <summary>
        /// Create a new brand object as [transient], existing object as [singleton] or [component] which has been gotten from inside gameObject
        /// </summary>
        public static T Resolve<T>(
            Transform parents,
            LifeCycle lifeCycle = LifeCycle.Default,
            object resolveFrom = null,
            params object[] parameters)
            where T : Component
        {
            var obj = Resolve(typeof(T), lifeCycle, resolveFrom, parameters) as T;
            if (parents && obj)
            {
                obj.transform.SetParent(parents);
            }

            if (obj != null)
            {
                obj.gameObject.SetActive(true);
            }

            return obj;
        }

        /// <summary>
        /// Set value for a property by its name
        /// </summary>
        /// <param name="inputObject"></param>
        /// <param name="propertyName"></param>
        /// <param name="propertyVal"></param>
        public static void SetPropertyValue(object inputObject, string propertyName, object propertyVal)
        {
            Type type = inputObject.GetType();
            PropertyInfo propertyInfo = type.GetProperty(propertyName);

            var targetType = IsNullableType(propertyInfo.PropertyType)
                ? Nullable.GetUnderlyingType(propertyInfo.PropertyType)
                : propertyInfo.PropertyType;

            propertyVal = Convert.ChangeType(propertyVal, targetType);
            propertyInfo.SetValue(inputObject, propertyVal, null);
        }

        public static bool IsNullableType(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        /// <summary>
        /// Bind a TAbstract with a TConcrete
        /// </summary>
        /// <param name="lifeCycle"></param>
        /// <typeparam name="TAbstract"></typeparam>
        /// <typeparam name="TConcrete"></typeparam>
        public static void Bind<TAbstract, TConcrete>(LifeCycle lifeCycle = LifeCycle.Default)
        {
            GetDefaultInstance(typeof(TAbstract)).container.Bind<TAbstract, TConcrete>(lifeCycle);
        }

        /// <summary>
        /// Bind a Type with itself
        /// </summary>
        /// <param name="lifeCycle"></param>
        /// <typeparam name="T"></typeparam>
        public static void Bind<T>(LifeCycle lifeCycle = LifeCycle.Default)
        {
            GetDefaultInstance(typeof(T)).container.Bind<T>(lifeCycle);
        }

        /// <summary>
        /// Bind to an instance as singleton
        /// </summary>
        /// <param name="instance"></param>
        public static void Bind(object instance)
        {
            var typeToResolve = instance.GetType();
            GetDefaultInstance(typeToResolve).Bind(typeToResolve, instance);
        }

        /// <summary>
        /// Bind from a external setting file
        /// </summary>
        /// <param name="bindingSetting"></param>
        public static void Bind(BindingSetting bindingSetting)
        {
            if (bindingSetting.assemblyHolder)
            {
                var customAssembly = Assembly.Load(bindingSetting.assemblyHolder.name);
                if (customAssembly != null)
                {
                    var type = customAssembly.GetTypes().FirstOrDefault();
                    GetDefaultInstance(type);
                }
            }

            GetDefaultInstance().LoadBindingSetting(bindingSetting);
        }

        /// <summary>
        /// Bind from a external setting file
        /// </summary>
        /// <param name="bindingSetting"></param>
        public static void Bind(InjectIntoBindingSetting bindingSetting)
        {
            if (bindingSetting.assemblyHolder)
            {
                var customAssembly = Assembly.Load(bindingSetting.assemblyHolder.name);
                if (customAssembly != null)
                {
                    var type = customAssembly.GetTypes().FirstOrDefault();
                    GetDefaultInstance(type);
                }
            }

            GetDefaultInstance().LoadBindingSetting(bindingSetting);
        }

        /// <summary>
        /// Get a component from a mono behaviour at a given path
        /// </summary>
        /// <param name="type"></param>
        /// <param name="component"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Component GetComponent(
            Type type,
            MonoBehaviour component,
            string path
        )
        {
            var componentAttribute = ComponentAttribute.DefaultInstance;
            componentAttribute.Path = path;
            return componentAttribute.GetComponent(component, type);
        }

        /// <summary>
        /// Get a component from a gameObject at a given path
        /// </summary>
        /// <param name="type"></param>
        /// <param name="gameObject"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Component GetComponent(
            Type type,
            GameObject gameObject,
            string path
        )
        {
            var comp = gameObject.GetComponent(type) as MonoBehaviour;
            if (comp)
            {
                //resolve with path
                var component = GetComponent(type, comp, path);
                if (component)
                    return component;
            }

            //resolve without path
            return Resolve(type) as Component;
        }

        /// <summary>
        /// Get a component from a gameObject at a given path
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="path"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetComponent<T>(
            GameObject gameObject,
            string path
        )
            where T : class
        {
            var type = typeof(T);
            var comp = gameObject.GetComponent(type) as MonoBehaviour;
            if (comp)
            {
                //resolve with path
                var component = GetComponent(type, comp, path) as T;
                if (component != null)
                    return component;
            }

            //resolve without path
            return Resolve(type) as T;
        }

        /// <summary>
        /// Get a component from a mono behaviour at a given path
        /// </summary>
        /// <param name="component"></param>
        /// <param name="path"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetComponent<T>(
            MonoBehaviour component,
            string path
        )
            where T : class
        {
            return GetComponent(typeof(T), component, path) as T;
        }

        /// <summary>
        /// Get an C# object from a mono behaviour at a given type
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object GetObject(
            MonoBehaviour obj,
            Type type
        )
        {
            return GetDefaultInstance(type).GetObjectFromGameObject(obj, type);
        }

        /// <summary>
        /// Get an C# object from a mono behaviour at a given type
        /// </summary>
        /// <param name="obj"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static object GetObject<T>(
            MonoBehaviour obj
        )
        {
            Type type = typeof(T);
            return GetDefaultInstance(type).GetObjectFromGameObject(obj, type);
        }

        #endregion
    }
}