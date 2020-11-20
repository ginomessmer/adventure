using Adventure.Core.Networking.Abstractions.Events;
using System;

namespace Adventure.Core.Networking.Helpers
{
    public class SocketServerBuilder<T> where T : SocketServer, new()
    {
        public class ServerEventHandlers
        {
            public Action Starting { get; set; }

            public Action Started { get; set; }

            public Action<SocketConnectionClientMessageReceivedArgs> MessageReceived { get; set; }

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
            _server.ServerStarting += (sender, args) => handler?.Invoke();
            return this;
        }

        public SocketServerBuilder<T> WithStartedHandler(Action handler)
        {
            _server.ServerStarted += (sender, args) => handler?.Invoke();
            return this;
        }

        public SocketServerBuilder<T> WithMessageReceivedHandler(Action<SocketConnectionClientMessageReceivedArgs> handler)
        {
            _server.MessageReceived += (sender, args) => handler?.Invoke(args);
            return this;
        }

        public SocketServerBuilder<T> WithClientDisconnectedHandler(Action<SocketConnectionClientDisconnectedArgs> handler)
        {
            _server.ClientDisconnected += (sender, args) => handler?.Invoke(args);
            return this;
        }

        public T Build() => _server;
    }
}