using Adventure.Core.Networking.Helpers;
using Adventure.Core.Networking.Providers;
using System;
using System.Threading.Tasks;

namespace Adventure.Server
{
    public class Program
    {
        /// <summary>
        /// A very simple implementation for a socket server built for testing purposes.
        /// </summary>
        public static readonly SimpleSocketServer SimpleSocketServer = new SocketServerBuilder<SimpleSocketServer>()
            .WithHandlers(handlers =>
            {
                handlers.Starting = () => Console.WriteLine("Server is starting...");
                handlers.Started = () => Console.WriteLine("Server started");
                handlers.MessageReceived = args => Console.WriteLine("Message received from client {0} ({1}): {2}",
                    args.ClientConnection.Id, args.ClientConnection.ClientEndpoint, args.Message);
                handlers.ClientDisconnected = args =>
                    Console.WriteLine("Client {0} disconnected from server", args.ClientConnection.Id);
            })
            .Build();

        /// <summary>
        /// Use this.
        /// </summary>
        public static readonly JsonSocketServer JsonSocketServer = new SocketServerBuilder<JsonSocketServer>()
            .Build();

        public static Task Main(string[] args) => JsonSocketServer.RunAsync();
    }
}
