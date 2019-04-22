/**
 * Author:    Vinh Vu Thanh
 * This class is a part of Universal Resolver project that can be downloaded free at 
 * https://github.com/game-libgdx-unity/UnityEngine.IoC
 * (c) Copyright by MrThanhVinh168@gmail.com
 **/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using NUnit.Framework;
using UnityEngine;
using UnityIoC;
using PropertyAttribute = UnityEngine.PropertyAttribute;
#if UNITY_EDITOR
using UnityEditor;

#endif

public static class RxEtensionMethod
{
    public static IDisposable AddTo(this IDisposable source, Component component)
    {
        return AddTo(source, component.gameObject);
    }

    public static IDisposable AddTo(this IDisposable source, GameObject gameObject)
    {
        var addTo = gameObject.GetOrAddComponent<AddTo>();
        addTo.disposables.Add(source);
        return source;
    }

//    public static IDisposable AddTo<T>(this Observer<T> source, Component component)
//    {
//        return AddTo(source, component.gameObject);
//    }
//
//    public static IDisposable AddTo<T>(this Observer<T> source, GameObject gameObject)
//    {
//        var addTo = gameObject.GetComponent<AddTo>() ?? gameObject.AddComponent<AddTo>();
//        addTo.observers.
//        addTo.disposables.Add(source);
//        return source;
//    }
}

public interface IReactiveProperty<T> : IObservable<T>
{
    T Value { get; set; }
    bool HasValue { get; }
}

public class Disposable : IDisposable
{
    public static readonly IDisposable Empty = new Disposable();

    private Disposable()
    {
    }

    public void Dispose()
    {
    }
}

public class ConsoleObserver<T> : IObserver<T>
{
    private readonly string subject;

    public ConsoleObserver(string subject = "")
    {
        this.subject = subject;
    }

    public void OnNext(T value)
    {
        Debug.LogFormat("{0}: {1}", subject, value);
    }

    public void OnError(Exception error)
    {
        Debug.LogErrorFormat("{0} - OnError: {1}", subject, error.Message);
        Debug.LogErrorFormat("\t {0}", error.StackTrace);
    }

    public void OnCompleted()
    {
        Debug.LogFormat("{0} - OnCompleted()", subject);
    }
}

public class ObserverGameObject<T> : Observer<T>
{
    public GameObject GameObject;

    public ObserverGameObject(GameObject gameObject, Action<T> onNext, Action<Exception> onError = null,
        Action onCompleted = null) : base(onNext, onError, onCompleted)
    {
        this.GameObject = gameObject;
    }

    public override bool IsCompleted
    {
        get
        {
            if (GameObject == null)
            {
                return false;
            }

            return GameObject.activeInHierarchy && base.IsCompleted;
        }
        set { base.IsCompleted = value; }
    }
}

public class Observer<T> : IObserver<T>
{
    private Action<T> onNext;
    private Action<Exception> onError;
    private Action onCompleted;

    public virtual bool IsCompleted { get; set; }
    protected bool CeaseIfError { get; set; }

    public Observer(Action<T> onNext, Action<Exception> onError = null, Action onCompleted = null)
    {
        this.onNext = onNext;
        this.onError = onError;
        this.onCompleted = onCompleted;
    }

    public void OnCompleted()
    {
        if (IsCompleted)
        {
            return;
        }

        if (onCompleted != null)
        {
            IsCompleted = true;
            onCompleted();
        }
    }

    public void OnError(Exception error)
    {
        if (IsCompleted)
        {
            return;
        }

        if (CeaseIfError)
        {
            IsCompleted = true;
        }

        if (onError != null)
        {
            onError(error);
        }
    }

    public void OnNext(T value)
    {
        if (IsCompleted)
        {
            return;
        }

        if (onNext != null)
        {
            onNext(value);
        }
    }
}

public class ObservableDisposable<T> : IDisposable
{
    private Observable<T> reactiveProperty;
    private IObserver<T> observer;

    public ObservableDisposable(Observable<T> reactiveProperty, IObserver<T> observer)
    {
        this.observer = observer;
        this.reactiveProperty = reactiveProperty;

        this.reactiveProperty.observers.Add(observer);
    }

    public void Dispose()
    {
        reactiveProperty.observers.Remove(observer);
    }
}

