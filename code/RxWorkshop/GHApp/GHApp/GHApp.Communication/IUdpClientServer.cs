using System;
using System.Reactive;

namespace GHApp.Communication
{
    public interface IUdpClientServer
    {
        IObservable<byte[]> Listen(int localPort);

        IObservable<Unit> Send(string remoteAddress, int port, byte[] data);
    }
}