using System.Threading;
using System.Threading.Tasks;
using Adventure.Core.Game;
using Adventure.Core.Networking.Providers;
using Microsoft.Extensions.Hosting;

namespace Adventure.Server
{
    public class AdventureGameHostedService : IHostedService
    {
        private readonly AdventureGameSocketServer _server;

        public AdventureGameHostedService(AdventureGameSocketServer server)
        {
            _server = server;
        }

        public Task StartAsync(CancellationToken cancellationToken) => _server.RunAsync();

        public Task StopAsync(CancellationToken cancellationToken) => Task.Run(_server.Shutdown, cancellationToken);
    }
}