using System;
using System.Collections;

namespace Nano.Reactive
{
    public class Subject<T> : IObservable<T>, IObserver<T>
    {
        private ArrayList _observers = new ArrayList();
        private object _syncRoot = new object();

        private IObserver<T>[] GetObservers()
        {
            var result = new IObserver<T>[_observers.Count];

            lock(_syncRoot)
            {
                _observers.CopyTo(result);
            }

            return result;
        }

        private void RemoveObservable(IObserver<T> observer)
        {
            lock(_syncRoot)
            {
                _observers.Remove(observer);
            }
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            if (observer == null) throw new ArgumentNullException(nameof(observer));

            lock(_syncRoot)
            {
                _observers.Add(observer);
            }

            return Disposable.Create(() => RemoveObservable(observer));
        }

        public void OnNext(T item)
        {
            try
            {
                var observers = GetObservers();

                foreach (var observer in observers)
                {
                    observer.OnNext(item);
                }
            }
            catch (Exception exception)
            {
                OnError(exception);
            }
        }

        public void OnError(Exception exception)
        {
            var observers = GetObservers();

            foreach (var observer in observers)
            {
                try
                {
                    observer.OnError(exception);
                }
                catch
                {
                    // Can't do anything here
                }
            }
        }

        public void OnCompleted()
        {
            try
            {
                var observers = GetObservers();

                foreach (var observer in observers)
                {
                    observer.OnCompleted();
                }
            }
            catch (Exception exception)
            {
                OnError(exception);
            }
        }
    }
}
