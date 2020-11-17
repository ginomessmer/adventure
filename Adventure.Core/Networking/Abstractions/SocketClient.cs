using System;
using System.Net;

namespace Adventure.Core.Networking.Abstractions
{
    public abstract class SocketClient : ISocketAgent, IDisposable
    {
        /// <summary>
        /// The remote server's IP address.
        /// </summary>
        public abstract IPAddress ServerIPAddress { get; init; }

        /// <summary>
        /// The remote server's port.
        /// </summary>
        public abstract int ServerPort { get; init; }

        /// <summary>
        /// The remote server's endpoint.
        /// </summary>
        public abstract EndPoint ServerEndPoint { get; }

        public abstract void Start();

        public abstract void SendMessage(string message);

        public abstract void SendInitialMessage();

        public abstract event EventHandler OnConnected;

        public abstract event EventHandler<SocketConnectionMessageReceivedArgs> OnMessageReceived;

        public abstract void Shutdown();

        public void Dispose() => Shutdown();
    }
}
