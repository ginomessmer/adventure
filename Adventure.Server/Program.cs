using Adventure.Core.Networking.Providers;
using System;
using System.Threading.Tasks;

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
