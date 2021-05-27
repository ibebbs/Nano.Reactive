using System;

namespace Nano.Reactive.Linq
{
    public static partial class Observable
    {
        private class PublishObservable<T> : IConnectableObservable<T>
        {
            private readonly IObservable<T> _source;
            private readonly Subject<T> _subject;

            public PublishObservable(IObservable<T> source)
            {
                _source = source;
                _subject = new Subject<T>();
            }

            public IDisposable Connect()
            {
                return _source.Subscribe(_subject);
            }

            public IDisposable Subscribe(IObserver<T> observer)
            {
                return _subject.Subscribe(observer);
            }
        }

        public static IConnectableObservable<T> Publish<T>(this IObservable<T> source)
        {
            return new PublishObservable<T>(source);
        }
    }
}
