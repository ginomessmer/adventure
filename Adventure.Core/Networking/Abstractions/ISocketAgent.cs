using System;
using System.Threading;

namespace Adventure.Core.Networking.Abstractions
{
    public interface ISocketAgent
    {
        void Start();

        void Shutdown();

        void SendMessage(string message);

        event EventHandler<SocketConnectionMessageReceivedArgs> OnMessageReceived;
    }
}