using Adventure.Core.Networking.Abstractions;
using System;
using System.IO;
using System.Net;
using System.Text.Json;

namespace Adventure.Core.Networking.Providers
{
    public sealed class JsonSocketClient : SocketClient
    {
        public override event EventHandler<SocketConnectionMessageReceivedArgs> OnMessageReceived;

        public override event EventHandler OnConnected;

        // TODO
        public override IPAddress ServerIPAddress { get; init; }
        public override int ServerPort { get; init; }
        public override EndPoint ServerEndPoint { get; }

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
            var stream = new MemoryStream();
            var writer = new Utf8JsonWriter(stream);

            writer.WriteString("Hello", "world");

            throw new NotImplementedException();
        }
        public override void SendInitialMessage()
        {
            throw new NotImplementedException();
        }
    }
}