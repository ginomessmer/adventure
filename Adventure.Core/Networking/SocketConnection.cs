﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Adventure.Core.Networking.Abstractions;

namespace Adventure.Core.Networking
{
    public class SocketConnection
    {
        public string Id { get; init; }

        public Socket ClientSocket { get; init; }

        public SocketServer Server { get; init; }

        private Thread _thread;

        private readonly byte[] _buffer = new byte[SocketDefaults.MessageSize];
        private string _data = string.Empty;


        public event EventHandler<ServerConnectionMessageReceivedArgs> OnMessageReceived; 

        public SocketConnection(Socket clientSocket, SocketServer server) : this(clientSocket, server, Guid.NewGuid().ToString())
        {
        }

        public SocketConnection(Socket clientSocket, SocketServer server, string id)
        {
            ClientSocket = clientSocket;
            Server = server;
            Id = id;

            _thread = new Thread(HandleConnection);
            _thread.Start();
        }

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
                // TODO: Handle disconnected client
            }
            finally
            {
                _data = string.Empty;
            }
        }
    }
}