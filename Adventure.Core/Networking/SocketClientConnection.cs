using Adventure.Core.Networking.Abstractions;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Adventure.Core.Networking.Abstractions.Events;

namespace Adventure.Core.Networking
{
    /// <summary>
    /// A single connection that handles incoming messages.
    /// </summary>
    public class SocketClientConnection : IDisposable
    {
        #region Properties

        /// <summary>
        /// The connection's ID.
        /// </summary>
        public string Id { get; init; }

        /// <summary>
        /// The server that hosts this connection.
        /// </summary>
        public SocketServer Server { get; init; }

        #region Client Properties

        /// <summary>
        /// The network socket that the client is connected to.
        /// </summary>
        public Socket ClientSocket { get; init; }

        /// <summary>
        /// Gets the remote client's endpoint.
        /// </summary>
        public EndPoint ClientEndpoint => ClientSocket.RemoteEndPoint;

        #endregion

        #endregion


        #region Fields

        /// <summary>
        /// The dedicated thread where the connection receives messages.
        /// </summary>
        private Thread _thread;

        /// <summary>
        /// The receive buffer.
        /// </summary>
        private readonly byte[] _buffer = new byte[SocketDefaults.MessageSize];

        /// <summary>
        /// The data this connection has received so far.
        /// </summary>
        private string _data = string.Empty;

        #endregion


        #region Event Handlers

        /// <summary>
        /// Fires when a new message is received on this connection.
        /// </summary>
        public event EventHandler<ServerConnectionMessageReceivedArgs> OnMessageReceived;

        /// <summary>
        /// Fires when the client disconnects from the connection.
        /// </summary>
        public event EventHandler<SocketConnectionClientDisconnectedArgs> OnDisconnected;

        #endregion


        /// <summary>
        /// <inheritdoc cref="SocketClientConnection(System.Net.Sockets.Socket,Adventure.Core.Networking.SocketServer,string)"/>
        /// </summary>
        /// <param name="clientSocket"></param>
        /// <param name="server"></param>
        public SocketClientConnection(Socket clientSocket, SocketServer server) : this(clientSocket, server, Guid.NewGuid().ToString())
        {
        }

        /// <summary>
        /// Creates a new connection and listens to messages on a dedicated thread.
        /// </summary>
        /// <param name="clientSocket"></param>
        /// <param name="server"></param>
        /// <param name="id"></param>
        public SocketClientConnection(Socket clientSocket, SocketServer server, string id)
        {
            ClientSocket = clientSocket;
            Server = server;
            Id = id;

            _thread = new Thread(HandleConnection);
            _thread.Start();
        }

        /// <summary>
        /// Handles the connection by listening to the receive socket, puts the received data into the buffer and data string.
        /// </summary>
        private void HandleConnection()
        {
            try
            {
                while (ClientSocket.Connected)
                {
                    ClientSocket.Receive(_buffer);

                    _data += Encoding.ASCII.GetString(_buffer);

                    // Get header length value
                    var headerIndex = _data.IndexOf(SocketDefaults.LengthHeaderName, StringComparison.Ordinal);
                    if (headerIndex > -1)
                    {
                        var header = _data.Substring(headerIndex, SocketDefaults.HeaderSize);

                        // Split or regex
                        var headerKeyValue = header.Split(':');

                        var length = Convert.ToInt32(headerKeyValue[1]);
                        var message = _data.Substring(headerIndex + SocketDefaults.HeaderSize, length);

                        OnMessageReceived?.Invoke(this, new ServerConnectionMessageReceivedArgs(message));
                    }
                }
            }
            catch (SocketException ex)
            {
                switch (ex.SocketErrorCode)
                {
                    case SocketError.ConnectionReset:
                        OnDisconnected?.Invoke(this, new SocketConnectionClientDisconnectedArgs(this));
                        break;
                    default:
                        throw;
                }
            }
            finally
            {
                _data = string.Empty;
            }
        }

        public void Dispose()
        {
            ClientSocket?.Dispose();
        }
    }
}
