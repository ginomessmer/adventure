using System;
using System.Net.Sockets;
using System.Text;

namespace Adventure.Core.Networking
{
    public sealed class SimpleSocketServer : SocketServer
    {
        private Socket _socket;
        private byte[] _buffer;
        private string _data;

        public override void Start()
        {
            var builder = new SocketBuilder()
                .WithHostEntry(SocketDefaults.LoopbackAddress)
                .WithPort(SocketDefaults.Port);

            _socket = builder.Build();
            _socket.Bind(builder.Endpoint);
            _socket.Listen(int.MaxValue);

            _buffer = new byte[SocketDefaults.MessageSize];
            _data = "";

            while (true)
            {
                var receiveSocket = _socket.Accept();

                while (receiveSocket.Connected)
                {
                    try
                    {
                        receiveSocket.Receive(_buffer);
                    }
                    catch (SocketException ex)
                    {
                        Console.WriteLine("Socket exception: {0}", ex);
                        break;
                    }

                    _data += Encoding.ASCII.GetString(_buffer);

                    // Get header length value
                    var headerIndex = _data.IndexOf(SocketDefaults.LengthHeaderName, StringComparison.Ordinal);
                    if (headerIndex > -1)
                    {
                        var header = _data.Substring(headerIndex, SocketDefaults.HeaderSize);

                        // Split or regex
                        var headerKeyValue = header.Split(':');
                        var length = Convert.ToInt32(headerKeyValue[1]);

                        Console.WriteLine("Received new message");
                        Console.WriteLine("- Length: {0}", length);

                        var payload = _data.Substring(headerIndex + SocketDefaults.HeaderSize, length);

                        Console.WriteLine("- Payload: {0}", payload);

                        receiveSocket.Send(Encoding.ASCII.GetBytes("Great success"));
                    }
                }

                _data = string.Empty;
            }
        }

        public override void Shutdown()
        {
            throw new NotImplementedException();
        }

        public override void SendMessage(string message)
        {
            throw new NotImplementedException();
        }

        public override event EventHandler<string> OnMessageReceived;
    }
}