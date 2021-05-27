using System;

namespace Nano.Reactive.Linq
{
    public static partial class Observable
    {
        private class MergeObservable<T> : IObservable<T>
        {
            private readonly IObservable<T>[] _sources;

            public MergeObservable(IObservable<T>[] sources)
            {
                _sources = sources;
            }

            public IDisposable Subscribe(IObserver<T> observer)
            {
                var subscriptions = new IDisposable[_sources.Length];

                for (int i = 0; i < _sources.Length; i++)
                {
                    subscriptions[i] = _sources[i].Subscribe(observer);
                }

                return new Disposables.CompositeDisposable(subscriptions);
            }
        }

        public static IObservable<T> Merge<T>(params IObservable<T>[] sources)
        {
            return new MergeObservable<T>(sources);
        }
    }
}
