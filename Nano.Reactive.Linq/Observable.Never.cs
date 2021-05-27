using System;

namespace Nano.Reactive.Linq
{
    public static partial class Observable
    {
        private class NeverObservable<T> : IObservable<T>
        {
            public IDisposable Subscribe(IObserver<T> observer)
            {
                return Disposable.Empty;
            }
        }

        public static IObservable<T> Never<T>()
        {
            return new NeverObservable<T>();
        }
    }
}
