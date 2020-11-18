using Adventure.Core.Networking.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Adventure.Core.Networking.Abstractions
{
    public abstract class SocketServer : IAsyncSocketAgent
    {
        #region Properties

        private readonly ICollection<SocketConnection> _connections = new List<SocketConnection>();
        public IReadOnlyCollection<SocketConnection> Connections => _connections.ToList();

        #endregion


        #region Fields

        protected Socket _socket;

        #endregion


        #region Events

        public event EventHandler OnServerStarting;
        public event EventHandler OnServerStarted;
        public event EventHandler<SocketConnectionMessageReceivedArgs> OnMessageReceived;
        public event EventHandler<SocketConnectionClientDisconnectedArgs> OnClientDisconnected;

        #endregion

        /// <summary>
        /// <inheritdoc cref="SocketServer.Start"/>. This method starts in a dedicated task.
        /// </summary>
        /// <returns></returns>
        public Task RunAsync() => Task.Run(Start);


        /// <summary>
        /// Starts the server and accepts new incoming requests. All new requests are added to a managed connections collection.
        /// </summary>
        public virtual void Start()
        {
            OnServerStarting?.Invoke(this, EventArgs.Empty);

            var builder = new SocketBuilder()
                .WithHostEntry(SocketDefaults.LoopbackAddress)
                .WithPort(SocketDefaults.Port);

            _socket = builder.Build();
            _socket.Bind(builder.Endpoint);
            _socket.Listen(int.MaxValue);

            OnServerStarted?.Invoke(this, EventArgs.Empty);

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

        public virtual void SendMessage(Socket client, string message)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Stops the server.
        /// </summary>
        public void Shutdown()
        {
            _socket.Shutdown(SocketShutdown.Both);
            _socket.Close();
            _socket.Dispose();
        }
    }
}