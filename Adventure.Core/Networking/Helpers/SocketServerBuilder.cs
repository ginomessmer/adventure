using System;
using Adventure.Core.Networking.Abstractions;

namespace Adventure.Core.Networking.Helpers
{
    public class SocketServerBuilder<T> where T : SocketServer, new()
    {
        public class ServerEventHandlers
        {
            public Action Starting { get; set; }

            public Action Started { get; set; }

            public Action<SocketConnectionMessageReceivedArgs> MessageReceived { get; set; }

            public Action<SocketConnectionClientDisconnectedArgs> ClientDisconnected { get; set; }
        }

        private T _server = new();

        public SocketServerBuilder<T> WithHandlers(Action<ServerEventHandlers> configure)
        {
            var handlers = new ServerEventHandlers();
            configure.Invoke(handlers);

            _server.OnServerStarting += (sender, args) => handlers.Starting?.Invoke();
            _server.OnServerStarted += (sender, args) => handlers.Started?.Invoke();
            _server.OnMessageReceived += (sender, args) => handlers.MessageReceived?.Invoke(args);
            _server.OnClientDisconnected += (sender, args) => handlers.ClientDisconnected?.Invoke(args);

            return this;
        }

        public T Build() => _server;
    }
}