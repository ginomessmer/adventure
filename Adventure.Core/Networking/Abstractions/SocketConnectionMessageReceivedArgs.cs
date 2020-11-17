namespace Adventure.Core.Networking.Abstractions
{
    public class SocketConnectionMessageReceivedArgs
    {
        public string Message { get; }

        public SocketConnection Connection { get; }

        public SocketConnectionMessageReceivedArgs(string message, SocketConnection connection)
        {
            Message = message;
            Connection = connection;
        }
    }

    public class SocketConnectionClientDisconnectedArgs
    {
        public SocketConnection Connection { get; }

        public SocketConnectionClientDisconnectedArgs(SocketConnection connection)
        {
            Connection = connection;
        }
    }
}