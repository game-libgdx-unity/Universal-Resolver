using System;
using NUnit.Framework;
using UniRx;
using UnityEngine;

public class TestObserver<T> : IObserver<T>
{
    private readonly T subject;
    private bool passTest;

    public TestObserver(T subject)
    {
        this.subject = subject;
    }

    public void OnNext(T value)
    {
        if (passTest)
        {
            return;
        }

        if (value.Equals(subject))
        {
            passTest = true;
        }
    }

    public void OnError(Exception error)
    {
        Debug.LogException(error);
    }

    public void OnCompleted()
    {
        if (passTest)
        {
            Assert.Pass();
        }
        else
        {
            Assert.Fail();
        }
    }
}