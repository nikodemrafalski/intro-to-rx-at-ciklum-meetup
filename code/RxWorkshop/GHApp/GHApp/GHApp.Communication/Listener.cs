using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Microsoft.Practices.Unity;

namespace GHApp.Communication
{
    /// <summary>
    ///     Observes client messages, notifies on server stream
    /// </summary>
    /// <typeparam name="TQuery"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public class Listener<TQuery, TResponse> : IListener<TQuery, TResponse>
        where TQuery : class, IMessage
        where TResponse : class, IMessage
    {
        private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();
        private readonly ICommunicationChannel _queryChannel;
        private readonly ICommunicationChannel _responseChannel;

        public Listener([Dependency(ChannelNames.Client)] ICommunicationChannel queryChannel,
            [Dependency(ChannelNames.Server)] ICommunicationChannel responseChannel)
        {
            _queryChannel = queryChannel;
            _responseChannel = responseChannel;
        }

        public void Listen(Func<TQuery, TResponse> processFunc)
        {
            var disposable = _queryChannel.MessageStream
                .Where(m => m != null && m.GetType() == typeof(TQuery))
                .Cast<TQuery>()
                .Subscribe(m => _compositeDisposable.Add(_responseChannel.SendMessage(processFunc(m)).Subscribe()));
            _compositeDisposable.Add(disposable);
        }

        public void ListenObservable(Func<TQuery, IObservable<TResponse>> processFunc)
        {
            var disposable = _queryChannel.MessageStream
                .Where(m => m != null && m.GetType() == typeof(TQuery))
                .Cast<TQuery>()
                .Subscribe(m => _compositeDisposable.Add(processFunc(m)
                    .Subscribe(p => _compositeDisposable.Add(_responseChannel.SendMessage(p).Subscribe()))));
            _compositeDisposable.Add(disposable);
        }

        public void Dispose()
        {
            _compositeDisposable.Dispose();
        }
    }
}