//public class Observable<K, V> : Observable<K>, IDisposable
//{
//    public Observable<V> Property { get; set; }
//
//    public Observable() : this(default(K), default(V))
//    {
//    }
//
//    public Observable(K key, V initialValue) : base(key)
//    {
//        Property = new Observable<V>(initialValue);
//    }
//
//    public void Set(K instanceFromPool, V transient)
//    {
//        Value = instanceFromPool;
//        Property.Value = transient;
//    }
//}



public class Observable<T> : IReactiveProperty<T>, IDisposable
{
    static readonly IEqualityComparer<T> defaultEqualityComparer = global::EqualityComparer.GetDefault<T>();
    public HashSet<IObserver<T>> observers = new HashSet<IObserver<T>>();

    private bool isDisposed;
    private T _value;

    protected virtual IEqualityComparer<T> EqualityComparer
    {
        get { return defaultEqualityComparer; }
    }

    public Observable()
    {
        if (typeof(T).IsClass || typeof(T).IsInterface)
        {
            HasValue = false;
        }
        else
        {
            HasValue = true;
        }
    }

    public Observable(T initialValue)
    {
        Value = initialValue;
        HasValue = true;
    }

    public IDisposable Subscribe(IObserver<T> observer)
    {
        if (IsDisposed)
        {
            observer.OnCompleted();
            return Disposable.Empty;
        }

        observers.Add(observer);

        if (HasValue)
        {
            observer.OnNext(_value);
        }

        return new ObservableDisposable<T>(this, observer);
    }

    public IDisposable Subscribe(Action<T> onNext, Action<Exception> onError = null, Action onCompleted = null)
    {
        return Subscribe(new Observer<T>(onNext, onError, onCompleted));
    }

    public IDisposable SubscribeToConsole(string subject)
    {
        return Subscribe(new ConsoleObserver<T>(subject));
    }

    public IDisposable Subscribe(GameObject gameObject, Action<T> onNext, Action<Exception> onError = null,
        Action onCompleted = null)
    {
        var disposable = Subscribe(new ObserverGameObject<T>(gameObject, onNext, onError, onCompleted));
        return disposable.AddTo(gameObject);
    }

    public IDisposable Subscribe(Component component, Action<T> onNext, Action<Exception> onError = null,
        Action onCompleted = null)
    {
        return Subscribe(component.gameObject, onNext, onError, onCompleted);
    }

    public T Value
    {
        get { return _value; }
        set
        {
            if (IsDisposed)
            {
                return;
            }

            if (!EqualityComparer.Equals(_value, value))
            {
                HasValue = true;
                this._value = value;

                foreach (var observer in observers)
                {
                    try
                    {
                        observer.OnNext(_value);
                    }
                    catch (Exception ex)
                    {
                        observer.OnError(ex);

#if UNITY_EDITOR || DEVELOPMENT_BUILD

                        Debug.LogError(ex.Message);
                        Debug.LogError(ex.StackTrace);

#endif
                    }
                }
            }
        }
    }

    public bool HasValue { get; private set; }

    public bool IsDisposed
    {
        get { return isDisposed; }
        set { isDisposed = value; }
    }

    public void Dispose()
    {
        HasValue = false;
        IsDisposed = true;
        foreach (var observer in observers)
        {
            observer.OnCompleted();
        }

        observers.Clear();
    }
}

public static class EqualityComparer
{
    public static readonly IEqualityComparer<Vector2> Vector2 = new Vector2EqualityComparer();
    public static readonly IEqualityComparer<Vector3> Vector3 = new Vector3EqualityComparer();
    public static readonly IEqualityComparer<Vector4> Vector4 = new Vector4EqualityComparer();
    public static readonly IEqualityComparer<Color> Color = new ColorEqualityComparer();
    public static readonly IEqualityComparer<Color32> Color32 = new Color32EqualityComparer();
    public static readonly IEqualityComparer<Rect> Rect = new RectEqualityComparer();
    public static readonly IEqualityComparer<Bounds> Bounds = new BoundsEqualityComparer();
    public static readonly IEqualityComparer<Quaternion> Quaternion = new QuaternionEqualityComparer();

    static readonly RuntimeTypeHandle vector2Type = typeof(Vector2).TypeHandle;
    static readonly RuntimeTypeHandle vector3Type = typeof(Vector3).TypeHandle;
    static readonly RuntimeTypeHandle vector4Type = typeof(Vector4).TypeHandle;
    static readonly RuntimeTypeHandle colorType = typeof(Color).TypeHandle;
    static readonly RuntimeTypeHandle color32Type = typeof(Color32).TypeHandle;
    static readonly RuntimeTypeHandle rectType = typeof(Rect).TypeHandle;
    static readonly RuntimeTypeHandle boundsType = typeof(Bounds).TypeHandle;
    static readonly RuntimeTypeHandle quaternionType = typeof(Quaternion).TypeHandle;

#if UNITY_2017_2_OR_NEWER

