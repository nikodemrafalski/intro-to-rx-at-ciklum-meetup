using System;

namespace GHApp.Communication
{
    public interface ITopic<out T>
        where T : class, IMessage
    {
        IObservable<T> Messages { get; }
    }
}