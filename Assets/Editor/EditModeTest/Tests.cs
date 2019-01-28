using System;
using NUnit.Framework;
using System.Reflection;
using Firebase.Database;
using UniRx;

public class Tests
{
    [Test]
    public void ToArray()
    {
        var subject = new Subject<int>();

        int[] array = null;
        subject.ToArray().Subscribe(xs => array = xs);

        subject.OnNext(1);
        subject.OnNext(10);
        subject.OnNext(100);
        subject.OnCompleted();

        array.Is(1, 10, 100);
    }

    [Test]
    public void TestRange()
    {
        var subject = new Subject<int>();

        int[] array = null;
        subject.ToArray().Subscribe(xs => array = xs);

        subject.OnNext(1);
        subject.OnNext(10);
        subject.OnNext(100);
        subject.OnCompleted();

        array.Is(1, 10, 100);
    }
}