    public static readonly IEqualityComparer<Vector2Int> Vector2Int = new Vector2IntEqualityComparer();
    public static readonly IEqualityComparer<Vector3Int> Vector3Int = new Vector3IntEqualityComparer();
    public static readonly IEqualityComparer<RangeInt> RangeInt = new RangeIntEqualityComparer();
    public static readonly IEqualityComparer<RectInt> RectInt = new RectIntEqualityComparer();
    public static readonly IEqualityComparer<BoundsInt> BoundsInt = new BoundsIntEqualityComparer();

    static readonly RuntimeTypeHandle vector2IntType = typeof(Vector2Int).TypeHandle;
    static readonly RuntimeTypeHandle vector3IntType = typeof(Vector3Int).TypeHandle;
    static readonly RuntimeTypeHandle rangeIntType = typeof(RangeInt).TypeHandle;
    static readonly RuntimeTypeHandle rectIntType = typeof(RectInt).TypeHandle;
    static readonly RuntimeTypeHandle boundsIntType = typeof(BoundsInt).TypeHandle;

#endif

    static class Cache<T>
    {
        public static readonly IEqualityComparer<T> Comparer;

        static Cache()
        {
            var comparer = GetDefaultHelper(typeof(T));
            if (comparer == null)
            {
                Comparer = EqualityComparer<T>.Default;
            }
            else
            {
                Comparer = (IEqualityComparer<T>) comparer;
            }
        }
    }

    public static IEqualityComparer<T> GetDefault<T>()
    {
        return Cache<T>.Comparer;
    }

    static object GetDefaultHelper(Type type)
    {
        var t = type.TypeHandle;

        if (t.Equals(vector2Type)) return (object) EqualityComparer.Vector2;
        if (t.Equals(vector3Type)) return (object) EqualityComparer.Vector3;
        if (t.Equals(vector4Type)) return (object) EqualityComparer.Vector4;
        if (t.Equals(colorType)) return (object) EqualityComparer.Color;
        if (t.Equals(color32Type)) return (object) EqualityComparer.Color32;
        if (t.Equals(rectType)) return (object) EqualityComparer.Rect;
        if (t.Equals(boundsType)) return (object) EqualityComparer.Bounds;
        if (t.Equals(quaternionType)) return (object) EqualityComparer.Quaternion;

#if UNITY_2017_2_OR_NEWER

        if (t.Equals(vector2IntType)) return (object) EqualityComparer.Vector2Int;
        if (t.Equals(vector3IntType)) return (object) EqualityComparer.Vector3Int;
        if (t.Equals(rangeIntType)) return (object) EqualityComparer.RangeInt;
        if (t.Equals(rectIntType)) return (object) EqualityComparer.RectInt;
        if (t.Equals(boundsIntType)) return (object) EqualityComparer.BoundsInt;
#endif

        return null;
    }

    sealed class Vector2EqualityComparer : IEqualityComparer<Vector2>
    {
        public bool Equals(Vector2 self, Vector2 vector)
        {
            return self.x.Equals(vector.x) && self.y.Equals(vector.y);
        }

        public int GetHashCode(Vector2 obj)
        {
            return obj.x.GetHashCode() ^ obj.y.GetHashCode() << 2;
        }
    }

    sealed class Vector3EqualityComparer : IEqualityComparer<Vector3>
    {
        public bool Equals(Vector3 self, Vector3 vector)
        {
            return self.x.Equals(vector.x) && self.y.Equals(vector.y) && self.z.Equals(vector.z);
        }

        public int GetHashCode(Vector3 obj)
        {
            return obj.x.GetHashCode() ^ obj.y.GetHashCode() << 2 ^ obj.z.GetHashCode() >> 2;
        }
    }

    sealed class Vector4EqualityComparer : IEqualityComparer<Vector4>
    {
        public bool Equals(Vector4 self, Vector4 vector)
        {
            return self.x.Equals(vector.x) && self.y.Equals(vector.y) && self.z.Equals(vector.z) &&
                   self.w.Equals(vector.w);
        }

