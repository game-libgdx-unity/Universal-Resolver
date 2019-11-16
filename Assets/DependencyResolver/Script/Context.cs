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
        #region Static members

        /// <summary>
        /// If the Context static API is ready to use
        /// </summary>
        public static bool Initialized => defaultInstance != null && defaultInstance.initialized;

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
        public static Dictionary<Type, List<object>>
            CacheOfResolvedObjects = new Dictionary<Type, List<object>>();

        /// <summary>
        /// Cache of data binding of data layer & view layer
        /// </summary>
        public static Dictionary<object, List<object>> DataViewBindings = new Dictionary<object, List<object>>();

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
                                    CacheOfResolvedObjects[type] = new List<object>();
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
            return GetDefaultInstance(context.GetType(), Setting.AutoFindBindingSetting, recreate);
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

                defaultInstance = new Context(type, Setting.AutoFindBindingSetting, false, false);
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
            
            //remove cache of object names
            _objectNames.Clear();
            
            //remove cache of object tags
            ObjectTags.Clear();
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
            SetPropertyValue(dataObject, "View", viewObject);
            
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
                DataViewBindings[dataObject] = new List<object>();
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
        public static List<object> GetView<T>(T obj)
        {
            Type type = typeof(T);
            List<object> viewLayers = null;

            if (DataViewBindings.ContainsKey(obj))
            {
                viewLayers = DataViewBindings[obj];
            }

            return viewLayers ?? new List<object>();
        }

        /// <summary>
        /// Don't update the object but Will update the ViewLayer of the object
        /// </summary>
        /// <param name="obj">view object</param>
        /// <typeparam name="T"></typeparam>
        private static List<object> UpdateView<T>(ref T obj)
        {
            Type type = typeof(T);
            List<object> viewLayers = null;

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
        /// <typeparam name="TData"></typeparam>
        public static void Delete<TData>(TData @object)
        {
            if(typeof(TData).IsImplementedGenericInterface(typeof(IViewBinding<>)))
            {
                var view = Context.GetPropertyValue(@object, "View") as MonoBehaviour;
                if (Setting.CreateViewFromPool)
                {
                    view.gameObject.SetActive(false);
                }
                else
                {
                    Object.Destroy(view);
                }
            }

            Update(o => ReferenceEquals(o, @object), (ref TData obj) => obj = default(TData), true);
        }

        /// <summary>
        /// Delete all objects of a type from resolvedObject cache
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="updateAction"></param>
        /// <typeparam name="T"></typeparam>
        public static void DeleteAll<T>()
        {
            var objPool = Pool<T>.List;
            foreach (var obj in objPool.ToArray())
            {
                Delete(obj);
            }
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

            foreach (var type  in CacheOfResolvedObjects.Keys.Where(
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
        /// Get value from a property by its name
        /// </summary>
        private static object GetPropertyValue(object inputObject, string propertyName)
        {
            Type type = inputObject.GetType();
            PropertyInfo propertyInfo = type.GetProperty(propertyName);
            return propertyInfo == null ? null : propertyInfo.GetValue(inputObject);
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
        public static void Bind<TAbstract, TConcrete>(LifeCycle lifeCycle = LifeCycle.Default) where TConcrete : TAbstract
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
            
//            /// <summary>
//            /// If Context should get results from its cache
//            /// </summary>
//            public static bool EnableCacheResult = false;

            /// <summary>
            /// if true, when a new scene is unloaded, call the Dispose method. Default is false.
            /// </summary>
            public static bool AutoDisposeWhenSceneChanged;
            /// <summary>
            /// if true, when a new scene is loaded, context auto find the default binding setting files
            /// </summary>
            public static bool AutoFindBindingSetting = false;
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
    }
}