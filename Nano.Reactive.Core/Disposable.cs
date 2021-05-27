using System;

namespace Nano.Reactive
{
    public static class Disposable
    {
        private class ActionDisposable : IDisposable
        {
            private readonly Action _action;

            public ActionDisposable(Action action)
            {
                _action = action;
            }

            public void Dispose()
            {
                _action();
            }
        }

        public static readonly IDisposable Empty = new ActionDisposable(() => { });

        public static IDisposable Create(Action action)
        {
            return new ActionDisposable(action);
        }
    }
}
