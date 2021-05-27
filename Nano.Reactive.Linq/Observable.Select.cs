using System;

namespace Nano.Reactive.Linq
{
    public static partial class Observable
    {
        private class ProjectionObservable<TSource, TTarget> : IObservable<TTarget>
        {
            private readonly IObservable<TSource> _source;
            private readonly Func<TSource, TTarget> _projection;

            public ProjectionObservable(IObservable<TSource> source, Func<TSource, TTarget> projection)
            {
                _source = source;
                _projection = projection;
            }

            public IDisposable Subscribe(IObserver<TTarget> observer)
            {
                return _source.Subscribe(
                    Observer.Create<TSource>(
                        item => observer.OnNext(_projection(item)),
                        observer.OnError,
                        observer.OnCompleted
                    )
                );
            }
        }

        public static IObservable<TTarget> Select<TSource, TTarget>(this IObservable<TSource> source, Func<TSource, TTarget> projection)
        {
            return new ProjectionObservable<TSource, TTarget>(source, projection);
        }
    }
}
