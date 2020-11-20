using Adventure.Core.Networking.Abstractions;

namespace Adventure.Server.Options
{
    public class AdventureServerOptions
    {
        public string Host { get; set; } = SocketDefaults.LoopbackAddress;

        public int Port { get; set; } = SocketDefaults.Port;
    }
}
