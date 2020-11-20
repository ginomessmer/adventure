using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Adventure.Core.Networking.Abstractions;
using Adventure.Core.Networking.Abstractions.Events;
using Adventure.Core.Networking.Helpers;

namespace Adventure.Core.Networking
{
    public abstract class SocketClient : ISocketAgent, IDisposable
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

        /// <summary>
        /// The remote server's endpoint.
        /// </summary>
        public EndPoint ServerEndPoint => ServerSocket.RemoteEndPoint;

        #endregion


        #region Fields

        /// <summary>
        /// The internal network socket.
        /// </summary>
        protected Socket ServerSocket;

        #endregion


        #region Events

        public event EventHandler OnConnected;

        public event EventHandler<SocketConnectionServerMessageReceivedArgs> OnMessageReceived;

        #endregion


        #region Constructors

        protected SocketClient(IPAddress serverIPAddress, int serverPort)
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
        protected SocketClient() : this(SocketDefaults.LoopbackAddress, SocketDefaults.Port)
        {
        }

        protected SocketClient(string host, int serverPort) : this(Dns.GetHostEntry(host).AddressList.FirstOrDefault(), serverPort)
        {
        }

        #endregion


        /// <summary>
        /// Starts the client.
        /// </summary>
        public virtual void Start()
        {
            var builder = new SocketBuilder()
                .WithEndpoint(ServerIPAddress)
                .WithPort(ServerPort);

            ServerSocket = builder.Build();
            ServerSocket.Connect(builder.Endpoint);

            SendInitialMessage();

            OnConnected?.Invoke(this, EventArgs.Empty);

            while (true)
            {
                var messageLengthBuffer = new byte[4];
                
                var receivedBytes = ServerSocket.Receive(messageLengthBuffer);
                var messageLength = BitConverter.ToInt32(messageLengthBuffer);

                if (receivedBytes is not 4)
                {
                    return;
                }

                var messageBuffer = new byte[messageLength];
                receivedBytes = ServerSocket.Receive(messageBuffer);
                var data = Encoding.ASCII.GetString(messageBuffer, 0, messageLength);

                OnMessageReceived?.Invoke(this, new SocketConnectionServerMessageReceivedArgs(data));
            }
        }

        /// <summary>
        /// Sends a message to the designated server.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="message"></param>
        public virtual void SendMessage(string message)
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

            ServerSocket.Send(payload.ToArray());
        }

        public virtual void SendInitialMessage() => SendMessage(SocketDefaults.HandshakeMessageContent);

        /// <summary>
        /// Shuts down the network connection.
        /// </summary>
        public virtual void Shutdown()
        {
            ServerSocket.Shutdown(SocketShutdown.Both);
            ServerSocket.Close();
            ServerSocket.Dispose();
        }

        public void Dispose() => Shutdown();
    }
}
