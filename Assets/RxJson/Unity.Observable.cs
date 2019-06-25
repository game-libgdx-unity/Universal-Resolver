using System;
using UnityEngine;

namespace UniRx
{
    public static partial class Observable
    {
        public static IObservable<T> TakeWhileEnable<T>(this IObservable<T> source, Behaviour target)
        {
            return source.TakeWhile(_ => target.enabled);
        }

        public static IObservable<T> TakeWhileEnable<T>(this IObservable<T> source, Component target)
        {
            return source.TakeWhile(_ => target.gameObject.activeInHierarchy);
        }
    }
}