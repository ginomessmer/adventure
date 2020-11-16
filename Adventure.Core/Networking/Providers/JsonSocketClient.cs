using System;
using System.IO;
using System.Text.Json;
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
            var stream = new MemoryStream();
            var writer = new Utf8JsonWriter(stream);

            writer.WriteString("Hello", "world");

            throw new NotImplementedException();
        }

        public override event EventHandler<MessageReceivedArgs> OnMessageReceived;
        public override void SendInitialMessage()
        {
            throw new NotImplementedException();
        }
    }
}