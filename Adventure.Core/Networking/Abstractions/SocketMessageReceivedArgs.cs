namespace Adventure.Core.Networking.Abstractions
{
    public class SocketMessageReceivedArgs
    {
        public string Message { get; }
        public SocketConnection Connection { get; }

        public SocketMessageReceivedArgs(string message, SocketConnection connection)
        {
            Message = message;
            Connection = connection;
        }
    }
}