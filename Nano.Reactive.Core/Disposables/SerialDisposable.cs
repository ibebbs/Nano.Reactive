using System;

namespace Nano.Reactive.Disposables
{
    public class SerialDisposable : IDisposable
    {
        private IDisposable _disposable;

        public SerialDisposable(IDisposable disposable)
        {
            _disposable = disposable;
        }

        public void Dispose()
        {
            if (Disposable != null)
            {
                Disposable.Dispose();
                Disposable = null;
            }
        }

        public IDisposable Disposable 
        {
            get { return _disposable; } 
            set
            {
                _disposable?.Dispose();
                _disposable = value;
            }
        }
    }
}
