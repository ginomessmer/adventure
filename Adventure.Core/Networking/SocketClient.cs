using System;
using System.Collections.Generic;
using System.Text;

namespace Adventure.Core.Networking
{
    public abstract class SocketClient : IDisposable
    {
        public abstract void Start();

        public abstract void SendMessage(string message);

        public abstract void SendInitialMessage();

        public abstract event EventHandler<string> OnMessageReceived;

        public abstract void Shutdown();

        public void Dispose() => Shutdown();
    }
}
