using Adventure.Core.Networking.Helpers;
using Adventure.Core.Networking.Providers;
using System;
using System.Threading.Tasks;
using Adventure.Core.Game;
using Adventure.Core.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Adventure.Server
{
    public class Program
    {
        #region Testing

        /// <summary>
        /// A very simple implementation for a socket server built for testing purposes.
        /// </summary>
        [Obsolete("Only for testing purposes")]
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

        #endregion

        public static Task Main(string[] args) => Host.CreateDefaultBuilder(args)
            .ConfigureServices(services =>
            {
                services.AddSingleton<AdventureGameSocketServer>();
                services.AddSingleton<IGameRepository, InMemoryGameRepository>();

                services.AddHostedService<AdventureGameHostedService>();
            })
            .Build()
            .RunAsync();
    }
}
