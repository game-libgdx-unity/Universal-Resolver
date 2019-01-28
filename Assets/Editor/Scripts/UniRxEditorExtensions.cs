using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public static class UniRxEditorExtensions
{
    public static ObservableYieldInstruction<T> IsValued<T>(
        this IObservable<T> observable,
        T expectedValue)
    {
        return observable.Where(v => v.Equals(expectedValue)).Take(1).ToYieldInstruction();
    }


    public static ObservableYieldInstruction<T> IsValued<T>(
        this IObservable<T> observable,
        Func<T, bool> predicate)
    {
        return observable.Where(predicate).Take(1).ToYieldInstruction();
    }

    public static ObservableYieldInstruction<T> IsValued<T>(
        this IObservable<T> observable)
    {
        return observable.Take(1).ToYieldInstruction();
    }

    public static IEnumerator Is<T>(
        this IObservable<T> observable,
        params T[] expectedValue)
    {
        bool finished = false;
        observable.ToList().Take(1).Subscribe(list =>
        {
            list.Is(expectedValue);
            finished = true;
        });

        yield return new WaitUntil(() => finished);
    }

    public static void WillFail<T>(
        this IObservable<T> observable)
    {
        Assert.Fail("just fail");
    }

    public static void WillPass<T>(
        this IObservable<T> observable)
    {
        Assert.Pass("passed");
    }
}