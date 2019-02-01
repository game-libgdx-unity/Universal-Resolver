/**
 * Author:    Vinh Vu Thanh
 * This class is a part of Unity IoC project that can be downloaded free at 
 * https://github.com/game-libgdx-unity/UnityEngine.IoC
 * (c) Copyright by MrThanhVinh168@gmail.com
 **/
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