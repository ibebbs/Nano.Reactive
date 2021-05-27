using System;

namespace Nano.Reactive.Linq
{
    public static partial class Observable
    {
        private class CreateObservable<T> : IObservable<T>
        {
            private readonly Func<IObserver<T>, IDisposable> _factory;

            public CreateObservable(Func<IObserver<T>, IDisposable> factory)
            {
                _factory = factory;
            }

            public IDisposable Subscribe(IObserver<T> observer)
            {
                return _factory(observer);
            }
        }

        public static IObservable<T> Create<T>(Func<IObserver<T>, IDisposable> factory)
        {
            return new CreateObservable<T>(factory);
        }

        private class EventSubscription<TEventArgs, TDelegate> : IDisposable
        {
            private readonly IObserver<TEventArgs> _observer;
            private readonly Action<TDelegate> _removeHandler;
            private readonly TDelegate _delegate;

            public EventSubscription(IObserver<TEventArgs> observer, Func<EventHandler<TEventArgs>, TDelegate> conversion, Action<TDelegate> addHandler, Action<TDelegate> removeHandler)
            {
                _observer = observer;
                _removeHandler = removeHandler;
                _delegate = conversion(Handler);

                addHandler(_delegate);
            }

            public void Dispose()
            {
                _removeHandler(_delegate);
            }

            private void Handler(object sender, TEventArgs e)
            {
                _observer.OnNext(e);
            }
        }

        private class FromEventObservable<TEventArgs, TDelegate> : IObservable<TEventArgs>
        {
            private readonly Func<EventHandler<TEventArgs>, TDelegate> _conversion;
            private readonly Action<TDelegate> _addHandler;
            private readonly Action<TDelegate> _removeHandler;

            public FromEventObservable(Func<EventHandler<TEventArgs>, TDelegate> conversion, Action<TDelegate> addHandler, Action<TDelegate> removeHandler)
            {
                _conversion = conversion;
                _addHandler = addHandler;
                _removeHandler = removeHandler;
            }

            public IDisposable Subscribe(IObserver<TEventArgs> observer)
            {
                return new EventSubscription<TEventArgs, TDelegate>(observer, _conversion, _addHandler, _removeHandler);
            }
        }


        public static IObservable<TEventArgs> FromEvent<TEventArgs,TDelegate>(Func<EventHandler<TEventArgs>, TDelegate> conversion, Action<TDelegate> addHandler, Action<TDelegate> removeHandler)
        {
            return new FromEventObservable<TEventArgs, TDelegate>(conversion, addHandler, removeHandler);
        }
    }
}
