using Adventure.Core.Networking.Abstractions;
using Adventure.Core.Networking.Helpers;
using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace Adventure.Core.Networking.Providers
{
    /// <summary>
    /// What the name implies... a simple socket server.
    /// </summary>
    public sealed class SimpleSocketServer : SocketServer
    {
        private Socket _socket;

        private readonly ICollection<SocketConnection> _connections = new List<SocketConnection>();

        /// <summary>
        /// Starts the server and accepts new incoming requests. All new requests are added to a managed connections collection.
        /// </summary>
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
                    connection.OnMessageReceived += (sender, args) => OnMessageReceived(sender, new SocketConnectionMessageReceivedArgs(args.Message, connection));

                if (OnClientDisconnected is not null)
                    connection.OnDisconnected += HandleOnDisconnected;

                _connections.Add(connection);
            }
        }

        /// <summary>
        /// A clean up method that removes a connection after it's gone.
        /// </summary>
        private void HandleOnDisconnected(object? sender, SocketConnectionClientDisconnectedArgs e)
        {
            _connections.Remove(e.Connection);
            OnClientDisconnected?.Invoke(this, e);
        }

        /// <summary>
        /// Stops the server.
        /// </summary>
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

        public override event EventHandler<SocketConnectionMessageReceivedArgs> OnMessageReceived;
        public event EventHandler<SocketConnectionClientDisconnectedArgs> OnClientDisconnected;
    }
}