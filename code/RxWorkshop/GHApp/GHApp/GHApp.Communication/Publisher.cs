using System;
using Microsoft.Practices.Unity;

namespace GHApp.Communication
{
    /// <summary>
    ///     Observes server messages, sends to client stream
    /// </summary>
    public class Publisher<T> : IPublisher<T>
        where T : class, IMessage
    {
        private readonly ICommunicationChannel _publishChannel;

        public Publisher([Dependency(ChannelNames.Server)] ICommunicationChannel publishChannel)
        {
            _publishChannel = publishChannel;
        }

        public void OnNext(T value)
        {
            _publishChannel.SendMessage(value).Subscribe();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }
    }
}