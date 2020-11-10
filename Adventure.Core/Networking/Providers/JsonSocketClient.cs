using System;
using Adventure.Core.Networking.Abstractions;

namespace Adventure.Core.Networking.Providers
{
    public sealed class JsonSocketClient : SocketClient
    {
        public override void Start()
        {
            throw new NotImplementedException();
        }

        public override void Shutdown()
        {
            throw new NotImplementedException();
        }

        public override void SendMessage(string message)
        {
            throw new NotImplementedException();
        }

        public override event EventHandler<MessageReceivedArgs> OnMessageReceived;
        public override void SendInitialMessage()
        {
            throw new NotImplementedException();
        }
    }
}