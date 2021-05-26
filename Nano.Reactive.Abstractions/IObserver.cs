using System;

namespace Nano.Reactive
{
    public interface IObserver<T>
    {
        void OnNext(T item);

        void OnError(Exception exception);

        void OnCompleted();
    }
}
