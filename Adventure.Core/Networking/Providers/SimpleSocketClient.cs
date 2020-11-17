using Adventure.Core.Networking.Abstractions;
using Adventure.Core.Networking.Helpers;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Adventure.Core.Networking.Providers
{
    public sealed class SimpleSocketClient : SocketClient
    {
        private Socket _socket;

        /// <summary>
        /// Starts the client.
        /// </summary>
        public override void Start()
        {
            var builder = new SocketBuilder()
                .WithHostEntry(SocketDefaults.LoopbackAddress)
                .WithPort(SocketDefaults.Port);

            _socket = builder.Build();
            _socket.Connect(builder.Endpoint);
        }

        /// <summary>
        /// Sends a message to the designated server.
        /// </summary>
        /// <param name="message"></param>
        public override void SendMessage(string message)
        {
            var messageBuffer = Encoding.ASCII.GetBytes(message);
            var messageSize = messageBuffer.Length;

            var headerBuffer = new byte[SocketDefaults.HeaderSize];
            var header = new StringBuilder()
                .Append(SocketDefaults.LengthHeaderName)
                .Append(":")
                .Append(messageSize)
                .ToString();
            var headerSize = Encoding.ASCII.GetBytes(header,
                charIndex: 0, charCount: header.Length,
                bytes: headerBuffer, byteIndex: 0);

            var payload = new List<byte>();
            payload.AddRange(headerBuffer);
            payload.AddRange(messageBuffer);

            _socket.Send(payload.ToArray());
        }

        public override void SendInitialMessage() => SendMessage(SocketDefaults.HandshakeMessageContent);

        public override event EventHandler<SocketConnectionMessageReceivedArgs> OnMessageReceived;

        public override void Shutdown()
        {
            _socket.Shutdown(SocketShutdown.Both);
            _socket.Close();
            _socket.Dispose();
        }
    }
}