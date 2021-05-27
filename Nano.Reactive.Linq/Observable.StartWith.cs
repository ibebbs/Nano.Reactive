using System;

namespace Nano.Reactive.Linq
{
    public static partial class Observable
    {
        public static IObservable<T> StartWith<T>(this IObservable<T> source, T value)
        {
            return Concat(new ReturnObservable<T>(value), source);
        }
    }
}
