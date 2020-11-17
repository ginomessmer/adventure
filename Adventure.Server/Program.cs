using Adventure.Core.Networking.Providers;
using System;
using System.Threading;
using System.Threading.Tasks;
using Adventure.Core.Networking.Helpers;

namespace Adventure.Server
{
    public class Program
    {
        public static Task Main(string[] args) => new SocketServerBuilder<SimpleSocketServer>()
            .WithHandlers(handlers =>
            {
                handlers.Starting = () => Console.WriteLine("Server is starting...");
                handlers.Started = () => Console.WriteLine("Server started"); 
                handlers.MessageReceived = args => Console.WriteLine("Message received from client {0} ({1}): {2}",
                    args.Connection.Id, args.Connection.ClientEndpoint, args.Message);
                handlers.ClientDisconnected = args => Console.WriteLine("Client {0} disconnected from server", args.Connection.Id);
            })
            .Build()
            .RunAsync();
    }
}
