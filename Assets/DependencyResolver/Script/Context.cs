/**
 * Author:    Vinh Vu Thanh
 * This class is a part of Universal Resolver project that can be downloaded free at 
 * https://github.com/game-libgdx-unity/UnityEngine.IoC
 * (c) Copyright by MrThanhVinh168@gmail.com
 **/

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace UnityIoC
{
    public partial class Context
    {
        #region Variables & Constants

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
        /// Automatic binding a external setting file with the same name as the assembly's
        /// </summary>
        private bool autoFindSetting;

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
        private string assemblyName = "";

        /// <summary>
        /// Allow to search on objects in the scene for the needed prefabs
        /// </summary>
        private bool searchPrefabFromScene;

        #endregion

        #region Constructors

        /// <summary>
        /// Pass a object belong to the assembly you want to process
        /// </summary>
        /// <param name="target">the object</param>
        /// <param name="autoFindBindSetting">if true, will load bindingsetting and process all game object for inject attribute</param>
        public Context(object target,
            bool autoFindBindSetting = true,
            bool searchPrefabFromScene = false,
            bool disableProcessAllBehaviours = false,
            string[] assetPaths = null)
        {
            this.searchPrefabFromScene = searchPrefabFromScene;
            Initialize(target.GetType(), autoFindBindSetting, disableProcessAllBehaviours, assetPaths);
        }

        /// <summary>
        /// Pass a type belong to the assembly you want to process
        /// </summary>
        /// <param name="target">the object</param>
        /// <param name="autoFindBindSetting">if true, will load bindingsetting and process all game object for inject attribute</param>
        public Context(Type typeInTargetedAssembly, bool autoFindBindSetting = true,
            bool searchPrefabFromScene = false,
            bool disableProcessAllBehaviours = false, string[] assetPaths = null)
        {
            this.searchPrefabFromScene = searchPrefabFromScene;
            Initialize(typeInTargetedAssembly, autoFindBindSetting, disableProcessAllBehaviours, assetPaths);
        }


        /// <summary>
        /// Use the default assembly to process
        /// </summary>
        /// <param name="target">the object</param>
        /// <param name="autoFindBindSetting">if true, will load bindingsetting and process all game object for inject attribute</param>
        public Context(BaseBindingSetting setting,
            bool searchPrefabFromScene = false,
            bool disableProcessAllBehaviours = false,
            string[] assetPaths = null)
        {
            this.searchPrefabFromScene = searchPrefabFromScene;
            assemblyName = setting.name;
            Initialize(null, false, disableProcessAllBehaviours, assetPaths);
            LoadBindingSetting(setting);
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

            if (!autoFindSetting)
            {
                return;
            }

            //try to load a default InjectIntoBindingSetting setting for context
            var injectIntoBindingSetting =
                Resources.Load<InjectIntoBindingSetting>(CurrentAssembly.GetName().Name);

            if (injectIntoBindingSetting)
            {
                debug.Log("Found InjectIntoBindingSetting for assembly");
                LoadBindingSetting(injectIntoBindingSetting);
                return;
            }

            //try to get the default InjectIntoBindingSetting setting for current scene
            var sceneInjectIntoBindingSetting = Resources.Load<InjectIntoBindingSetting>(
                String.Format("{0}_{1}", CurrentAssembly.GetName().Name, SceneManager.GetActiveScene().name)
            );

            if (sceneInjectIntoBindingSetting)
            {
                debug.Log("Found InjectIntoBindingSetting for scene");
                LoadBindingSetting(sceneInjectIntoBindingSetting);
                return;
            }

            //try to load a default BindingSetting setting for context
            var bindingSetting =
                Resources.Load<BindingSetting>(CurrentAssembly.GetName().Name);

            if (bindingSetting)
            {
                debug.Log("Found binding setting for assembly");
                LoadBindingSetting(bindingSetting);
                return;
            }

            //try to get the default BindingSetting setting for current scene
            var sceneBindingSetting = Resources.Load<BindingSetting>(
                String.Format("{0}_{1}", CurrentAssembly.GetName().Name, SceneManager.GetActiveScene().name)
            );

            if (sceneBindingSetting)
            {
                debug.Log("Found binding setting for scene");
                LoadBindingSetting(sceneBindingSetting);
                return;
            }


            if (DisableProcessAllBehaviour)
            {
                return;
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

                    bindingSetting.Prefab = (GameObject) bindingSetting.ImplementedTypeHolder;
                }
                else
                {
                    bindingSetting.ImplementedType =
                        GetTypeFromCurrentAssembly(bindingSetting.ImplementedTypeHolder.name);

                    bindingSetting.Prefab = null;
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
        public Type GetTypeFromCurrentAssembly(string className)
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

            debug.Log(String.Format("Found {0} method to process", methods.Length));

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

            debug.Log(String.Format("Found {0} property to process", properties.Length));

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

                    //link view object to data object
                    if (mono is Component && component != null)
                    {
                        if (!DataViewBindings.ContainsKey(component))
                        {
                            DataViewBindings[component] = new HashSet<object>();
                        }

                        DataViewBindings[component].Add(mono);
                    }

                    //try to use IObjectResolvable to resolve objects
                    if (component == null && !property.PropertyType.IsSubclassOf(typeof(Component)))
                    {
                        component = GetObjectFromGameObject(mono, property.PropertyType);
                    }

                    //try to use IComponentResolvable to resolve objects
                    if (component == null)
                    {
                        component = ResolveFromHierarchy(mono, property.PropertyType, inject);
                    }

                    if (component != null)
                    {
                        if (inject.LifeCycle == LifeCycle.Singleton ||
                            (inject.LifeCycle & LifeCycle.Singleton) == LifeCycle.Singleton)
                        {
                            container.BindInstance(property.PropertyType, component);
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

        private object TryGetObjectFromCache(InjectBaseAttribute inject, Type type)
        {
            if ((inject.LifeCycle == LifeCycle.Cache ||
                 (inject.LifeCycle & LifeCycle.Cache) == LifeCycle.Cache) &&
                CacheOfResolvedObjects.Count > 0)
            {
                return GetObjectFromCache(type);
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
                debug.Log(String.Format("Found {0} fieldInfo to process", fieldInfos.Length));

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

                bool defaultValue = false;
                //only process a field if the field's value is not set yet
                var value = field.GetValue(mono);

                if (type.IsSubclassOf(typeof(MonoBehaviour)) && value == null)
                {
                    defaultValue = true;
                }
                else
                {
                    var overrideAttribute =
                        field.GetCustomAttributes(typeof(OverrideAttribute), true).FirstOrDefault();

                    if (overrideAttribute == null)
                    {
                        if (field.FieldType.IsArray)
                        {
                            if (value != null)
                            {
                                defaultValue = value != value.DefaultValue();
                                var array = value as Array;
                                if (array.Length == 0)
                                {
                                    defaultValue = true;
                                }
                            }
                        }
                        else
                        {
                            if (value != null)
                            {
                                defaultValue = value != value.DefaultValue();
                            }
                        }
                    }
                }

                if (defaultValue)
                {
                    debug.Log(String.Format("Don't set value for field {0} due to non-default value", field.Name));

                    //check to bind this instance as singleton
                    if (inject.LifeCycle == LifeCycle.Singleton ||
                        (inject.LifeCycle & LifeCycle.Singleton) == LifeCycle.Singleton)
                    {
                        if (type.IsSubclassOf(typeof(MonoBehaviour)))
                        {
                            if (container.registeredTypes.Contains(type))
                            {
                                value = container.ResolveObject(type, inject.LifeCycle);
                            }

                            if (value == null)
                            {
                                value = AllBehaviours.FirstOrDefault(b =>
                                    b.GetType() == type || b.GetType().IsSubclassOf(type));
                            }

                            if (value != null)
                            {
                                debug.Log(String.Format("Bind instance for field {0}", field.Name));
                                container.BindInstance(field.FieldType, value);
                                field.SetValue(mono, value);
                            }
                        }
                    }

                    continue;
                }

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
                else if (type.IsGenericType)
                {
                    var genericTypeDefinition = type.GetGenericTypeDefinition();
                    if (
                        genericTypeDefinition == typeof(IEnumerable<>) ||
                        genericTypeDefinition == typeof(ICollection<>) ||
                        Setting.UseSetForCollection && genericTypeDefinition == typeof(ISet<>) ||
                        !Setting.UseSetForCollection && genericTypeDefinition == typeof(IList<>)
                    )
                    {
                        var argurmentType = type.GetGenericArguments().FirstOrDefault();
                        var collection = Pool.GetList(argurmentType);
                        debug.Log("Resolved type {0} of field {1} from Pool<T>", type, field.Name);
                        field.SetValue(mono, collection);
                        continue;
                    }
                }

                object component = null;

                //try get from cache if conditions are met
                component = TryGetObjectFromCache(inject, type);

                //link view object to data object
                if (mono is Component && component != null)
                {
                    if (!DataViewBindings.ContainsKey(component))
                    {
                        DataViewBindings[component] = new HashSet<object>();
                    }

                    DataViewBindings[component].Add(mono);
                }

                //try to use IObjectResolvable to resolve objects
                if (component == null && !type.IsSubclassOf(typeof(Component)))
                {
                    component = GetObjectFromGameObject(mono, type);
                }

                //try to use IComponentResolvable to resolve objects
                if (component == null)
                {
                    component = ResolveFromHierarchy(mono, type, inject);
                }

                if (component != null)
                {
                    //if the life cycle is singleton, bind the instance of the Type with this component
                    if (inject.LifeCycle == LifeCycle.Singleton ||
                        (inject.LifeCycle & LifeCycle.Singleton) == LifeCycle.Singleton)
                    {
                        container.BindInstance(type, component);
                    }

                    field.SetValue(mono, component);

                    continue;
                }


                debug.Log("IComponentResolvable fails to resolve {0} of field {1}", type, field.Name);

                //resolve object as [Singleton], [Transient] or [AsComponent] if component attribute fails to resolve
                field.SetValue(mono,
                    container.ResolveObject(
                        type,
                        inject.LifeCycle,
                        mono));
            }
        }

        /// <summary>
        /// Convert Component[] to a CustomType[]
        /// </summary>
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
        private object ResolveFromHierarchy(object mono, Type type, InjectBaseAttribute injectAttribute)
        {
            var behaviour = mono as MonoBehaviour;

            if (behaviour == null) return null;

            //output component
            Component component = null;

            if (type.IsSubclassOf(typeof(Component)))
            {
                //try get/add component with IInjectComponent interface
                //resolve by IComponentResolvable to get component to the gameobject
                var componentResolvable = injectAttribute as IComponentResolvable;
                if (componentResolvable != null)
                {
                    component = componentResolvable.GetComponent(behaviour, type);
                    //unable to get it from gameObject
                    if (component != null)
                    {
                        if (injectAttribute.LifeCycle == LifeCycle.Prefab)
                        {
                            component.gameObject.SetActive(false);
                        }

                        return component;
                    }
                }
            }

            if (type.IsSubclassOf(typeof(ScriptableObject)))
            {
                //resolve by IObjectResolvable to get the scriptableObject
                var objectResolvable = injectAttribute as IObjectResolvable;
                if (objectResolvable != null)
                {
                    var obj = objectResolvable.GetObject(type);
                    if (obj != null)
                    {
                        return obj;
                    }
                }
            }

            return null;
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


        private void Initialize(Type target = null, bool automaticBinding = false,
            bool disableProcessAllBehaviours = false, string[] assetPaths = null)
        {
            if (!Initialized)
            {
                defaultInstance = this;
            }

            DisableProcessAllBehaviour = disableProcessAllBehaviours;
            autoFindSetting = automaticBinding;
            if (assetPaths != null && assetPaths.Length > 0)
            {
                assetPaths.CopyTo(this.assetPaths, 0);
            }

            targetType = target;
            container = new Container(this);

            InitialProcess();

            initialized = true;
        }

        #endregion

        #region Public members

        /// <summary>
        /// disable every process related to AllBehaviours
        /// </summary>
        public bool DisableProcessAllBehaviour { get; set; }

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
                            if (String.IsNullOrEmpty(assemblyName))
                            {
                                _currentAssembly = Assembly.Load(Setting.AssemblyName);
                            }
                            else
                            {
                                _currentAssembly = Assembly.Load(assemblyName);
                            }
                        }
                        catch (Exception ex)
                        {
                            //if all the AssemblyNames are incorrect
                            _currentAssembly = Assembly.GetExecutingAssembly();
                        }
                    }
                    else
                    {
                        _currentAssembly = targetType.Assembly;
                    }
                }

                return _currentAssembly;
            }
        }

        public void ResolveAction<T>(Action<T> action, LifeCycle lifeCycle = LifeCycle.Default,
            Type resultFrom = null)
        {
            var arg = (T) ResolveObject(typeof(T), lifeCycle, resultFrom);
            action(arg);
        }

        public void ResolveAction<T1, T2>(Action<T1, T2> action,
            LifeCycle lifeCycle1 = LifeCycle.Default,
            LifeCycle lifeCycle2 = LifeCycle.Default,
            Type resultFrom1 = null, Type resultFrom2 = null)
        {
            var arg1 = (T1) ResolveObject(typeof(T1), lifeCycle1, resultFrom1);
            var arg2 = (T2) ResolveObject(typeof(T1), lifeCycle2, resultFrom2);
            action(arg1, arg2);
        }

        public Result ResolveFunc<Input, Result>(Func<Input, Result> func, LifeCycle lifeCycle = LifeCycle.Default,
            Type resultFrom = null)
        {
            var arg = (Input) ResolveObject(typeof(Input), lifeCycle, resultFrom);
            return func(arg);
        }

        public Result ResolveFunc<Input1, Input2, Result>(Func<Input1, Input2, Result> func,
            LifeCycle lifeCycle1 = LifeCycle.Default,
            LifeCycle lifeCycle2 = LifeCycle.Default,
            Type resultFrom1 = null,
            Type resultFrom2 = null)
        {
            var arg1 = (Input1) ResolveObject(typeof(Input1), lifeCycle1, resultFrom1);
            var arg2 = (Input2) ResolveObject(typeof(Input2), lifeCycle2, resultFrom2);
            return func(arg1, arg2);
        }

        /// <summary>
        /// Get an object which is resolved by Context, following the rule of LIFO 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object GetObjectFromCache(Type type)
        {
            if (!CacheOfResolvedObjects.ContainsKey(type))
            {
                return null;
            }

            var hashSet = CacheOfResolvedObjects[type];

            if (hashSet.Count == 0)
            {
                return null;
            }

            return hashSet.Last(
                o => type.IsInterface && type.IsInstanceOfType(o)
                     || !type.IsInterface && o.GetType() == type);
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

            if (DisableProcessAllBehaviour)
            {
                return;
            }

            var processableUnityEngineScripts = AllBehaviours.Where(m =>
                {
                    var type = m.GetType();
                    var ns = type.Namespace;

                    if (ns != null && ns.StartsWith("UnityEngine"))
                    {
                        return false;
                    }

                    return !type.GetCustomAttributes(typeof(IgnoreProcessingAttribute), true).Any();
                })
                .ToArray();

            var sortableBehaviours = Array.FindAll(processableUnityEngineScripts,
                b => b.GetType().GetCustomAttributes(typeof(ProcessingOrderAttribute), true).Any());

            var nonSortableBehaviours = processableUnityEngineScripts.Except(sortableBehaviours);

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

            //Obsoleted: now you just can write code in Start() after all dependencies got resolved.
            //IRunBeforeUpdate is now no more necessary.
            //process for IRunBeforeUpdate interface 
//            var runBeforeUpdateComp = allBehaviours.Where(m => m is IRunBeforeUpdate).ToArray();
//            foreach (var mono in runBeforeUpdateComp)
//            {
//                if (mono.GetType().GetCustomAttributes(typeof(IgnoreProcessingAttribute), true).Any())
//                {
//                    if (mono)
//                    {
//                        debug.Log("Process on object " + mono.GetType().Name);
//                        ProcessInjectAttribute(mono);
//                    }
//                }
//
//                mono.GetOrAddComponent<RunBeforeUpdate>();
//            }
        }

        /// <summary>
        /// Clone an object with all [inject] attributes resolved inside
        /// </summary>
        /// <param name="sample"></param>
        /// <param name="parent"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T CreateInstance<T>(T sample, Transform parent = null) where T : Object
        {
            T clone = null;
            Component clonedComp = null;
            GameObject clonedGameObj = null;
            GameObject gameObject = sample as GameObject;
            if (gameObject)
            {
                clone = Object.Instantiate(sample);
                clonedGameObj = clone as GameObject;
                clonedGameObj.transform.SetParent(parent != null ? parent : gameObject.transform.parent);
            }
            else
            {
                var comp = sample as Component;
                if (comp)
                {
                    clone = Object.Instantiate(comp.gameObject).GetComponent(typeof(T)) as T;
                }
                else
                {
                    clone = Object.Instantiate(sample, parent);
                }

                clonedComp = clone as Component;

                if (clonedComp)
                {
                    if (comp)
                    {
                        clonedComp.transform.SetParent(parent != null ? parent : comp.gameObject.transform.parent);
                    }

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
        /// Read base binding setting type to decide which method will be used to process the data
        /// </summary>
        /// <param name="bindingSetting"></param>
        public void LoadBindingSetting(BaseBindingSetting bindingSetting)
        {
            var injectIntoBindingSetting = bindingSetting as InjectIntoBindingSetting;
            if (injectIntoBindingSetting)
            {
                LoadBindingSetting(injectIntoBindingSetting);
                return;
            }

            var bs = bindingSetting as BindingSetting;
            if (bs)
            {
                LoadBindingSetting(bs);
            }
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

                if (bindingSetting.autoProcessSceneObjects && !DisableProcessAllBehaviour)
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

            if (bindingSetting.autoProcessSceneObjects && !DisableProcessAllBehaviour)
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

            if (bindingSetting.autoProcessSceneObjects && !DisableProcessAllBehaviour)
            {
                ProcessInjectAttributeForMonoBehaviours(bindingSetting.ignoreGameComponent);
            }
        }

        public void LoadBindingSetting(string settingName)
        {
            LoadBindingSetting(MyResources.Load<InjectIntoBindingSetting>(settingName));
        }

        public void LoadBindingSetting(InjectIntoBindingSetting bindingSetting)
        {
            if (bindingSetting == null)
            {
                return;
            }

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

                if (bindingSetting.autoProcessSceneObjects && !DisableProcessAllBehaviour)
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
            container.BindInstance(typeToResolve, instance);
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
            Type resolveFrom = null,
            params object[] parameters)
        {
            return (TTypeToResolve) ResolveObject(typeof(TTypeToResolve), lifeCycle, resolveFrom, parameters);
        }

        public object ResolveObject(
            Type typeToResolve,
            LifeCycle lifeCycle = LifeCycle.Default,
            Type resolveFrom = null,
            params object[] parameters)
        {
            return container.ResolveObject(typeToResolve, lifeCycle, resolveFrom, parameters);
        }

        #endregion

        #region Static members

        /// <summary>
        /// If the Context static API is ready to use
        /// </summary>
        public static bool Initialized
        {
            get { return defaultInstance != null && defaultInstance.initialized; }
        }

        /// <summary>
        /// Get or Init the Context for the default assembly that Unity3d automatically created for your scripts
        /// </summary>
        public static Context DefaultInstance
        {
            get { return GetDefaultInstance(); }
            set { defaultInstance = value; }
        }

        ///<summary>
        /// cache of resolved objects
        /// </summary>
        public static Dictionary<Type, HashSet<object>>
            CacheOfResolvedObjects = new Dictionary<Type, HashSet<object>>();

        /// <summary>
        /// Cache of data binding of data layer & view layer
        /// </summary>
        public static Dictionary<object, HashSet<object>> DataViewBindings = new Dictionary<object, HashSet<object>>();

        /// <summary>
        /// General pools for mono-behaviour-based Views for view recyclable purposes. 
        /// </summary>
        public static ViewPool ViewPools = new ViewPool();

        /// <summary>
        /// cached all monobehaviours
        /// </summary>
        private static MonoBehaviour[] _allBehaviours;

        /// <summary>
        /// Get all monobehaviours from behaviour cache 
        /// </summary>
        public static MonoBehaviour[] AllBehaviours
        {
            get
            {
                if (_allBehaviours == null)
                {
                    _allBehaviours = Resources.FindObjectsOfTypeAll<MonoBehaviour>();
                }

                return _allBehaviours;
            }
        }


        private static Observable<Exception> _onError;

        /// <summary>
        /// just a private variable
        /// </summary>
        private static Observable<object> _onResolved;

        /// <summary>
        /// subject to resolving object
        /// </summary>
        internal static Observable<object> onResolved
        {
            get
            {
                if (_onResolved == null)
                {
                    _onResolved = new Observable<object>();
                    _onResolved.Subscribe(obj =>
                        {
                            if (obj != null)
                            {
                                Type type = obj.GetType();
                                if (!CacheOfResolvedObjects.ContainsKey(type))
                                {
                                    CacheOfResolvedObjects[type] = new HashSet<object>();
                                }

                                //add this obj to internal cache
                                CacheOfResolvedObjects[type].Add(obj);

                                //Create view for this obj (in case it's necessary)
                                CreateViewFromData(obj);
                            }
                        }
                    );
                }

                return _onResolved;
            }
        }


        /// <summary>
        /// just a private variable
        /// </summary>
        private static Observable<object> _onUpdated;

        /// <summary>
        /// subject to resolving object
        /// </summary>
        internal static Observable<object> onUpdated
        {
            get
            {
                if (_onUpdated == null)
                {
                    _onUpdated = new Observable<object>();
                }

                return _onUpdated;
            }
        }

        /// <summary>
        /// just a private variable
        /// </summary>
        private static Observable<object> _onViewResolved;

        /// <summary>
        /// subject to resolving object
        /// </summary>
        internal static Observable<object> onViewResolved
        {
            get
            {
                if (_onViewResolved == null)
                {
                    _onViewResolved = new Observable<object>();
                }

                return _onViewResolved;
            }
        }

        /// <summary>
        /// just a private variable
        /// </summary>
        private static Observable<object> _onDisposed;

        /// <summary>
        /// subject to resolving object
        /// </summary>
        public static Observable<object> onDisposed
        {
            get
            {
                if (_onDisposed == null)
                {
                    _onDisposed = new Observable<object>();
                    _onDisposed.Subscribe(obj =>
                        {
                            if (obj != null)
                            {
                                Type type = obj.GetType();
                                if (CacheOfResolvedObjects.ContainsKey(type))
                                {
                                    CacheOfResolvedObjects[type].Remove(obj);
                                    DataViewBindings.Remove(obj);
//                                    Pool.Remove(obj);
                                }
                            }
                        }
                    );
                }

                return _onDisposed;
            }
        }

        /// <summary>
        /// cached all root gameObjects
        /// </summary>
        private static GameObject[] _rootgameObjects;

        /// <summary>
        /// Get all root gameObjects from cache
        /// </summary>
        public static GameObject[] AllRootgameObjects
        {
            get
            {
                if (_rootgameObjects == null || _rootgameObjects.Length == 0)
                {
                    var gameObjectList = new List<GameObject>();
                    for (int i = 0; i < SceneManager.sceneCount; i++)
                    {
                        gameObjectList.AddRange(SceneManager.GetSceneAt(i).GetRootGameObjects());
                    }

                    _rootgameObjects = gameObjectList.ToArray();
                }

                return _rootgameObjects;
            }
        }

        public static Observable<T> OnResolved<T>()
        {
            var output = new Observable<T>();
            onResolved.Subscribe(o =>
            {
                if (o is T obj)
                {
                    output.Value = obj;
                }
            });
            return output;
        }

        public static Observable<T> OnUpdated<T>()
        {
            var output = new Observable<T>();
            onUpdated.Subscribe(o =>
            {
                if (o is T obj)
                {
                    output.Value = obj;
                }
            });
            return output;
        }

        public static Observable<T> OnDisposed<T>()
        {
            var output = new Observable<T>();
            onDisposed.Subscribe(o =>
            {
                if (o is T obj)
                {
                    output.Value = obj;
                }
            });
            return output;
        }

        public static Observable<T> OnViewResolved<T>()
        {
            var output = new Observable<T>();
            onViewResolved.Subscribe(o =>
            {
                if (o is T obj)
                {
                    output.Value = obj;
                }
            });
            return output;
        }

        /// <summary>
        /// Call POST Method to REST api for getting objects which are parsed from json.
        /// </summary>
        public static IEnumerator Post<T>(
            string link,
            object request,
            Action<T> result = null,
            Action<string> error = null)
        {
            string jsonString = request == null ? "{}" : JsonUtility.ToJson(request);
            yield return Post(link, jsonString, text =>
            {
                T t = ResolveFromJson<T>(text);
                if (result != null)
                {
                    result(t);
                }
            }, error);
        }

        /// <summary>
        /// Call POST Method to REST api for raw string result.
        /// </summary>
        public static IEnumerator Post(
            string link,
            string request,
            Action<string> result = null,
            Action<string> error = null)
        {
            UnityWebRequest www = UnityWebRequest.Put(link, request);
            www.SetRequestHeader("Content-Type", "application/json");
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
                if (result != null)
                {
                    result(www.downloadHandler.text);
                }
            }
        }

        /// <summary>
        /// Call GET Method to REST api for parsed results from json.
        /// </summary>
        public static IEnumerator GetObjectsFromCache<T>(
            string link,
            Action<T> result = null,
            Action<string> error = null)
        {
            yield return GetObjectsFromCache(link, text =>
            {
                T t = ResolveFromJson<T>(text);
                if (result != null)
                {
                    result(t);
                }
            }, error);
        }

        /// <summary>
        /// Call GET Method to REST api for raw string result.
        /// </summary>
        public static IEnumerator GetObjectsFromCache(
            string link,
            Action<string> result = null,
            Action<string> error = null)
        {
            UnityWebRequest www = UnityWebRequest.Get(link);
            www.SetRequestHeader("Content-Type", "application/json");
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
                if (result != null)
                {
                    result(www.downloadHandler.text);
                }
            }
        }

        /// <summary>
        /// Update an object by a Json patch
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="json"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T PatchObjectFromJson<T>(object obj, string json) where T : class
        {
            JsonUtility.FromJsonOverwrite(json, obj);
            Update(ref obj);
            if (obj != null)
            {
                return obj as T;
            }

            return default(T);
        }

        private const string UniRxNameSpace = "UniRx";

        /// <summary>
        /// Get C# object from a given json as string
        /// </summary>
        /// <param name="json"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T ResolveFromJson<T>(string json)
        {
            var obj = typeof(T).Namespace != UniRxNameSpace
                ? JsonUtility.FromJson<T>(json)
                : RxJson.FromJson<T>(json);
            if (obj != null)
            {
                Pool<T>.AddItem(obj);
                onResolved.Value = obj;
            }

            return obj;
        }

        /// <summary>
        /// Get C# object from a given json as string by a field 'className' inside the json
        /// </summary>
        /// <param name="json"></param>
        /// <param name="lifeCycle"></param>
        /// <param name="resolveFrom"></param>
        /// <returns></returns>
        public static object ResolveFromJson(string json, LifeCycle lifeCycle = LifeCycle.Default,
            object resolveFrom = null)
        {
            //find the className inside the json
            var clsNameHolder = new ClassName();
            JsonUtility.FromJsonOverwrite(json, clsNameHolder);

            var className = clsNameHolder.className;
            var type = DefaultInstance.GetTypeFromCurrentAssembly(className);
            var obj = JsonUtility.FromJson(json, type);
            if (obj != null)
            {
                onResolved.Value = obj;
            }

            return obj;
        }

        /// <summary>
        /// Genericly create a brand new C# / Unity objects by a className inside the current assembly
        /// </summary>
        public static TAbstractType ResolveFromClassName<TAbstractType>(
            string className,
            LifeCycle lifeCycle = LifeCycle.Default)
        {
            var type = GetDefaultInstance(typeof(TAbstractType)).GetTypeFromCurrentAssembly(className);
            var resolveObject = Resolve(type, lifeCycle);

            //add to a shared pool
            var resolveFromClassName = (TAbstractType) resolveObject;
            if (resolveFromClassName != null) Pool<TAbstractType>.AddItem(resolveFromClassName);
            return resolveFromClassName;
        }

        /// <summary>
        /// Generally Create a brand new C# / Unity objects by a className inside the current assembly
        /// </summary>
        public static object ResolveFromClassName(
            string className, LifeCycle lifeCycle = LifeCycle.Default)
        {
            var type = GetDefaultInstance().GetTypeFromCurrentAssembly(className);
            var resolveObject = Resolve(type, lifeCycle);
            return resolveObject;
        }

        /// <summary>
        /// Create a brand new C# only objects from a hashtable object
        /// </summary>
        public static T ResolveFromHashtable<T>(
            Hashtable data,
            object resolveFrom = null) where T : new()
        {
            var obj = new T();

            foreach (var key in data)
            {
                SetPropertyValue(obj, key.ToString(), data[key]);
                SetFieldValue(obj, key.ToString(), data[key]);
            }

            if (obj != null)
            {
                Pool<T>.AddItem(obj);
                onResolved.Value = obj;
            }

            return obj;
        }

        /// <summary>
        /// Create a brand new C# only objects from a hashtable object with no key 'className' inside
        /// </summary>
        public static object ResolveFromHashtable(
            Hashtable data,
            object resolveFrom = null)
        {
            if (data.ContainsKey(ClassName.FIELD))
            {
                var type = DefaultInstance.GetTypeFromCurrentAssembly(data[ClassName.FIELD] as string);
                var obj = Activator.CreateInstance(type);

                foreach (var key in data)
                {
                    SetPropertyValue(obj, key.ToString(), data[key]);
                    SetFieldValue(obj, key.ToString(), data[key]);
                }

                onResolved.Value = obj;
                return obj;
            }

            return null;
        }

        /// <summary>
        /// Get an Unity Object from Resources or AssetBundles by a given path
        /// </summary>
        public static T ResolveFromAssets<T>(string path) where T : class
        {
            var obj = MyResources.Load(path) as T;
            if (obj != null)
            {
                onResolved.Value = obj;
                //add to a shared pool
                Pool<T>.AddItem(obj);
                return obj;
            }


            return obj;
        }

        /// <summary>
        /// Get or Create instances from Object Pools
        /// </summary>
        public static T ResolveFromPool<T>(
            Transform parentObject = null,
            int preload = 0,
            object resolveFrom = null,
            params object[] parameters) where T : Component
        {
            if (!Initialized)
            {
                GetDefaultInstance(typeof(T));
            }

            if (preload > 0 && preload > Pool<T>.List.Count)
            {
                PreloadFromPool<T>(preload, parentObject, resolveFrom, parameters);
            }

            var instanceFromPool = Pool<T>.List.GetInstanceFromPool(parentObject, resolveFrom, parameters);
            instanceFromPool.gameObject.SetActive(true);

            return instanceFromPool;
        }

        /// <summary>
        /// initialize pools by creating instances in advance
        /// </summary>
        public static void PreloadFromPool<T>(
            int preload,
            Transform parentObject = null,
            object resolveFrom = null,
            params object[] parameters)
            where T : Component
        {
            if (!Initialized)
            {
                GetDefaultInstance(typeof(T));
            }

            Pool<T>.List.Preload(preload, parentObject, resolveFrom, parameters);
        }

        /// <summary>
        /// Get all instances of a pools by a given type T
        /// </summary>
        public static ICollection<T> GetPool<T>() where T : Component
        {
            return Pool<T>.List;
        }

//        /// <summary>
//        /// Get all instances of a pools by a given type T
//        /// </summary>
//        public static HashSet<T> GetPool<T>() where T : Component
//        {
//            return Pool<T>.List;
//        }

        /// <summary>
        /// Create a objectContext from the defaultInstance of Context
        /// </summary>
        public static ObjectContext FromObject(object obj, BindingSetting data = null)
        {
            return new ObjectContext(obj, GetDefaultInstance(obj), data);
        }

        /// <summary>
        /// Create a objectContext from the defaultInstance of Context
        /// </summary>
        public static ObjectContext<T> FromObject<T>(BindingSetting data = null)
        {
            return new ObjectContext<T>(GetDefaultInstance(typeof(T)), data);
        }

        private static Context defaultInstance;

//        private static Dictionary<Type, Context> _instances = new Dictionary<Type, Context>();
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

        public static Object Instantiate(Object origin)
        {
            return DefaultInstance.CreateInstance(origin);
        }

        /// <summary>
        /// Clone an object from an existing one
        /// </summary>
        public static T Instantiate<T>(T origin, Object parent) where T : Component
        {
            return DefaultInstance.CreateInstance(origin, parent as Transform);
        }

        /// <summary>
        /// Clone an object from an existing one
        /// </summary>
        public static GameObject Instantiate(GameObject origin, Object parent)
        {
            return DefaultInstance.CreateInstance(origin, parent as Transform);
        }

        /// <summary>
        /// Get default-static general purposes context from an object
        /// </summary>
        public static Context GetDefaultInstance(
            object context,
            bool recreate = false
        )
        {
            return GetDefaultInstance(context.GetType(), Setting.AutoBindDefaultSetting, recreate);
        }

        /// <summary>
        /// Get default-static general purposes context from a type
        /// </summary>
        public static Context GetDefaultInstance(Type type = null, bool automaticBind = true,
            bool recreate = false)
        {
            if (defaultInstance == null || recreate)
            {
                if (Setting.AutoDisposeWhenSceneChanged)
                {
                    SceneManager.sceneLoaded += SceneManagerOnSceneLoaded;
                }

                defaultInstance = new Context(type, false, false, false);
            }

            return defaultInstance;
        }

        /// <summary>
        /// Reset static members to default, should be called if you have changed scene
        /// </summary>
        public static void Reset()
        {
            if (!Initialized)
            {
                return;
            }

            //remove delegate
            if (Setting.AutoDisposeWhenSceneChanged)
            {
                SceneManager.sceneLoaded -= SceneManagerOnSceneLoaded;
            }

            //remove constraints
            ClearConstraints();

            //remove caches
            CacheOfResolvedObjects.Clear();
            DataViewBindings.Clear();
            Pool.Clear();
            ViewPools.Clear();

            //recycle the observable
            if (!onEventRaised.IsDisposed)
            {
                _onEventRaised.Dispose();
                _onEventRaised = null;
            }

            //recycle the observable
            if (!onResolved.IsDisposed)
            {
                onResolved.Dispose();
                _onResolved = null;
            }

            if (!onUpdated.IsDisposed)
            {
                onUpdated.Dispose();
                _onUpdated = null;
            }

            if (!onViewResolved.IsDisposed)
            {
                onViewResolved.Dispose();
                _onViewResolved = null;
            }

            if (!onDisposed.IsDisposed)
            {
                onDisposed.Dispose();
                _onDisposed = null;
            }

            //recycle the defaultInstance
            if (defaultInstance != null)
            {
                defaultInstance.Dispose();
                defaultInstance = null;
            }

            //remove cache of behaviours
            if (_allBehaviours != null)
            {
                Array.Clear(_allBehaviours, 0, _allBehaviours.Length);
                _allBehaviours = null;
            }

            //remove cache of root game objects
            if (_rootgameObjects != null)
            {
                Array.Clear(_rootgameObjects, 0, _rootgameObjects.Length);
                _rootgameObjects = null;
            }
        }

        private static void SceneManagerOnSceneLoaded(Scene arg0, LoadSceneMode loadSceneMode)
        {
            Reset();
        }

        /// <summary>
        /// Create a brand new object as [transient], existing object as [singleton] or getting [component] from inside gameObject
        /// </summary>
        public static object Resolve(
            Type typeToResolve,
            LifeCycle lifeCycle = LifeCycle.Default,
            Type resolveFrom = null,
            params object[] parameters)
        {
            var context = GetDefaultInstance(typeToResolve);
            var resolveObject = context.ResolveObject(typeToResolve, lifeCycle, resolveFrom, parameters);

            if (resolveObject != null)
            {
                //trigger the subject
                onResolved.Value = resolveObject;
            }

            return resolveObject;
        }

        private static void CreateViewFromData(
            object data,
            LifeCycle lifeCycle = LifeCycle.Default,
            Type resolveFrom = null)
        {
            if (data == null)
            {
                return;
            }

            if (lifeCycle != LifeCycle.Singleton && (lifeCycle & LifeCycle.Singleton) != LifeCycle.Singleton)
            {
                //check if the resolved object implements the IDataBinding interface
                var dataBindingTypes = data.GetType().GetInterfaces()
                    .Where(i => i.IsGenericType)
                    .Where(i => i.GetGenericTypeDefinition() == typeof(IViewBinding<>) ||
                                i.GetGenericTypeDefinition() == typeof(IViewBinding<,>) ||
                                i.GetGenericTypeDefinition() == typeof(IViewBinding<,,>) ||
                                i.GetGenericTypeDefinition() == typeof(IViewBinding<,,,>) ||
                                i.GetGenericTypeDefinition() == typeof(IViewBinding<,,,,>)
                    );

                //check if the resolved object implements the IBindByID interface
                var bindByID = data as IBindByID;
                var bindingID = bindByID != null ? bindByID.GetID().ToString() : String.Empty;

                if (dataBindingTypes.Length > 0)
                {
                    foreach (var dataBindingType in dataBindingTypes)
                    {
                        //resolve the type that is in the type argument of IDataBinding<> 
                        var viewTypes = dataBindingType.GetGenericArguments();
                        object viewObject = null;

                        foreach (var viewType in viewTypes)
                        {
                            //resolve by ID, currently not support pools
                            if (bindingID != String.Empty)
                            {
                                foreach (var assetPath in DefaultInstance.assetPaths)
                                {
                                    var path = assetPath.Replace(
                                            "{scene}",
                                            SceneManager.GetActiveScene().name)
                                        .Replace("{type}", viewType.Name)
                                        .Replace("{id}", bindingID);

                                    var prefab = MyResources.Load(path) as GameObject;
                                    if (prefab != null)
                                    {
                                        var gameObject = Instantiate(prefab) as GameObject;
                                        var viewAsComponent = gameObject.GetComponent(viewType);
                                        if (viewAsComponent != null)
                                        {
                                            viewObject = viewAsComponent;
                                        }
                                        else
                                        {
                                            viewObject = gameObject;
                                        }

                                        break;
                                    }
                                }
                            }
                            else if (Setting.CreateViewFromPool)
                            {
                                viewObject = ViewPools.GetObject(viewType,
                                    () => GetDefaultInstance(data.GetType())
                                        .ResolveObject(
                                            viewType,
                                            LifeCycle.Transient,
                                            resolveFrom,
                                            null) as Component);
                            }
                            else
                            {
                                viewObject = DefaultInstance.ResolveObject(viewType, LifeCycle.Transient,
                                    resolveFrom,
                                    null);
                            }

                            //trigger the observer
                            onViewResolved.Value = viewObject;
                            //bind just created View with its data
                            BindDataWithView(data, viewObject);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// trigger "OnNext", then add the data-view to caches
        /// </summary>
        /// <param name="dataObject"></param>
        /// <param name="viewObject"></param>
        /// <returns></returns>
        private static object BindDataWithView(
            object dataObject,
            object viewObject)
        {
            var observerTypes = viewObject.GetType().GetInterfaces()
                .Where(i => i.IsGenericType)
                .Where(i => i.GetGenericTypeDefinition() == typeof(IDataBinding<>));

            if (observerTypes.Length > 0)
            {
                //Bind View for the data object
                foreach (var observerType in observerTypes)
                {
                    var dataObjectType = observerType.GetGenericArguments().FirstOrDefault();

                    var mi = viewObject.GetType().GetMethods()
                        .FirstOrDefault(m =>
                            m.Name == "OnNext" && m.GetParameters().FirstOrDefault().ParameterType ==
                            dataObjectType);

                    if (dataObject.GetType() != dataObjectType)
                    {
                        var cachedObj =
                            GetObjectFromCache(dataObjectType) ?? GetDefaultInstance(dataObjectType)
                                .ResolveObject(dataObjectType, LifeCycle.Transient);

                        dataObject = cachedObj;
                    }

                    mi.Invoke(viewObject, new[] {dataObject});

                    AddToCache(dataObject, viewObject);
                }
            }
            else
            {
                var viewAsGO = viewObject as GameObject;
                if (viewAsGO != null)
                {
                    viewAsGO.SendMessage("OnNext", dataObject, SendMessageOptions.DontRequireReceiver);
                }
                else
                {
                    var viewAsBehaviour = viewObject as MonoBehaviour;
                    if (viewAsBehaviour != null)
                    {
                        viewAsBehaviour.SendMessage("OnNext", dataObject, SendMessageOptions.DontRequireReceiver);
                    }
                }

                AddToCache(dataObject, viewObject);
            }

            return viewObject;
        }

        private static void AddToCache(object dataObject, object viewObject)
        {
            if (!DataViewBindings.ContainsKey(dataObject))
            {
                DataViewBindings[dataObject] = new HashSet<object>();
            }

            DataViewBindings[dataObject].Add(viewObject);
        }

        /// <summary>
        /// Create a brand new C# only objects with parameters for its constructors
        /// </summary>
        /// <param name="parameters"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Resolve<T>(
            params object[] parameters)
        {
            T resolveObject = default(T);
            var obj = resolveObject as object;
            var typeToResolve = typeof(T);
            var valid = ValidateData(typeToResolve, ref obj, When.BeforeResolve);
            if (!valid)
            {
                return default(T);
            }

            var isPoolableType = typeToResolve.GetInterfaces().Contains(typeof(IPoolable));
            if (resolveObject == null)
            {
                //try to get instances from a shared pool
                if (isPoolableType)
                {
                    var list = Pool<T>.GetCollection();
                    foreach (var instance in list)
                    {
                        var t = (IPoolable) instance;
                        if (!t.Alive)
                        {
                            t.Alive = true;
                            t.OnReused();
                            resolveObject = (T) t;
                            break;
                        }
                    }
                }

                if (resolveObject == null)
                {
                    resolveObject = (T) Resolve(typeToResolve, LifeCycle.Transient, null, parameters);
                    if (isPoolableType)
                    {
                        ((IPoolable) resolveObject).Alive = true;
                    }
                }
            }

            obj = resolveObject;
            if (obj != null)
            {
                valid = ValidateData(typeToResolve, ref obj, When.AfterResolve);
                if (!valid)
                {
                    Delete(resolveObject);
                    return default(T);
                }

                //add to a shared pool
                Pool<T>.AddItem(resolveObject);
            }

            return resolveObject;
        }

        /// <summary>
        /// Create a brand new C#/Unity object as [transient], existing object as [singleton] or [component] which has been gotten from inside gameObject
        /// </summary>
        public static T Resolve<T>(
            LifeCycle lifeCycle = LifeCycle.Default,
            Type resolveFrom = null,
            params object[] parameters)
        {
            var obj = (T) Resolve(typeof(T), lifeCycle, resolveFrom, parameters);
            if (obj != null)
            {
                Pool<T>.AddItem(obj);
            }

            return obj;
        }

        /// <summary>
        /// Create a brand new C#/Unity object as [transient], existing object as [singleton] or [component] which has been gotten from inside gameObject
        /// </summary>
        public static T Resolve<T>(
            LifeCycle lifeCycle = LifeCycle.Default,
            params object[] parameters)
        {
            var obj = (T) Resolve(typeof(T), lifeCycle, null, parameters);
            if (obj != null)
            {
                Pool<T>.AddItem(obj);
            }

            return obj;
        }

        /// <summary>
        /// Create an Unity Component as [transient], existing object as [singleton] or [component] which has been gotten from inside gameObject
        /// </summary>
        public static T Resolve<T>(
            Transform parents,
            LifeCycle lifeCycle = LifeCycle.Default,
            Type resolveFrom = null,
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
                Pool<T>.AddItem(obj);
            }

            return obj;
        }

        /// <summary>
        /// Resolve a component with custom logic to find the register object
        /// The default container of [Context] will not be used in this method
        /// </summary>
        /// <param name="parents"></param>
        /// <param name="customLogic"></param>
        /// <param name="parameters"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Resolve<T>(
            Transform parents,
            Func<RegisteredObject, bool> customLogic,
            LifeCycle lifeCycle = LifeCycle.Default,
            object resolveFrom = null,
            params object[] parameters)
            where T : Component
        {
            var context = GetDefaultInstance(typeof(T));
            var registeredObjects = context.DefaultContainer.registeredObjects;
            var registeredObject = registeredObjects
                .Where(r => typeof(T).IsAssignableFrom(r.AbstractType))
                .Where(customLogic)
                .FirstOrDefault();

            if (registeredObject != null)
            {
                var resolve = registeredObject.CreateInstance(context, lifeCycle, resolveFrom, parameters) as T;
                if (resolve != null)
                {
                    onResolved.Value = resolve;
                    Pool<T>.AddItem(resolve);
                }

                return resolve;
            }

            //resolve as default approach
            return Resolve<T>(parents, lifeCycle, resolveFrom, parameters);
        }

        public static void Update<T>(object key, T obj) where T : IBindByID
        {
            var id = obj.GetID();
            if (!obj.Equals(key))
            {
                onEventRaised.Value = new InvalidDataException("Id and Object's Id do not match!");
                return;
            }

            Func<T, bool> filter = o => o.GetID().Equals(key);
            RefAction<T> updater = (ref T o) => o = (T) obj.Clone();

            Update(filter, updater);
        }

        /// <summary>
        /// Update objects from resolvedObjects cache by an action
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="updateAction"></param>
        /// <typeparam name="T"></typeparam>
        public static T Update<T>(T obj, Action<T> updateAction = null)
        {
            var data = obj as object;
            if (data != null)
            {
                var valid = ValidateData(typeof(T), ref data, When.BeforeUpdate);
                if (!valid)
                {
                    return default(T);
                }
            }

            var type = typeof(T);
            if (CacheOfResolvedObjects.ContainsKey(type))
            {
                updateAction?.Invoke(obj);
                if (obj != null)
                {
                    if (obj != null)
                    {
                        onUpdated.Value = obj;
                    }
                }

                if (data != null)
                {
                    var valid = ValidateData(typeof(T), ref data, When.AfterUpdate);
                    if (!valid)
                    {
                        return default(T);
                    }
                }

                UpdateView(ref obj);

                return obj;
            }

            return default(T);
        }

        /// <summary>
        /// Update objects as ref from resolvedObjects cache by an action
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="updateAction"></param>
        /// <typeparam name="T"></typeparam>
        public static T Update<T>(ref T obj, RefAction<T> updateAction = null)
        {
            var data = obj as object;
            if (data != null)
            {
                var valid = ValidateData(typeof(T), ref data, When.BeforeUpdate);
                if (!valid)
                {
                    return default(T);
                }
            }

            var type = typeof(T);
            if (CacheOfResolvedObjects.ContainsKey(type))
            {
                updateAction?.Invoke(ref obj);

                if (obj != null)
                    onUpdated.Value = obj;

                if (data != null)
                {
                    var valid = ValidateData(typeof(T), ref data, When.AfterUpdate);
                    if (!valid)
                    {
                        return default(T);
                    }
                }

                UpdateView(ref obj);

                return obj;
            }

            return default(T);
        }

        /// <summary>
        ///Get all ViewLayers of an object
        /// </summary>
        /// <param name="obj">view object</param>
        /// <typeparam name="T"></typeparam>
        public static HashSet<object> GetView<T>(T obj)
        {
            Type type = typeof(T);
            HashSet<object> viewLayers = null;

            if (DataViewBindings.ContainsKey(obj))
            {
                viewLayers = DataViewBindings[obj];
            }

            return viewLayers ?? new HashSet<object>();
        }

        /// <summary>
        /// Don't update the object but Will update the ViewLayer of the object
        /// </summary>
        /// <param name="obj">view object</param>
        /// <typeparam name="T"></typeparam>
        private static HashSet<object> UpdateView<T>(ref T obj)
        {
            Type type = typeof(T);
            HashSet<object> viewLayers = null;

//update the dataBindings
            if (DataViewBindings.ContainsKey(obj))
            {
                viewLayers = DataViewBindings[obj];

                foreach (var viewLayer in viewLayers)
                {
                    if (obj == null)
                    {
                        //remove view if data is null
                        var viewAsBehaviour = viewLayer as MonoBehaviour;
                        if (viewAsBehaviour != null)
                        {
                            if (Setting.CreateViewFromPool)
                            {
                                viewAsBehaviour.gameObject.SetActive(false);
                            }
                            else
                            {
                                Object.Destroy(viewAsBehaviour.gameObject);
                            }
                        }

                        DataViewBindings.Remove(obj);
                    }
                    else
                    {
                        var mi = viewLayer.GetType().GetMethod("OnNext", new[] {obj.GetType()});
                        mi?.Invoke(viewLayer, new object[] {obj});
                    }
                }
            }

            if (obj == null)
            {
                //remove object if data is null
                onDisposed.Value = obj;

                //remove from the shared pool
                Pool<T>.RemoveItem(obj);
            }

            return viewLayers;
        }

        /// <summary>
        /// Delete an object of a type from resolvedObject cache
        /// </summary>
        public static void Delete<T>(Func<T, bool> filter)
        {
            Update(filter, (ref T obj) => obj = default(T), true);
        }

        /// <summary>
        /// Delete an object by its Id
        /// </summary>
        public static void Delete<T>(object key) where T : IBindByID
        {
            Func<T, bool> filter = o => o.GetID().Equals(key);
            Delete(filter);
        }


        /// <summary>
        /// Delete an object of a type from resolvedObject cache
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="updateAction"></param>
        /// <typeparam name="T"></typeparam>
        public static void Delete<T>(T @object)
        {
            var data = @object as object;
            if (data != null)
            {
                var valid = ValidateData(typeof(T), ref data, When.BeforeDelete);
                if (!valid)
                {
                    return;
                }
            }

            Update(o => ReferenceEquals(o, @object), (ref T obj) => obj = default(T), true);
        }

        /// <summary>
        /// Delete all objects of a type from resolvedObject cache
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="updateAction"></param>
        /// <typeparam name="T"></typeparam>
        public static void DeleteAll<T>()
        {
            Update(o => true, (ref T obj) => obj = default(T), true);
        }

        /// <summary>
        /// Update an object from resolvedObject cache by a ref action
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="updateAction"></param>
        /// <typeparam name="T"></typeparam>
        public static void Update<T>(Func<T, bool> filter, RefAction<T> updateAction, bool isDelete = false)
        {
            var updateType = typeof(T);

            foreach (var type in CacheOfResolvedObjects.Keys.Where(
                t => t == updateType ||
                     t.IsSubclassOf(updateType) ||
                     updateType.IsAssignableFrom(t)))
            {
                if (CacheOfResolvedObjects.ContainsKey(type))
                {
                    var objs = CacheOfResolvedObjects[type].Where(o => filter((T) o)).ToArray();
                    for (var index = 0; index < objs.Length; index++)
                    {
                        //call the delegate
                        var obj = (T) objs[index];

                        var data = obj.Clone();
                        if (!isDelete && data != null)
                        {
                            var valid = ValidateData(typeof(T), ref objs[index], When.BeforeUpdate);
                            if (!valid)
                            {
                                continue;
                            }
                        }

                        //Do update
                        updateAction(ref obj);
                        //trigger the observable
                        if (obj != null) onUpdated.Value = obj;
                        //callback post back
                        if (obj == null)
                        {
                            object o = null;
                            var valid = ValidateData(typeof(T), ref o, When.AfterDelete);
                            if (!valid)
                            {
                                //undo the null assignment
                                objs[index] = data;
                                continue;
                            }
                            Pool<T>.RemoveItem((T) objs[index]);
                        }
                        else
                        {
                            object o = obj;
                            var valid = ValidateData(typeof(T), ref o, When.AfterUpdate);
                            if (!valid)
                            {
                                //undo the null assignment
                                objs[index] = data;
                                continue;
                            }
                        }

                        //update the dataBindings
                        if (DataViewBindings.ContainsKey(objs[index]))
                        {
                            var viewLayers = DataViewBindings[objs[index]];

                            foreach (var viewLayer in viewLayers)
                            {
                                if (obj == null)
                                {
                                    //remove view if data is null
                                    var viewAsBehaviour = viewLayer as MonoBehaviour;
                                    if (viewAsBehaviour != null)
                                    {
                                        if (Setting.CreateViewFromPool)
                                        {
                                            viewAsBehaviour.gameObject.SetActive(false);
                                        }
                                        else
                                        {
                                            Object.Destroy(viewAsBehaviour.gameObject);
                                        }
                                    }

                                    DataViewBindings.Remove(objs[index]);
                                }
                                else
                                {
                                    var mi = viewLayer.GetType().GetMethod("OnNext");
                                    mi.Invoke(viewLayer, new object[] {obj});
                                }
                            }
                        }

                        if (obj == null)
                        {
                            //remove the old object if data is null
                            onDisposed.Value = objs[index];
                            //remove from a shared pool
                            Pool<T>.RemoveItem((T) objs[index]);
//                        //remove obj from the interal cache of resolved objects
//                        ResolvedObjects[type].Remove(objs[index]);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Update an object from resolvedObject cache by an action
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="updateAction"></param>
        /// <typeparam name="T"></typeparam>
        public static void Update<T>(Func<T, bool> filter, Action<T> updateAction) where T : class
        {
            var type = typeof(T);
            if (CacheOfResolvedObjects.ContainsKey(type))
            {
                var objs = CacheOfResolvedObjects[type].Where(o => filter(o as T)).ToArray();
                for (var index = 0; index < objs.Length; index++)
                {
                    //call the delegate
                    var obj = objs[index] as T;

                    var data = obj as object;
                    if (data != null)
                    {
                        var valid = ValidateData(typeof(T), ref data, When.BeforeUpdate);
                        if (!valid)
                        {
                            continue;
                        }
                    }

                    updateAction(obj);
                    //trigger the observable
                    if (obj != null) onUpdated.Value = obj;
                    //update the dataBindings
                    if (DataViewBindings.ContainsKey(objs[index]))
                    {
                        var viewLayers = DataViewBindings[objs[index]];
                        foreach (var viewLayer in viewLayers)
                        {
                            var mi = viewLayer.GetType().GetMethod("OnNext", new[] {typeof(T)});
                            mi.Invoke(viewLayer, new object[] {obj});
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Get all resolved objects of a type from resolvedObjects cache
        /// </summary>
        public static ICollection<T> GetObjects<T>() where T : class
        {
//            Debug.Log(typeof(T));
            return Pool<T>.List;
        }

        /// <summary>
        /// Get all resolved objects of a type from resolvedObjects cache
        /// </summary>
        public static T[] GetObjectsFromCache<T>() where T : class
        {
//            Debug.Log(typeof(T));
            return CacheOfResolvedObjects[typeof(T)].Cast<T>().ToArray();
        }

        /// <summary>
        /// Get all resolved objects of a type by a filter from resolvedObjects cache
        /// </summary>
        /// <param name="filter"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T[] GetObjectsFromCache<T>(Func<T, bool> filter) where T : class
        {
            return CacheOfResolvedObjects[typeof(T)].Where(p => filter(p as T)).Cast<T>().ToArray();
        }

        /// <summary>
        /// Get the first matching object of a type from resolvedObjects cache
        /// </summary>
        /// <param name="filter"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetObject<T>(Func<T, bool> filter) where T : class
        {
            return CacheOfResolvedObjects[typeof(T)].FirstOrDefault(p => filter(p as T)) as T;
        }

        public delegate void RefAction<T>(ref T obj);

        /// <summary>
        /// Dispose an obj, which has been created by the Context before.
        /// </summary>
        /// <param name="obj"></param>
        public static void Dispose(ref object obj)
        {
            if (obj != null)
            {
                Type type = obj.GetType();
                if (!CacheOfResolvedObjects.ContainsKey(type))
                {
                    CacheOfResolvedObjects[type] = new HashSet<object>();
                }

                DataViewBindings.Remove(obj);
                onDisposed.Value = obj;

                var disposable = obj as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }

                obj = null;
            }
        }

        /// <summary>
        /// Set value for a property by its name
        /// </summary>
        private static void SetPropertyValue(object inputObject, string propertyName, object value)
        {
            Type type = inputObject.GetType();
            PropertyInfo propertyInfo = type.GetProperty(propertyName);

            if (propertyInfo == null)
            {
                return;
            }

            var targetType = IsNullableType(propertyInfo.PropertyType)
                ? Nullable.GetUnderlyingType(propertyInfo.PropertyType)
                : propertyInfo.PropertyType;

            value = Convert.ChangeType(value, targetType);
            propertyInfo.SetValue(inputObject, value, null);
        }

        /// <summary>
        /// Set value for a field by its name
        /// </summary>
        private static void SetFieldValue(object inputObject, string fieldName, object value)
        {
            Type type = inputObject.GetType();
            var fieldInfo = type.GetField(fieldName);

            if (fieldInfo == null) return;

            var targetType = IsNullableType(fieldInfo.FieldType)
                ? Nullable.GetUnderlyingType(fieldInfo.FieldType)
                : fieldInfo.FieldType;

            value = Convert.ChangeType(value, targetType);
            fieldInfo.SetValue(inputObject, value);
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
        /// Unbind a TAbstract with a TConcrete
        /// </summary>
        /// <param name="lifeCycle"></param>
        /// <typeparam name="TAbstract"></typeparam>
        /// <typeparam name="TConcrete"></typeparam>
        public static void Unbind<TAbstract>()
        {
            GetDefaultInstance(typeof(TAbstract)).container.Unbind(typeof(TAbstract));
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
        /// Get a component from a mono behaviour by a given path on hierarchy
        /// </summary>
        /// <param name="type"></param>
        /// <param name="component"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Component ResolveFromHierarchy(
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
        /// Get a component from a gameObject by a given path
        /// </summary>
        /// <param name="type"></param>
        /// <param name="gameObject"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Component ResolveFromHierarchy(
            Type type,
            GameObject gameObject,
            string path
        )
        {
            var comp = gameObject.GetComponent(type) as MonoBehaviour;
            if (comp)
            {
                //resolve with path
                var component = ResolveFromHierarchy(type, comp, path);
                if (component)
                    return component;
            }

            //resolve without path
            return Resolve(type) as Component;
        }

        /// <summary>
        /// Get a component from a gameObject by a given path
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="path"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T ResolveFromHierarchy<T>(
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
                var component = ResolveFromHierarchy(type, comp, path) as T;
                if (component != null)
                    return component;
            }

            //resolve without path
            return Resolve(type) as T;
        }

        /// <summary>
        /// Get a component from a mono behaviour by a given path
        /// </summary>
        /// <param name="component"></param>
        /// <param name="path"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T ResolveFromHierarchy<T>(
            MonoBehaviour component,
            string path
        )
            where T : class
        {
            return ResolveFromHierarchy(typeof(T), component, path) as T;
        }

        /// <summary>
        /// Get an C# object from a mono behaviour by a given type
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
        /// Get an C# object from a mono behaviour by a given type
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

        private static Observable<object> _onEventRaised;

        public static Observable<object> onEventRaised
        {
            get
            {
                if (_onEventRaised == null)
                {
                    _onEventRaised = new Observable<object>();
                }

                return _onEventRaised;
            }
        }

        public static Observable<T> OnEventRaised<T>()
        {
            var output = new Observable<T>();
            onEventRaised.Subscribe(ex =>
            {
                if (ex is T obj)
                {
                    output.Value = obj;
                }
            });
            return output;
        }

        #endregion

        #region Settings

        public class Setting
        {
            /// <summary>
            /// This is the default name of the default assembly that unity generated to compile your code
            /// </summary>
            public const string DefaultAssemblyName = "Assembly-CSharp";

            /// <summary>
            /// This is the  name of the default assembly that Context will be looking for
            /// </summary>
            public static string AssemblyName = DefaultAssemblyName;

            /// <summary>
            /// Get views from pools rather than a new object. Default is true.
            /// </summary>
            public static bool CreateViewFromPool = true;

            /// <summary>
            /// if true, when a new scene is unloaded, call the Dispose method. Default is false.
            /// </summary>
            public static bool AutoDisposeWhenSceneChanged;

            /// <summary>
            /// if true, when the default instance get initialized, it will process all mono-behaviours in current active scenes. Default is true.
            /// </summary>
            public static bool AutoBindDefaultSetting = false;

            /// <summary>
            /// Pool's collection will be constructed by Set, instead of List. Default is false.
            /// </summary>
            public static bool UseSetForCollection = false;

            /// <summary>
            /// Name of the default bundle that MyResource will load from if no bundleName is set
            /// </summary>
            public static string DefaultBundleName = "resources";

            /// <summary>
            /// While running in editor, always load from resources before searching in asset bundles
            /// </summary>
            public static bool EditorLoadFromResource = true;

            public static bool EnableLogging = true;
        }

        #endregion


        public static IDictionary<Type, ICollection<ValidState>> ValidatorCollection
        {
            get
            {
                if (validatorlist == null)
                {
                    validatorlist = new Dictionary<Type, ICollection<ValidState>>();
                }

                return validatorlist;
            }
        }

        public static void AddConstraint(
            Type dataType,
            ValidState.Predicate validator,
            string msg,
            When action = When.All
        )
        {
            if (dataType != null)
            {
                ValidState vs = new ValidState();
                vs.predicate = validator;
                vs.message = msg;
                vs.when = action;

                AddToValidatorCollection(dataType, vs);
            }
        }

        private static void AddToValidatorCollection(Type dataType, ValidState vs)
        {
            if (!ValidatorCollection.ContainsKey(dataType))
            {
                ValidatorCollection[dataType] = new HashSet<ValidState>();
            }

            ValidatorCollection[dataType].Add(vs);
        }

        public static bool RemoveConstraint<T>(string msg = null, When when = When.All)
        {
            return RemoveConstraint(typeof(T), msg, when);
        }

        public static bool RemoveConstraint<T>(When when = When.All)
        {
            return RemoveConstraint(typeof(T), string.Empty, when);
        }


        public delegate bool RefPredicate<T>(ref T obj);

        public static void AddConstraint<T>(
            RefPredicate<T> validator,
            string msg,
            When action = When.All
        )
        {
            var dataType = typeof(T);
            ValidState vs = new ValidState();
            vs.predicate = (ref object obj) =>
            {
                var t = (T) obj;
                return validator(ref t);
            };
            vs.message = msg;
            vs.when = action;

            AddToValidatorCollection(dataType, vs);
        }

        public static void AddConstraint<T>(
            Func<T, bool> validator,
            string msg,
            When action = When.All
        )
        {
            var dataType = typeof(T);
            ValidState vs = new ValidState();
            vs.predicate = (ref object obj) =>
            {
                var t = (T) obj;
                return validator(t);
            };
            vs.message = msg;
            vs.when = action;

            AddToValidatorCollection(dataType, vs);
        }

        public static void ClearConstraints()
        {
            validatorlist?.Clear();
        }

        public static ICollection<ValidState> GetValidators(Type type)
        {
            if (!ValidatorCollection.ContainsKey(type))
            {
                ValidatorCollection[type] = new HashSet<ValidState>();
            }

            return ValidatorCollection[type];
        }

        public static bool RemoveConstraint(Type dataType, When when = When.All)
        {
            return RemoveConstraint(dataType, string.Empty, when);
        }

        public static bool RemoveConstraint(Type dataType, string msg = null, When when = When.All)
        {
            if (ValidatorCollection.ContainsKey(dataType))
            {
                var validStates = ValidatorCollection[dataType];
                foreach (var validState in validStates.ToList())
                {
                    if (when.IsEqual(validState.when))
                    {
                        var legalToRemove = string.IsNullOrEmpty(msg) || validState.message.Equals(msg);
                        if (legalToRemove)
                        {
                            return validStates.Remove(validState);
                        }
                    }
                }

                return false;
            }

            return false;
        }
    }
}


//        /// <summary>
//        /// Get or Create instances from Object Pools
//        /// </summary>
//        public static T ResolveFromPool<T>(
//            int preload = 0,
//            object resolveFrom = null,
//            params object[] parameters) where T : IPoolable
//        {
//            if (!Initialized)
//            {
//                GetDefaultInstance(typeof(T));
//            }
//
//            if (preload > 0 && preload > Pool<T>.List.Count)
//            {
//                PreloadFromPool<T>(preload, resolveFrom, parameters);
//            }
//
//            var instanceFromPool = Pool<T>.List.GetObjectFromPool(resolveFrom, parameters);
//
//            if (instanceFromPool != null)
//            {
//                onResolved.Value = instanceFromPool;
//            }
//
//            return instanceFromPool;
//        }