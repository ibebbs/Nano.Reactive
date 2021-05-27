using System;

namespace Nano.Reactive
{
    public interface IConnectableObservable<T> : IObservable<T>
    {
        IDisposable Connect();
    }
}
