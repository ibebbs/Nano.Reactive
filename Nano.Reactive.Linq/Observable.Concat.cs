using System;

namespace Nano.Reactive.Linq
{
    public static partial class Observable
    {
        private class ConcatObservable<T> : IObservable<T>
        {
            private readonly IObservable<T>[] _sources;

            public ConcatObservable(IObservable<T>[] sources)
            {
                _sources = sources;
            }

            public IDisposable Subscribe(IObserver<T> observer)
            {
                var disposable = new Disposables.SerialDisposable(Disposable.Empty);

                int source = 0;

                IObserver<T> sourceObserver = Observer.Empty<T>();

                sourceObserver = Observer.Create<T>(
                    observer.OnNext, 
                    observer.OnError, 
                    () =>
                    {
                        if (++source < _sources.Length)
                        {
                            disposable.Disposable = _sources[source].Subscribe(sourceObserver);
                        }
                        else
                        {
                            disposable.Dispose();
                            observer.OnCompleted();
                        }
                    }
                );

                disposable.Disposable = _sources[source].Subscribe(sourceObserver);

                return disposable;
            }
        }

        public static IObservable<T> Concat<T>(this IObservable<T> source, IObservable<T> next)
        {
            return new ConcatObservable<T>(new[] { source, next });
        }
    }
}
