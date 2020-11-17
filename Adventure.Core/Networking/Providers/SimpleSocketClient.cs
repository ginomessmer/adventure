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

        /// <summary>
        /// The remote server's IP address.
        /// </summary>
        public IPAddress ServerIPAddress { get; init; }

        /// <summary>
        /// The remote server's port.
        /// </summary>
        public int ServerPort { get; init; }

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

        public SimpleSocketClient() : this(SocketDefaults.LoopbackAddress, SocketDefaults.Port)
        {
        }

        public SimpleSocketClient(IPAddress serverIpAddress, int serverPort)
        {
            ServerIPAddress = serverIpAddress;
            ServerPort = serverPort;
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
                .WithPort(SocketDefaults.Port);

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