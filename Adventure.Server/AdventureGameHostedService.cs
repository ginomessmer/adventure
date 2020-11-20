using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

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