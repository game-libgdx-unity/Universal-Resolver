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
            var assemblyName = CurrentAssembly.GetName().Name;
            var injectIntoBindingSetting =
                Resources.Load<InjectIntoBindingSetting>(assemblyName);

            debug.Log("Looking for assembly {0} 's BindingSetting ", assemblyName);

            if (injectIntoBindingSetting)
            {
                debug.Log("Found BindingSetting for assembly {0}", assemblyName);
                LoadBindingSetting(injectIntoBindingSetting);
            }

            //try to get the default InjectIntoBindingSetting setting for current scene
            var sceneInjectIntoBindingSetting = Resources.Load<InjectIntoBindingSetting>(
                String.Format("{0}_{1}", assemblyName, SceneManager.GetActiveScene().name)
            );

            if (sceneInjectIntoBindingSetting)
            {
                debug.Log("Found InjectIntoBindingSetting for scene");
                LoadBindingSetting(sceneInjectIntoBindingSetting);
            }

            //try to load a default BindingSetting setting for context
            var bindingSetting =
                Resources.Load<BindingSetting>(assemblyName);

            if (bindingSetting)
            {
                debug.Log("Found binding setting for assembly");
                LoadBindingSetting(bindingSetting);
            }

            //try to get the default BindingSetting setting for current scene
            var sceneBindingSetting = Resources.Load<BindingSetting>(
                String.Format("{0}_{1}", assemblyName, SceneManager.GetActiveScene().name)
            );

            if (sceneBindingSetting)
            {
                debug.Log("Found binding setting for scene");
                LoadBindingSetting(sceneBindingSetting);
            }

            
            ProcessBindingAttribute();
                
            ProcessInjectAttributeForMonoBehaviours();
            
        }

        private List<BindingAttribute> bindingAttributes = new List<BindingAttribute>(); 
        /// <summary>
        /// Process the current assembly to create RegisterObjects from BindingAttributes
        /// </summary>
        private void ProcessBindingAttribute()
        {
            debug.Log("Finding binding attributes from assembly {0}", CurrentAssembly);

            var myAssembly = CurrentAssembly;
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

        private void Initialize(Type target = null, bool autoFindSetting = false,
            bool disableProcessAllBehaviours = false, string[] assetPaths = null)
        {
            if (!Initialized)
            {
                defaultInstance = this;
            }

            DisableProcessAllBehaviour = disableProcessAllBehaviours;
            this.autoFindSetting = autoFindSetting;
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
    }
}