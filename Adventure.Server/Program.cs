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
            Console.WriteLine("Starting server");

            var server = new SimpleSocketServer();
            server.OnMessageReceived += (sender, receivedArgs) =>
            {
                Console.WriteLine("Message received: {0}", receivedArgs.Message);
            };

            server.Start();
        }
    }
}
