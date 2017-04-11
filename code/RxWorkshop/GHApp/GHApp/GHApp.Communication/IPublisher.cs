using System;

namespace GHApp.Communication
{
    public interface IPublisher<T> : IObserver<T>
    {
    }
}