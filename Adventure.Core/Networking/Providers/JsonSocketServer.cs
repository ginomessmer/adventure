using Adventure.Core.Networking.Abstractions;
using System;

namespace Adventure.Core.Networking.Providers
{
    public sealed class JsonSocketServer : SocketServer
    {
        public override void Start()
        {
            throw new NotImplementedException();
        }


        public override event EventHandler OnServerStarting;
        public override event EventHandler OnServerStarted;
        public override event EventHandler<SocketConnectionClientDisconnectedArgs> OnClientDisconnected;
        public override event EventHandler<SocketConnectionMessageReceivedArgs> OnMessageReceived;

        public override void Shutdown()
        {
            throw new NotImplementedException();
        }

        public override void SendMessage(string message)
        {
            throw new NotImplementedException();
        }
    }
}