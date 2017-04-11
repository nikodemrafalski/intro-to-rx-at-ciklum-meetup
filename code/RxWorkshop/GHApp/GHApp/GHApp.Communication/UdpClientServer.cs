using System;
using System.Net;
using System.Net.Sockets;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;

namespace GHApp.Communication
{
    public class UdpClientServer : IUdpClientServer
    {
        public IObservable<byte[]> Listen(int localPort)
        {
            return Observable.Using(
                () => new UdpClient(new IPEndPoint(IPAddress.Any, localPort)),
                udp => Observable.Defer(() => udp.ReceiveAsync().ToObservable())
                    .Repeat()
                    .Select(result => result.Buffer)
            );
        }

        public IObservable<Unit> Send(string remoteAddress, int port, byte[] data)
        {
            return Observable.Using(
                () => new UdpClient(new IPEndPoint(IPAddress.Any, 0)),
                udp => udp.SendAsync(data, data.Length, new IPEndPoint(IPAddress.Parse(remoteAddress), port))
                    .ToObservable()
                    .Select(_ => Unit.Default)
            );
        }
    }
}