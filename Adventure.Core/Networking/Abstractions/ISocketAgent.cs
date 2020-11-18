using System;
using System.Net.Sockets;

namespace Adventure.Core.Networking.Abstractions
{
    public interface ISocketAgent
    {
        void Start();

        void Shutdown();

        event EventHandler<SocketConnectionMessageReceivedArgs> OnMessageReceived;
    }
}