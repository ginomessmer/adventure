using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Adventure.Core.Networking;

namespace Adventure.Server
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            // Create socket
            Console.WriteLine("Opening socket...");
            var entry = await Dns.GetHostEntryAsync("127.0.0.1");
            var endpoint = new IPEndPoint(entry.AddressList.First(), SocketDefaults.Port);
            var socket = new Socket(endpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            // Bind
            Console.WriteLine("BIND");
            socket.Bind(endpoint);

            // Listen
            Console.WriteLine("LISTEN");
            socket.Listen(int.MaxValue);
            Console.WriteLine("Listening on {0}...", endpoint);

            var buffer = new byte[SocketDefaults.MessageSize];
            var data = "";

            // Receive
            while (true)
            {
                Console.WriteLine("ACCEPT...");
                var receiveSocket = socket.Accept();

                while (receiveSocket.Connected)
                {
                    try
                    {
                        receiveSocket.Receive(buffer);
                    }
                    catch (SocketException ex)
                    {
                        Console.WriteLine("Socket exception: {0}", ex);
                        break;
                    }

                    data += Encoding.ASCII.GetString(buffer);

                    // Get header length value
                    var headerIndex = data.IndexOf(SocketDefaults.LengthHeaderName, StringComparison.Ordinal);
                    if (headerIndex > -1)
                    {
                        var header = data.Substring(headerIndex, SocketDefaults.HeaderSize);

                        // Split or regex
                        var headerKeyValue = header.Split(':');
                        var length = Convert.ToInt32(headerKeyValue[1]);

                        Console.WriteLine("Received new message");
                        Console.WriteLine("- Length: {0}", length);

                        var payload = data.Substring(headerIndex + SocketDefaults.HeaderSize, length);

                        Console.WriteLine("- Payload: {0}", payload);

                        receiveSocket.Send(Encoding.ASCII.GetBytes("Great success"));
                    }
                }
            }
        }
    }
}
