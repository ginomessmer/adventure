using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Adventure.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            // Create socket
            Console.WriteLine("Connecting to socket...");
            var entry = await Dns.GetHostEntryAsync("127.0.0.1");
            var endpoint = new IPEndPoint(entry.AddressList.First(), 14500);
            var socket = new Socket(endpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            // Connect
            Console.WriteLine("CONNECT");
            await socket.ConnectAsync(endpoint);

            // Send
            Console.WriteLine("SEND");
            socket.Send(Encoding.ASCII.GetBytes("Hello world <EOF>"));

            // Receive
            var buffer = new byte[1024];
            socket.Receive(buffer);
            
            Console.WriteLine("Reply: {0}", Encoding.ASCII.GetString(buffer));
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
            socket.Dispose();

            Console.WriteLine("Press any key to exit...");
            Console.Read();
        }
    }
}
