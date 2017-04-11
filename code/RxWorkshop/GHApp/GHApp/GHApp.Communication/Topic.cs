using System;
using System.Reactive.Linq;
using Microsoft.Practices.Unity;

namespace GHApp.Communication
{
    /// <summary>
    ///     Observes client messages, notifies on server stream
    /// </summary>
    public class Topic<T> : ITopic<T>
        where T : class, IMessage
    {
        private readonly ICommunicationChannel _messageChannel;

        public Topic([Dependency(ChannelNames.Server)] ICommunicationChannel messageChannel)
        {
            _messageChannel = messageChannel;
        }

        public IObservable<T> Messages
        {
            get
            {
                return _messageChannel.MessageStream
                    .Where(m => m != null && m.GetType() == typeof(T))
                    .Cast<T>();
            }
        }
    }
}