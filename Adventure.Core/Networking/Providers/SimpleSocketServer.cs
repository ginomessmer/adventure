using Adventure.Core.Networking.Abstractions;
using Adventure.Core.Networking.Helpers;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;

namespace Adventure.Core.Networking.Providers
{
    public sealed class SimpleSocketServer : SocketServer
    {
        private Socket _socket;

        private readonly IList<SocketConnection> _connections = new List<SocketConnection>();

        public override void Start()
        {
            var builder = new SocketBuilder()
                .WithHostEntry(SocketDefaults.LoopbackAddress)
                .WithPort(SocketDefaults.Port);

            _socket = builder.Build();
            _socket.Bind(builder.Endpoint);
            _socket.Listen(int.MaxValue);

            while (true)
            {
                var receiveSocket = _socket.Accept();
                receiveSocket.ReceiveTimeout = SocketDefaults.ReceiveTimeout;

                var connection = new SocketConnection(receiveSocket, this);

                if (OnMessageReceived is not null)
                    connection.OnMessageReceived += (sender, args) => OnMessageReceived(sender, new SocketMessageReceivedArgs(args.Message, connection));

                _connections.Add(connection);
            }
        }

        public override void Shutdown()
        {
            _socket.Shutdown(SocketShutdown.Both);
            _socket.Close();
            _socket.Dispose();
        }

        public override void SendMessage(string message)
        {
            throw new NotImplementedException();
        }

        public override event EventHandler<SocketMessageReceivedArgs> OnMessageReceived;
    }
}