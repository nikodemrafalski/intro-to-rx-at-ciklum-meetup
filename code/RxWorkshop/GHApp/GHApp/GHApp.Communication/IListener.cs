using System;

namespace GHApp.Communication
{
    public interface IListener<out TQuery, in TResponse> : IDisposable
        where TQuery : class, IMessage
        where TResponse : class, IMessage
    {
        void Listen(Func<TQuery, TResponse> processFunc);

        void ListenObservable(Func<TQuery, IObservable<TResponse>> processFunc);
    }
}