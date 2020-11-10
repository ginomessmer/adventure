using System;

namespace Adventure.Core.Networking
{
    public sealed class SimpleSocketClient : SocketClient
    {
        public override void Start()
        {
            throw new NotImplementedException();
        }

        public override void SendMessage(string message)
        {
            throw new NotImplementedException();
        }

        public override void SendInitialMessage() => throw new NotImplementedException();

        public override EventHandler<string> OnMessageReceived()
        {
            throw new NotImplementedException();
        }

        public override void Shutdown()
        {
            throw new NotImplementedException();
        }
    }
}