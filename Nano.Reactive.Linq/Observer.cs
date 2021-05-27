using System;

namespace Nano.Reactive.Linq
{
    public static class Observer
    {
        private class CreateObserver<T> : IObserver<T>
        {
            private readonly Action<T> _onNext;
            private readonly Action<Exception> _onError;
            private readonly Action _onCompleted;

            public CreateObserver(Action<T> onNext = null, Action<Exception> onError = null, Action onCompleted = null)
            {
                _onNext = onNext;
                _onError = onError;
                _onCompleted = onCompleted;
            }

            public void OnCompleted()
            {
                _onCompleted?.Invoke();
            }

            public void OnError(Exception exception)
            {
                _onError?.Invoke(exception);
            }

            public void OnNext(T item)
            {
                _onNext?.Invoke(item);
            }
        }

        public static IObserver<T> Create<T>(Action<T> onNext = null, Action<Exception> onError = null, Action onCompleted = null)
        {
            return new CreateObserver<T>(onNext, onError, onCompleted);
        }

        public static IObserver<T> Empty<T>()
        {
            return new CreateObserver<T>();
        }
    }
}
