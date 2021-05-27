using System;

namespace Nano.Reactive.Linq
{
    public static partial class Observable
    {
        private class ReturnObservable<T> : IObservable<T>
        {
            private readonly T _value;

            public ReturnObservable(T value)
            {
                _value = value;
            }

            public IDisposable Subscribe(IObserver<T> observer)
            {
                observer.OnNext(_value);

                return Disposable.Empty;
            }
        }

        public static IObservable<T> Return<T>(T value)
        {
            return new ReturnObservable<T>(value);
        }
    }
}
