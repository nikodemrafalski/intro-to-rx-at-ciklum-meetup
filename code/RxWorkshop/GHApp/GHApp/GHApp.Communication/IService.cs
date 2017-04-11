using System;

namespace GHApp.Communication
{
    public interface IService<in TQuery, out TResponse>
        where TQuery : class, IMessage
        where TResponse : class, IMessage
    {
        IObservable<TResponse> Query(TQuery parameter);
    }
}