        public int GetHashCode(Vector4 obj)
        {
            return obj.x.GetHashCode() ^ obj.y.GetHashCode() << 2 ^ obj.z.GetHashCode() >> 2 ^ obj.w.GetHashCode() >> 1;
        }
    }

    sealed class ColorEqualityComparer : IEqualityComparer<Color>
    {
        public bool Equals(Color self, Color other)
        {
            return self.r.Equals(other.r) && self.g.Equals(other.g) && self.b.Equals(other.b) && self.a.Equals(other.a);
        }

        public int GetHashCode(Color obj)
        {
            return obj.r.GetHashCode() ^ obj.g.GetHashCode() << 2 ^ obj.b.GetHashCode() >> 2 ^ obj.a.GetHashCode() >> 1;
        }
    }

    sealed class RectEqualityComparer : IEqualityComparer<Rect>
    {
        public bool Equals(Rect self, Rect other)
        {
            return self.x.Equals(other.x) && self.width.Equals(other.width) && self.y.Equals(other.y) &&
                   self.height.Equals(other.height);
        }

        public int GetHashCode(Rect obj)
        {
            return obj.x.GetHashCode() ^ obj.width.GetHashCode() << 2 ^ obj.y.GetHashCode() >> 2 ^
                   obj.height.GetHashCode() >> 1;
        }
    }

    sealed class BoundsEqualityComparer : IEqualityComparer<Bounds>
    {
        public bool Equals(Bounds self, Bounds vector)
        {
            return self.center.Equals(vector.center) && self.extents.Equals(vector.extents);
        }

        public int GetHashCode(Bounds obj)
        {
            return obj.center.GetHashCode() ^ obj.extents.GetHashCode() << 2;
        }
    }

    sealed class QuaternionEqualityComparer : IEqualityComparer<Quaternion>
    {
        public bool Equals(Quaternion self, Quaternion vector)
        {
            return self.x.Equals(vector.x) && self.y.Equals(vector.y) && self.z.Equals(vector.z) &&
                   self.w.Equals(vector.w);
        }

        public int GetHashCode(Quaternion obj)
        {
            return obj.x.GetHashCode() ^ obj.y.GetHashCode() << 2 ^ obj.z.GetHashCode() >> 2 ^ obj.w.GetHashCode() >> 1;
        }
    }

    sealed class Color32EqualityComparer : IEqualityComparer<Color32>
    {
        public bool Equals(Color32 self, Color32 vector)
        {
            return self.a.Equals(vector.a) && self.r.Equals(vector.r) && self.g.Equals(vector.g) &&
                   self.b.Equals(vector.b);
        }

        public int GetHashCode(Color32 obj)
        {
            return obj.a.GetHashCode() ^ obj.r.GetHashCode() << 2 ^ obj.g.GetHashCode() >> 2 ^ obj.b.GetHashCode() >> 1;
        }
    }

#if UNITY_2017_2_OR_NEWER

    sealed class Vector2IntEqualityComparer : IEqualityComparer<Vector2Int>
    {
        public bool Equals(Vector2Int self, Vector2Int vector)
        {
            return self.x.Equals(vector.x) && self.y.Equals(vector.y);
        }

        public int GetHashCode(Vector2Int obj)
        {
            return obj.x.GetHashCode() ^ obj.y.GetHashCode() << 2;
        }
    }

    sealed class Vector3IntEqualityComparer : IEqualityComparer<Vector3Int>
    {
        public static readonly Vector3IntEqualityComparer Default = new Vector3IntEqualityComparer();

        public bool Equals(Vector3Int self, Vector3Int vector)
        {
            return self.x.Equals(vector.x) && self.y.Equals(vector.y) && self.z.Equals(vector.z);
        }

        public int GetHashCode(Vector3Int obj)
        {
            return obj.x.GetHashCode() ^ obj.y.GetHashCode() << 2 ^ obj.z.GetHashCode() >> 2;
        }
    }

    sealed class RangeIntEqualityComparer : IEqualityComparer<RangeInt>
    {
        public bool Equals(RangeInt self, RangeInt vector)
        {
            return self.start.Equals(vector.start) && self.length.Equals(vector.length);
        }

        public int GetHashCode(RangeInt obj)
        {
            return obj.start.GetHashCode() ^ obj.length.GetHashCode() << 2;
        }
    }

    sealed class RectIntEqualityComparer : IEqualityComparer<RectInt>
    {
        public bool Equals(RectInt self, RectInt other)
        {
            return self.x.Equals(other.x) && self.width.Equals(other.width) && self.y.Equals(other.y) &&
                   self.height.Equals(other.height);
        }

