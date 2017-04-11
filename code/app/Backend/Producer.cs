using System;
using System.IO;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Hosting;
using Owin;

namespace Backend
{
    public class SignalsHost : IDisposable
    {
        private IDisposable _stop;

        private class LinksHub : Hub
        {
            public void Publish(string link)
            {
                Clients.All.send(link);
            }
        }

        private class Startup
        {
            public void Configuration(IAppBuilder app)
            {
                app.MapSignalR();
            }
        }

        public SignalsHost()
        {
            _stop = WebApp.Start<Startup>("http://localhost:8080/");
            Console.WriteLine("Server running at http://localhost:8080/");
        }

        public void Send(string msg)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<LinksHub>();
            context.Clients.All.Send(msg);
        }

        public void Dispose()
        {
            _stop?.Dispose();
            Console.WriteLine("Server stoped");
        }
    }

    public class Producer
    {
        private readonly TimeSpan _valueSpan;
        private IDisposable _subscription = Disposable.Empty;
        private SignalsHost _signal;

        public Producer(TimeSpan valueSpan)
        {
            _valueSpan = valueSpan;
        }

        public void Start()
        {
            _signal = new SignalsHost();
            _subscription = File
                .ReadAllLines(@".\links.txt")
                .ToObservable()
                .Zip(Observable.Interval(_valueSpan), (v, i) => v)
                .Subscribe(v => _signal.Send(v));
        }

        public void Stop()
        {
            _subscription.Dispose();
            _signal.Dispose();
        }
    }
}
// without zip
// console logging
// enabling communication
// disposable, non blocking
