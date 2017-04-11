using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;

namespace WpfClient
{
    public class ReceiverA
    {
        private HubConnection _hubConnection;

        public ReceiverA(string url = "http://localhost:8080/")
        {
            _hubConnection = new HubConnection(url);
        }

        public event EventHandler<LinkReceivedEventArgs> LinkReceived;
        public event EventHandler<HtmlReceivedEventArgs> HtmlReceived;

        public async void Start()
        {
            IHubProxy linksProxy = _hubConnection.CreateHubProxy("LinksHub");
            linksProxy.On<string>("Send", async link => await OnReceivedFromHub(link));
            await _hubConnection.Start();
        }

        private async Task OnReceivedFromHub(string link)
        {
            OnLinkReceived(link);
            var body = await new HttpClient().GetStringAsync(link);
            OnHtmlReceived(body);
        }

        public void Stop()
        {
            _hubConnection.Stop();
        }

        protected virtual void OnLinkReceived(string link)
        {
            LinkReceived?.Invoke(this, new LinkReceivedEventArgs { Link = link });
        }

        protected virtual void OnHtmlReceived(string html)
        {
            HtmlReceived?.Invoke(this, new HtmlReceivedEventArgs { Html = html });
        }
    }

    public class LinkReceivedEventArgs : EventArgs
    {
        public string Link { get; set; }
    }

    public class HtmlReceivedEventArgs : EventArgs
    {
        public string Html { get; set; }
    }
}