        public int GetHashCode(RectInt obj)
        {
            return obj.x.GetHashCode() ^ obj.width.GetHashCode() << 2 ^ obj.y.GetHashCode() >> 2 ^
                   obj.height.GetHashCode() >> 1;
        }
    }

    sealed class BoundsIntEqualityComparer : IEqualityComparer<BoundsInt>
    {
        public bool Equals(BoundsInt self, BoundsInt vector)
        {
            return Vector3IntEqualityComparer.Default.Equals(self.position, vector.position)
                   && Vector3IntEqualityComparer.Default.Equals(self.size, vector.size);
        }

        public int GetHashCode(BoundsInt obj)
        {
            return Vector3IntEqualityComparer.Default.GetHashCode(obj.position) ^
                   Vector3IntEqualityComparer.Default.GetHashCode(obj.size) << 2;
        }
    }

#endif
}

/// <summary>
/// Inspectable ReactiveProperty.
/// </summary>
[Serializable]
public class IntReactiveProperty : Observable<int>
{
    public IntReactiveProperty()
        : base()
    {
    }

    public IntReactiveProperty(int initialValue)
        : base(initialValue)
    {
    }
}

/// <summary>
/// Inspectable ReactiveProperty.
/// </summary>
[Serializable]
public class LongReactiveProperty : Observable<long>
{
    public LongReactiveProperty()
        : base()
    {
    }

    public LongReactiveProperty(long initialValue)
        : base(initialValue)
    {
    }
}


/// <summary>
/// Inspectable ReactiveProperty.
/// </summary>
[Serializable]
public class ByteReactiveProperty : Observable<byte>
{
    public ByteReactiveProperty()
        : base()
    {
    }

    public ByteReactiveProperty(byte initialValue)
        : base(initialValue)
    {
    }
}

/// <summary>
/// Inspectable ReactiveProperty.
/// </summary>
[Serializable]
public class FloatReactiveProperty : Observable<float>
{
    public FloatReactiveProperty()
        : base()
    {
    }

    public FloatReactiveProperty(float initialValue)
        : base(initialValue)
    {
    }
}

/// <summary>
/// Inspectable ReactiveProperty.
/// </summary>
[Serializable]
public class DoubleReactiveProperty : Observable<double>
{
    public DoubleReactiveProperty()
        : base()
    {
    }

    public DoubleReactiveProperty(double initialValue)
        : base(initialValue)
    {
    }
}

/// <summary>
/// Inspectable ReactiveProperty.
/// </summary>
[Serializable]
public class StringReactiveProperty : Observable<string>
{
    public StringReactiveProperty()
        : base()
    {
    }

    public StringReactiveProperty(string initialValue)
        : base(initialValue)
    {
    }
}

/// <summary>
/// Inspectable ReactiveProperty.
/// </summary>
[Serializable]
public class BoolReactiveProperty : Observable<bool>
{
    public BoolReactiveProperty()
        : base()
    {
    }

    public BoolReactiveProperty(bool initialValue)
        : base(initialValue)
    {
    }
}

/// <summary>Inspectable ReactiveProperty.</summary>
[Serializable]
public class Vector2ReactiveProperty : Observable<Vector2>
{
    public Vector2ReactiveProperty()
    {
    }

    public Vector2ReactiveProperty(Vector2 initialValue)
        : base(initialValue)
    {
    }

    protected override IEqualityComparer<Vector2> EqualityComparer
    {
        get { return global::EqualityComparer.Vector2; }
    }
}

/// <summary>Inspectable ReactiveProperty.</summary>
[Serializable]
public class Vector3ReactiveProperty : Observable<Vector3>
{
    public Vector3ReactiveProperty()
    {
    }

    public Vector3ReactiveProperty(Vector3 initialValue)
        : base(initialValue)
    {
    }

    protected override IEqualityComparer<Vector3> EqualityComparer
    {
        get { return global::EqualityComparer.Vector3; }
    }
}

/// <summary>Inspectable ReactiveProperty.</summary>
[Serializable]
public class Vector4ReactiveProperty : Observable<Vector4>
{
    public Vector4ReactiveProperty()
    {
    }

    public Vector4ReactiveProperty(Vector4 initialValue)
        : base(initialValue)
    {
    }

    protected override IEqualityComparer<Vector4> EqualityComparer
    {
        get { return global::EqualityComparer.Vector4; }
    }
}

