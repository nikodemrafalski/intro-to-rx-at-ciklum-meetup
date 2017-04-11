using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Microsoft.Practices.Unity;

namespace GHApp.Communication
{
    /// <summary>
    ///     Observes server messages, sends to client stream
    /// </summary>
    public class Service<TQuery, TResponse> : IService<TQuery, TResponse>
        where TQuery : class, IMessage
        where TResponse : class, IMessage
    {
        private readonly ICommunicationChannel _queryChannel;
        private readonly ICommunicationChannel _responseChannel;

        public Service([Dependency(ChannelNames.Client)] ICommunicationChannel queryChannel,
            [Dependency(ChannelNames.Server)] ICommunicationChannel responseChannel)
        {
            _queryChannel = queryChannel;
            _responseChannel = responseChannel;
        }

        public IObservable<TResponse> Query(TQuery parameter)
        {
            return Observable.Create<TResponse>(
                observer =>
                {
                    return new CompositeDisposable
                    {
                        _responseChannel.MessageStream
                            .Where(m => m != null && m.GetType() == typeof(TResponse))
                            .Cast<TResponse>()
                            .Where(m => m.Id == parameter.Id)
                            .Subscribe(observer),
                        _queryChannel.SendMessage(parameter).Subscribe()
                    };
                });
        }
    }
}