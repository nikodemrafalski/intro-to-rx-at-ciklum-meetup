using System;
using System.Reactive;

namespace GHApp.Communication
{
    public interface ICommunicationChannel : IDisposable
    {
        IObservable<object> MessageStream { get; }

        IObservable<Unit> SendMessage(object message);
    }
}