/// <summary>Inspectable ReactiveProperty.</summary>
[Serializable]
public class ColorReactiveProperty : Observable<Color>
{
    public ColorReactiveProperty()
    {
    }

    public ColorReactiveProperty(Color initialValue)
        : base(initialValue)
    {
    }

    protected override IEqualityComparer<Color> EqualityComparer
    {
        get { return global::EqualityComparer.Color; }
    }
}

/// <summary>Inspectable ReactiveProperty.</summary>
[Serializable]
public class RectReactiveProperty : Observable<Rect>
{
    public RectReactiveProperty()
    {
    }

    public RectReactiveProperty(Rect initialValue)
        : base(initialValue)
    {
    }

    protected override IEqualityComparer<Rect> EqualityComparer
    {
        get { return global::EqualityComparer.Rect; }
    }
}

/// <summary>Inspectable ReactiveProperty.</summary>
[Serializable]
public class AnimationCurveReactiveProperty : Observable<AnimationCurve>
{
    public AnimationCurveReactiveProperty()
    {
    }

    public AnimationCurveReactiveProperty(AnimationCurve initialValue)
        : base(initialValue)
    {
    }
}

/// <summary>Inspectable ReactiveProperty.</summary>
[Serializable]
public class BoundsReactiveProperty : Observable<Bounds>
{
    public BoundsReactiveProperty()
    {
    }

    public BoundsReactiveProperty(Bounds initialValue)
        : base(initialValue)
    {
    }

    protected override IEqualityComparer<Bounds> EqualityComparer
    {
        get { return global::EqualityComparer.Bounds; }
    }
}

/// <summary>Inspectable ReactiveProperty.</summary>
[Serializable]
public class QuaternionReactiveProperty : Observable<Quaternion>
{
    public QuaternionReactiveProperty()
    {
    }

    public QuaternionReactiveProperty(Quaternion initialValue)
        : base(initialValue)
    {
    }

    protected override IEqualityComparer<Quaternion> EqualityComparer
    {
        get { return global::EqualityComparer.Quaternion; }
    }
}


#if UNITY_EDITOR

