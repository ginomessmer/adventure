using System;

namespace Adventure.Core.Networking.Abstractions
{
    public abstract class SocketServer : ISocketAgent
    {
        public abstract void Start();

        public abstract void SendMessage(string message);

        public abstract event EventHandler<string> OnMessageReceived;

        public abstract void Shutdown();
    }
}