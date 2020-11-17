using Adventure.Core.Networking.Abstractions;
using Adventure.Core.Networking.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Adventure.Core.Networking.Providers
{
    public sealed class SimpleSocketClient : SocketClient
    {
        #region Properties
        /// <inheritdoc cref="SocketClient.ServerIPAddress"/>
        public override IPAddress ServerIPAddress { get; init; }

        /// <inheritdoc cref="SocketClient.ServerPort"/>
        public override int ServerPort { get; init; }

        /// <inheritdoc cref="SocketClient.ServerEndPoint"/>
        public override EndPoint ServerEndPoint => _socket.RemoteEndPoint;

        #endregion


        #region Fields

        /// <summary>
        /// The internal network socket.
        /// </summary>
        private Socket _socket;

        #endregion

        #region Events

        /// <summary>
        /// Fires as soon as the client connects to the server for the first time.
        /// </summary>
        public override event EventHandler OnConnected;

        #endregion

        #region Constructors

        // TODO: Should extract constructors to base class since they are used across all implementations anyway
        public SimpleSocketClient(IPAddress serverIPAddress, int serverPort)
        {
            if (serverIPAddress is null or default(IPAddress))
                throw new ArgumentNullException(nameof(serverIPAddress), "Remote endpoint cannot be left unspecified");

            if (serverPort is default(int) or >= IPEndPoint.MaxPort or < 1024)
                throw new ArgumentOutOfRangeException(nameof(serverPort), $"Port cannot be {serverPort}");


            ServerIPAddress = serverIPAddress;
            ServerPort = serverPort;
        }

        /// <summary>
        /// Creates a new client with default options.
        /// </summary>
        public SimpleSocketClient() : this(SocketDefaults.LoopbackAddress, SocketDefaults.Port)
        {
        }

        public SimpleSocketClient(string host, int serverPort) : this(Dns.GetHostEntry(host).AddressList.FirstOrDefault(), serverPort)
        {
        }

        #endregion

        /// <summary>
        /// Starts the client.
        /// </summary>
        public override void Start()
        {
            var builder = new SocketBuilder()
                .WithEndpoint(ServerIPAddress)
                .WithPort(ServerPort);

            _socket = builder.Build();
            _socket.Connect(builder.Endpoint);

            OnConnected?.Invoke(this, EventArgs.Empty);
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

        /// <summary>
        /// Shuts down the network connection.
        /// </summary>
        public override void Shutdown()
        {
            _socket.Shutdown(SocketShutdown.Both);
            _socket.Close();
            _socket.Dispose();
        }
    }
}