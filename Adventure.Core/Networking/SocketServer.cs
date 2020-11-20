using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Adventure.Core.Networking.Abstractions;
using Adventure.Core.Networking.Helpers;

namespace Adventure.Core.Networking
{
    public abstract class SocketServer : IAsyncSocketAgent
    {
        #region Properties

        private readonly ICollection<SocketClientConnection> _connections = new List<SocketClientConnection>();
        public IReadOnlyCollection<SocketClientConnection> Connections => _connections.ToList();

        #endregion


        #region Fields

        protected Socket _socket;

        #endregion


        #region Events

        public event EventHandler OnServerStarting;
        public event EventHandler OnServerStarted;
        public event EventHandler<SocketConnectionClientMessageReceivedArgs> OnMessageReceived;
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

                var connection = new SocketClientConnection(receiveSocket, this);

                if (OnMessageReceived is not null)
                    connection.OnMessageReceived += (sender, args) => OnMessageReceived(sender, new SocketConnectionClientMessageReceivedArgs(args.Message, connection));

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
            _connections.Remove(e.ClientConnection);
            OnClientDisconnected?.Invoke(this, e);
        }

        public virtual void SendMessage(string message, SocketClientConnection clientConnection) =>
            SendMessage(message, clientConnection.ClientSocket);

        public virtual void SendMessage(string message, Socket socket)
        {
            //var responseBuffer = new byte[SocketDefaults.MessageSize];
            var nessage = Encoding.ASCII.GetBytes(message);
            var messageLength = BitConverter.GetBytes(message.Length);

            socket.Send(messageLength);
            socket.Send(nessage);
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