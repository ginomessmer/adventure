using System;

namespace Adventure.Core.Networking.Abstractions
{
    public abstract class SocketClient : ISocketAgent, IDisposable
    {
        public abstract void Start();

        public abstract void SendMessage(string message);

        public abstract void SendInitialMessage();

        public abstract event EventHandler<SocketMessageReceivedArgs> OnMessageReceived;

        public abstract void Shutdown();

        public void Dispose() => Shutdown();
    }
}
