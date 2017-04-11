using System;
using System.Diagnostics;
using System.Net.Http;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Microsoft.AspNet.SignalR.Client;

// error handling

namespace WpfClient
{
    public class ReceiverB
    {
        private HubConnection _hubConnection;

        private Subject<string> _linksSubject = new Subject<string>();
        private Subject<string> _htmlSubject = new Subject<string>();

        public IObservable<string> LinksStream => _linksSubject.AsObservable();
        public IObservable<string> HtmlStream => _htmlSubject.AsObservable();

        public ReceiverB(string url = "http://localhost:8080/")
        {
            // show - distinct until changed
            // disposable - handling

            _hubConnection = new HubConnection(url);
            LinksStream
                .Do(l => Debug.WriteLine(l))
                .SelectMany(link => new HttpClient().GetStringAsync(link))
                .Subscribe(_htmlSubject);
        }

        public async void Start()
        {
            try
            {
                IHubProxy linksProxy = _hubConnection.CreateHubProxy("LinksHub");
                linksProxy.On<string>("Send", _linksSubject.OnNext);
                await _hubConnection.Start();
            }
            catch (Exception e)
            {
                _linksSubject.OnError(e);
            }
        }

        public void Stop()
        {
            _hubConnection.Stop();
        }
    }
}



// .SelectMany(link => Observable.Using(() => new HttpClient(), client => Observable.FromAsync(() => client.GetStringAsync(link))))