[System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
public class InspectorDisplayAttribute : PropertyAttribute
{
    public string FieldName { get; private set; }
    public bool NotifyPropertyChanged { get; private set; }

    public InspectorDisplayAttribute(string fieldName = "value", bool notifyPropertyChanged = true)
    {
        FieldName = fieldName;
        NotifyPropertyChanged = notifyPropertyChanged;
    }
}

/// <summary>
/// Enables multiline input field for StringReactiveProperty. Default line is 3.
/// </summary>
[System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
public class MultilineReactivePropertyAttribute : PropertyAttribute
{
    public int Lines { get; private set; }

    public MultilineReactivePropertyAttribute()
    {
        Lines = 3;
    }

    public MultilineReactivePropertyAttribute(int lines)
    {
        this.Lines = lines;
    }
}

/// <summary>
/// Enables range input field for Int/FloatReactiveProperty.
/// </summary>
[System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
public class RangeReactivePropertyAttribute : PropertyAttribute
{
    public float Min { get; private set; }
    public float Max { get; private set; }

    public RangeReactivePropertyAttribute(float min, float max)
    {
        this.Min = min;
        this.Max = max;
    }
}

#endif

#if UNITY_EDITOR


// InspectorDisplay and for Specialized ReactiveProperty
// If you want to customize other specialized ReactiveProperty
// [UnityEditor.CustomPropertyDrawer(typeof(YourSpecializedReactiveProperty))]
// public class ExtendInspectorDisplayDrawer : InspectorDisplayDrawer { } 

[UnityEditor.CustomPropertyDrawer(typeof(InspectorDisplayAttribute))]
[UnityEditor.CustomPropertyDrawer(typeof(IntReactiveProperty))]
[UnityEditor.CustomPropertyDrawer(typeof(LongReactiveProperty))]
[UnityEditor.CustomPropertyDrawer(typeof(ByteReactiveProperty))]
[UnityEditor.CustomPropertyDrawer(typeof(FloatReactiveProperty))]
[UnityEditor.CustomPropertyDrawer(typeof(DoubleReactiveProperty))]
[UnityEditor.CustomPropertyDrawer(typeof(StringReactiveProperty))]
[UnityEditor.CustomPropertyDrawer(typeof(BoolReactiveProperty))]
[UnityEditor.CustomPropertyDrawer(typeof(Vector2ReactiveProperty))]
[UnityEditor.CustomPropertyDrawer(typeof(Vector3ReactiveProperty))]
[UnityEditor.CustomPropertyDrawer(typeof(Vector4ReactiveProperty))]
[UnityEditor.CustomPropertyDrawer(typeof(ColorReactiveProperty))]
[UnityEditor.CustomPropertyDrawer(typeof(RectReactiveProperty))]
[UnityEditor.CustomPropertyDrawer(typeof(AnimationCurveReactiveProperty))]
[UnityEditor.CustomPropertyDrawer(typeof(BoundsReactiveProperty))]
[UnityEditor.CustomPropertyDrawer(typeof(QuaternionReactiveProperty))]
public class InspectorDisplayDrawer : UnityEditor.PropertyDrawer
{
    public override void OnGUI(Rect position, UnityEditor.SerializedProperty property, GUIContent label)
    {
        string fieldName;
        bool notifyPropertyChanged;
        {
            var attr = this.attribute as InspectorDisplayAttribute;
            fieldName = (attr == null) ? "value" : attr.FieldName;
            notifyPropertyChanged = (attr == null) ? true : attr.NotifyPropertyChanged;
        }

        if (notifyPropertyChanged)
        {
            EditorGUI.BeginChangeCheck();
        }

        var targetSerializedProperty = property.FindPropertyRelative(fieldName);
        if (targetSerializedProperty == null)
        {
            UnityEditor.EditorGUI.LabelField(position, label,
                new GUIContent() {text = "InspectorDisplay can't find target:" + fieldName});
            if (notifyPropertyChanged)
            {
                EditorGUI.EndChangeCheck();
            }

            return;
        }
        else
        {
            EmitPropertyField(position, targetSerializedProperty, label);
        }

        if (notifyPropertyChanged)
        {
            if (EditorGUI.EndChangeCheck())
            {
                property.serializedObject.ApplyModifiedProperties();

                var paths = property.propertyPath.Split('.'); // X.Y.Z...
                var attachedComponent = property.serializedObject.targetObject;

                var targetProp = (paths.Length == 1)
                    ? fieldInfo.GetValue(attachedComponent)
                    : GetValueRecursive(attachedComponent, 0, paths);
                if (targetProp == null) return;
                var propInfo = targetProp.GetType().GetProperty(fieldName,
                    BindingFlags.IgnoreCase | BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public |
                    BindingFlags.NonPublic);
                var modifiedValue = propInfo.GetValue(targetProp, null); // retrieve new value

                var methodInfo = targetProp.GetType().GetMethod("SetValueAndForceNotify",
                    BindingFlags.IgnoreCase | BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public |
                    BindingFlags.NonPublic);
                if (methodInfo != null)
                {
                    methodInfo.Invoke(targetProp, new object[] {modifiedValue});
                }
            }
            else
            {
                property.serializedObject.ApplyModifiedProperties();
            }
        }
    }

    object GetValueRecursive(object obj, int index, string[] paths)
    {
        var path = paths[index];

        FieldInfo fldInfo = null;
        var type = obj.GetType();
        while (fldInfo == null)
        {
            // attempt to get information about the field
            fldInfo = type.GetField(path,
                BindingFlags.IgnoreCase | BindingFlags.GetField | BindingFlags.Instance | BindingFlags.Public |
                BindingFlags.NonPublic);

            if (fldInfo != null ||
                type.BaseType == null ||
                type.BaseType.IsSubclassOf(typeof(Observable<>))) break;

            // if the field information is missing, it may be in the base class
            type = type.BaseType;
        }

        // If array, path = Array.data[index]
        if (fldInfo == null && path == "Array")
        {
            try
            {
                path = paths[++index];
                var m = Regex.Match(path, @"(.+)\[([0-9]+)*\]");
                var arrayIndex = int.Parse(m.Groups[2].Value);
                var arrayValue = (obj as System.Collections.IList)[arrayIndex];
                if (index < paths.Length - 1)
                {
                    return GetValueRecursive(arrayValue, ++index, paths);
                }
                else
                {
                    return arrayValue;
                }
            }
            catch
            {
                Debug.Log("InspectorDisplayDrawer Exception, objType:" + obj.GetType().Name + " path:" +
                          string.Join(", ", paths));
                throw;
            }
        }
        else if (fldInfo == null)
        {
            throw new Exception("Can't decode path, please report to UniRx's GitHub issues:" +
                                string.Join(", ", paths));
        }

        var v = fldInfo.GetValue(obj);
        if (index < paths.Length - 1)
        {
            return GetValueRecursive(v, ++index, paths);
        }

        return v;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var attr = this.attribute as InspectorDisplayAttribute;
        var fieldName = (attr == null) ? "value" : attr.FieldName;

        var height = base.GetPropertyHeight(property, label);
        var valueProperty = property.FindPropertyRelative(fieldName);
        if (valueProperty == null)
        {
            return height;
        }

        if (valueProperty.propertyType == SerializedPropertyType.Rect)
        {
            return height * 2;
        }

        if (valueProperty.propertyType == SerializedPropertyType.Bounds)
        {
            return height * 3;
        }

        if (valueProperty.propertyType == SerializedPropertyType.String)
        {
            var multilineAttr = GetMultilineAttribute();
            if (multilineAttr != null)
            {
                return ((!EditorGUIUtility.wideMode) ? 16f : 0f) + 16f + (float) ((multilineAttr.Lines - 1) * 13);
            }

            ;
        }

        if (valueProperty.isExpanded)
        {
            var count = 0;
            var e = valueProperty.GetEnumerator();
            while (e.MoveNext()) count++;
            return ((height + 4) * count) + 6; // (Line = 20 + Padding) ?
        }

        return height;
    }

    protected virtual void EmitPropertyField(Rect position, UnityEditor.SerializedProperty targetSerializedProperty,
        GUIContent label)
    {
        var multiline = GetMultilineAttribute();
        if (multiline == null)
        {
            var range = GetRangeAttribute();
            if (range == null)
            {
                UnityEditor.EditorGUI.PropertyField(position, targetSerializedProperty, label, includeChildren: true);
            }
            else
            {
                if (targetSerializedProperty.propertyType == SerializedPropertyType.Float)
                {
                    EditorGUI.Slider(position, targetSerializedProperty, range.Min, range.Max, label);
                }
                else if (targetSerializedProperty.propertyType == SerializedPropertyType.Integer)
                {
                    EditorGUI.IntSlider(position, targetSerializedProperty, (int) range.Min, (int) range.Max, label);
                }
                else
                {
                    EditorGUI.LabelField(position, label.text, "Use Range with float or int.");
                }
            }
        }
        else
        {
            var property = targetSerializedProperty;

            label = EditorGUI.BeginProperty(position, label, property);
            var method = typeof(EditorGUI).GetMethod("MultiFieldPrefixLabel",
                BindingFlags.Static | BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.NonPublic);
            position = (Rect) method.Invoke(null, new object[] {position, 0, label, 1});

            EditorGUI.BeginChangeCheck();
            int indentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            var stringValue = EditorGUI.TextArea(position, property.stringValue);
            EditorGUI.indentLevel = indentLevel;
            if (EditorGUI.EndChangeCheck())
            {
                property.stringValue = stringValue;
            }

            EditorGUI.EndProperty();
        }
    }

    MultilineReactivePropertyAttribute GetMultilineAttribute()
    {
        var fi = this.fieldInfo;
        if (fi == null) return null;
        return fi.GetCustomAttributes(false).OfType<MultilineReactivePropertyAttribute>().FirstOrDefault();
    }

    RangeReactivePropertyAttribute GetRangeAttribute()
    {
        var fi = this.fieldInfo;
        if (fi == null) return null;
        return fi.GetCustomAttributes(false).OfType<RangeReactivePropertyAttribute>().FirstOrDefault();
    }
}

#endif


#if !(NETFX_CORE || NET_4_6 || NET_STANDARD_2_0 || UNITY_WSA_10_0)
/// <summary>
/// Simple IObserver implementation for Unity
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IObserver<T>
{
    void OnCompleted();
    void OnError(Exception error);
    void OnNext(T value);
}

/// <summary>
/// Simple IObservable implementation for Unity
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IObservable<T>
{
    IDisposable Subscribe(IObserver<T> observer);
}

#endif


///// <summary>
///// Simple IObservable implementation for Unity
///// </summary>
///// <typeparam name="T"></typeparam>
//public interface IObservable<K, V>
//{
//    IDisposable Subscribe(IObserver<K, V> observer);
//}
///// <summary>
///// Simple IObserver implementation for Unity
///// </summary>
///// <typeparam name="T"></typeparam>
//public interface IObserver<K, V>
//{
//    void OnCompleted();
//    void OnError(Exception error);
//    void OnNext(K value);
//    
//    void OnNext(V value);
//}