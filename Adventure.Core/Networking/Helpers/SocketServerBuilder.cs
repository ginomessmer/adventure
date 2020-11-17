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

            return WithStartingHandler(handlers.Starting)
                .WithStartedHandler(handlers.Started)
                .WithMessageReceivedHandler(handlers.MessageReceived)
                .WithClientDisconnectedHandler(handlers.ClientDisconnected);
        }

        public SocketServerBuilder<T> WithStartingHandler(Action handler)
        {
            _server.OnServerStarting += (sender, args) => handler?.Invoke();
            return this;
        }

        public SocketServerBuilder<T> WithStartedHandler(Action handler)
        {
            _server.OnServerStarted += (sender, args) => handler?.Invoke();
            return this;
        }

        public SocketServerBuilder<T> WithMessageReceivedHandler(Action<SocketConnectionMessageReceivedArgs> handler)
        {
            _server.OnMessageReceived += (sender, args) => handler?.Invoke(args);
            return this;
        }

        public SocketServerBuilder<T> WithClientDisconnectedHandler(Action<SocketConnectionClientDisconnectedArgs> handler)
        {
            _server.OnClientDisconnected += (sender, args) => handler?.Invoke(args);
            return this;
        }

        public T Build() => _server;
    }
}