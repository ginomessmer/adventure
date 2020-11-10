using System;
using System.Collections.Generic;
using System.Text;

namespace Adventure.Core.Networking
{
    public abstract class SocketClient
    {
        public abstract void Start();

        public abstract void SendMessage(string message);

        public abstract void SendInitialMessage();

        public abstract EventHandler<string> OnMessageReceived();

        public abstract void Shutdown();
    }
}
