/**
 * Author:    Vinh Vu Thanh
 * This class is a part of Unity IoC project that can be downloaded free at 
 * https://github.com/game-libgdx-unity/UnityEngine.IoC
 * (c) Copyright by MrThanhVinh168@gmail.com
 **/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using UnityIoC;
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

namespace UnityIoC
{
    public partial class Context
    {
        #region Variables & Constants

        private IList<BindingAttribute> bindingAttributes = new List<BindingAttribute>();
        private IList<InjectAttribute> injectAttributes = new List<InjectAttribute>();

        public bool loadDefaultSetting;
        public bool requirePreRegistered = true;

        public ImplementClass implementClasses;

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
            Initialize(null);
        }

        private void InitialProcess()
        {
            if (container == null)
            {
                Debug.LogError("You need to call Initialize before call this method");
                return;
            }

            LoadClassesFromConfigFile(implementClasses);

            if (loadDefaultSetting)
            {
                //try to load a default setting for context
                var objClassConfig = Resources.Load<ImplementClass>(SceneManager.GetActiveScene().name);
                LoadClassesFromConfigFile(objClassConfig);
            }

            ProcessComponentAttribute();

            ProcessBindingAttribute();

            ProcessAutomaticBinding();

            ProcessInjectAttributeForMonoBehaviour();
        }

        private void ProcessComponentAttribute()
        {
        }

        private void ProcessAutomaticBinding()
        {
            var myAssembly = TargetType == null ? Assembly.GetExecutingAssembly() : TargetType.Assembly;
            var concreteTypes = myAssembly.GetTypes().Where(t => !(t.IsInterface || t.IsAbstract)).ToArray();
            var abstractions = myAssembly.GetTypes().Where(t => t.IsInterface || t.IsAbstract).ToArray();

            //find interface with only 1 implement
            foreach (var abstraction in abstractions)
            {
                //process for IContainer internally
                if (abstraction == typeof(IContainer))
                {
                    Bind(abstraction, this.container);
                    continue;
                }

                //binding attribute gives Type a higher priority than automatic binding with no attributes 
                //Only automatically bind for classes have no binding attributes  
                var concreteType = concreteTypes.FirstOrDefault(t =>
                    t.GetCustomAttributes(typeof(BindingAttribute), true).Length == 0 &&
                    (t.GetInterface(abstraction.Name) != null) || t.IsSubclassOf(abstraction));

                if (concreteType == null) continue;

                //still bind for existing registered typeToResolve
                //however this is low priority than binding using attributes or Bind() methods.
                if (Container.IsRegistered(abstraction))
                {
                    Debug.LogFormat("type of {0} which is already registered", abstraction);
                }

                Debug.LogFormat("Automatically Bind {0} for {1}", concreteType, abstraction);
                Bind(abstraction, concreteType);
            }
        }

        private void LoadClassesFromConfigFile(ImplementClass implementClasses)
        {
            if (implementClasses)
            {
                foreach (var objClass in implementClasses.classObjectsToLoad)
                {
                    var className = objClass.name;
                    var classType = Type.GetType(className);

                    if (classType.IsSubclassOf(typeof(MonoBehaviour)))
                    {
                        //
                    }
                    else
                    {
                        Debug.Log("create of type " + classType);
                        var obj = Activator.CreateInstance(Type.GetType(className), false);
                        Bind(classType, obj);
                    }
                }
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
                ProcessInjectAttribute(mono);
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

                //try to resolve as monoBehaviour first
                var component = GetComponentFromGameObject(mono, property.PropertyType, inject);
                if (component)
                {
                    property.SetValue(mono, component, null);
                    continue;
                }

                //default object resolve method 
                ProcessMethodInfo(mono, method, inject);
            }
        }

        private void ProcessVariables(object mono)
        {
            Type objectType = mono.GetType();
            var fieldInfos = objectType
                .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(fieldInfo => fieldInfo.IsDefined(typeof(InjectAttribute), false))
                .ToArray();

            if (fieldInfos.Length > 0)
                Debug.Log(string.Format("Found {0} fieldInfo to process", fieldInfos.Length));

            foreach (var field in fieldInfos)
            {
                var inject =
                    field.GetCustomAttributes(typeof(InjectAttribute), true).FirstOrDefault() as InjectAttribute;

                injectAttributes.Add(inject);

                //try to resolve as monoBehaviour first
                var component = GetComponentFromGameObject(mono, field.FieldType, inject);
                if (component)
                {
                    field.SetValue(mono, component);
                    continue;
                }

                //default object resolve method 
                field.SetValue(mono,
                    container.ResolveObject(field.FieldType, mono,
                        inject == null ? LifeCycle.Default : inject.LifeCycle));
            }
        }

        /// <summary>
        /// Try to resolve unity component, this should be used in other attribute process methods
        /// </summary>
        /// <param name="mono">object is expected as unity mono behaviour</param>
        /// <returns>true if you want to stop other attribute process methods</returns>
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

            //resolve by inject component to the gameobject
            var injectComponent = injectAttribute as IInjectComponent;
            if (injectComponent != null)
            {
                var component = injectComponent.GetComponent(behaviour, type);

                //try to search from registeredObject of this type and resolve
                if (container.IsRegistered(type))
                {
                    //valid when concreteType is subclass of monobehaviour
                    var obj = Container.GetRegisteredObject(type);
                    if (obj.ConcreteType.IsSubclassOf(typeof(MonoBehaviour)))
                    {
                        Debug.LogFormat("Adding {0} is to gameObject", obj.ConcreteType);
                        return behaviour.gameObject.AddComponent(obj.ConcreteType);
                    }
                }
                
                //unable to get it from gameObject
                if (component == null)
                {
                    throw new InvalidOperationException("You cannot apply Component attribute to resolve non-unity component");
                }

                return component;
            }

            return null;
        }

        #endregion

        #region Public members

        public IContainer Container
        {
            get { return container; }
        }

        public void Initialize(Type target)
        {
            TargetType = target;
            container = new DefaultContainer(this, TargetType);

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