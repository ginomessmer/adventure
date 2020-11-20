using System.Threading;
using System.Threading.Tasks;
using Adventure.Core.Networking.Providers;
using Microsoft.Extensions.Hosting;

namespace Adventure.Server
{
    public class AdventureGameHostedService : IHostedService
    {
        private readonly JsonSocketServer _server;

        public AdventureGameHostedService(JsonSocketServer server)
        {
            _server = server;
        }

        public Task StartAsync(CancellationToken cancellationToken) => _server.RunAsync();

        public Task StopAsync(CancellationToken cancellationToken) => Task.Run(_server.Shutdown, cancellationToken);
    }
}