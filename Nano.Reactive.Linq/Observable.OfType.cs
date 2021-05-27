using System;

namespace Nano.Reactive.Linq
{
    public static partial class Observable
    {
        private class OfTypeObservable<TSource, TDest> : IObservable<TDest>
            where TDest : TSource
        {
            private readonly IObservable<TSource> _source;

            public OfTypeObservable(IObservable<TSource> source)
            {
                _source = source;
            }

            public IDisposable Subscribe(IObserver<TDest> observer)
            {
                return _source.Subscribe(
                    Observer.Create<TSource>(
                        item =>
                        {
                            if (item is TDest dest)
                            {
                                observer.OnNext(dest);
                            }
                        },
                        observer.OnError,
                        observer.OnCompleted
                    )
                );
            }
        }

        public static IObservable<TDest> OfType<TSource, TDest>(this IObservable<TSource> source)
            where TDest : TSource
        {
            return new OfTypeObservable<TSource, TDest>(source);
        }
    }
}
