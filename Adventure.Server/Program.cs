using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Adventure.Server
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            // Create socket
            Console.WriteLine("Opening socket...");
            var entry = await Dns.GetHostEntryAsync("127.0.0.1");
            var endpoint = new IPEndPoint(entry.AddressList.First(), 14500);
            var socket = new Socket(endpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            // Bind
            Console.WriteLine("BIND");
            socket.Bind(endpoint);

            // Listen
            Console.WriteLine("LISTEN");
            socket.Listen(int.MaxValue);

            // Accept
            Console.WriteLine("ACCEPT");
            var receiveSocket = socket.Accept();

            var buffer = new byte[1024];
            var data = "";

            // Receive
            Console.WriteLine("RECEIVE...");
            while (true)
            {
                receiveSocket.Receive(buffer);
                data += Encoding.ASCII.GetString(buffer);

                if (data.IndexOf("<EOF>", StringComparison.Ordinal) > -1)
                {
                    receiveSocket.Send(Encoding.ASCII.GetBytes("Great success"));
                    break;
                }
            }

            Console.WriteLine("Data received:\n{0}", data);

            Console.WriteLine("Press any key to exit...");
            Console.Read();
        }
    }
}
