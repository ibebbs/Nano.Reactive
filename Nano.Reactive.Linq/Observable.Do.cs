using System;

namespace Nano.Reactive.Linq
{
    public static partial class Observable
    {
        private class DoObservable<T> : IObservable<T>
        {
            private readonly IObservable<T> _source;
            private readonly Action<T> _do;

            public DoObservable(IObservable<T> source, Action<T> @do)
            {
                _source = source;
                _do = @do;
            }

            public IDisposable Subscribe(IObserver<T> observer)
            {
                return _source.Subscribe(
                    Observer.Create<T>(
                        item =>
                        {
                            _do(item);
                            observer.OnNext(item);
                        },
                        observer.OnError,
                        observer.OnCompleted
                    )
                );
            }
        }

        public static IObservable<T> Do<T>(this IObservable<T> source, Action<T> @do)
        {
            return new DoObservable<T>(source, @do);
        }
    }
}
