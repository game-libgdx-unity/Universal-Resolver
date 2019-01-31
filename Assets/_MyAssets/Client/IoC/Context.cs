using System;
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

        public bool loadDefaultSetting;
        public bool requirePreRegistered = true;

        public ImplementClass implementClasses;

        public BindingAttribute Binding { get; set; }
        public InjectAttribute Inject { get; set; }

        public Type TargetType { get; set; }

        #endregion

        #region Private members

        public Context(Type target)
        {
            Initialize(target);
        }

        public Context()
        {
            Initialize(typeof(Context));
        }

        private IContainer container;

        public void Initialize(Type target)
        {
            TargetType = target;
            container = new DefaultContainer(this, TargetType);

            InitialProcess();
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

            ProcessBindingAttribute();

            ProcessInjectAttributeForMonoBehaviour();
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

        public void Unload()
        {
            if (container != null)
            {
                container.Unload();
            }
        }

        private void ProcessBindingAttribute()
        {
            var myAssembly = TargetType.Assembly;
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

                        this.Binding = bindingAttribute;

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

            //disable binding cache
            this.Binding = null;
        }

        private void ProcessInjectAttributeForMonoBehaviour()
        {
            MonoBehaviour[] behaviours = Object.FindObjectsOfType<MonoBehaviour>();

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
        /// Try to resolve unity component, this should be used in other process methods
        /// </summary>
        /// <param name="mono">object is expected as unity mono behaviour</param>
        /// <returns>true if you want to stop other attribute process methods</returns>
        private Component GetComponentFromGameObject(object mono, Type type, InjectAttribute injectAttribute)
        {
            if (type.IsSubclassOf(typeof(MonoBehaviour)))
            {
                var behaviour = mono as MonoBehaviour;

                if (behaviour == null) return null;

                //not supported for transient or singleton inject
                if (injectAttribute.LifeCycle == LifeCycle.Transient || 
                    injectAttribute.LifeCycle == LifeCycle.Singleton)
                {
                    return null;
                }
                
                //by default, try to resolve by getting the component from the game object
                var componentFromGameObject = behaviour.GetComponent(type);
                if (componentFromGameObject != null)
                {
                    return componentFromGameObject;
                }

                //resolve by inject component to the gameobject
                if (injectAttribute.GetType() == typeof(ComponentAttribute))
                {
                    return behaviour.gameObject.AddComponent(type);
                }

                //resolve by finding component from gameObject by name
                if (injectAttribute.GetType() == typeof(FindGameObjectByNameAttribute))
                {
                    var attribute = injectAttribute as FindGameObjectByNameAttribute;
                    var findingComponent = GameObject.Find(attribute.Name).GetComponent(type);
                    if (findingComponent != null)
                    {
                        return findingComponent;
                    }
                }

                //resolve by finding component from gameObjects by tag
                if (injectAttribute.GetType() == typeof(FindGameObjectsByTagAttribute))
                {
                    var attribute = injectAttribute as FindGameObjectsByTagAttribute;
                    var findingObj = GameObject.FindGameObjectsWithTag(attribute.Name)
                        .FirstOrDefault(go => go.GetComponent(type) != null);

                    if (findingObj != null)
                    {
                        var findingComponent = findingObj.GetComponent(type);
                        if (findingComponent != null)
                        {
                            return findingComponent;
                        }
                    }
                }

                //resolve by finding type of mono behaviour
                if (injectAttribute.GetType() == typeof(FindObjectOfTypeAttribute))
                {
                    var findingObj = Object.FindObjectOfType(type) as MonoBehaviour;
                    if (findingObj != null)
                    {
                        return findingObj;
                    }
                }
            }

            return null;
        }

        #endregion

        #region Public members

        public IContainer GetInstance()
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