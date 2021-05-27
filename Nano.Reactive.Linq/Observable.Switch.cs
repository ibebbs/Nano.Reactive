using System;

namespace Nano.Reactive.Linq
{
    public static partial class Observable
    {
        private class SwitchObservable<T> : IObservable<T>
        {
            private readonly IObservable<IObservable<T>> _sources;

            public SwitchObservable(IObservable<IObservable<T>> sources)
            {
                _sources = sources;
            }

            public IDisposable Subscribe(IObserver<T> observer)
            {
                var disposable = new Disposables.SerialDisposable(Disposable.Empty);

                var subscription = _sources.Subscribe(
                    Observer.Create<IObservable<T>>(
                        observable => disposable.Disposable = observable.Subscribe(observer),
                        observer.OnError,
                        observer.OnCompleted
                    )
                );

                return new Disposables.CompositeDisposable(disposable, subscription);
            }
        }

        public static IObservable<T> Switch<T>(this IObservable<IObservable<T>> sources)
        {
            return new SwitchObservable<T>(sources);
        }
    }
}
