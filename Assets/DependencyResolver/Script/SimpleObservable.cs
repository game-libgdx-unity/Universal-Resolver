using System;
using System.Collections.Generic;

public class SimpleObservable<T> : IObservable<T>, IDisposable where T : class
{
    private List<IObserver<T>> observers = new List<IObserver<T>>();

    public IDisposable Subscribe(IObserver<T> observer)
    {
        this.observers.Add(observer);
        observer.OnNext(this as T);
        return Disposable.Empty;
    }

    public void Dispose()
    {
        observers.ForEach(ob =>
        {
            if (ob != null) ob.OnCompleted();
        });
    }
}