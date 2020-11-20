namespace Adventure.Core.Networking.Abstractions
{
    public record SocketConnectionClientMessageReceivedArgs(string Message, SocketConnection Connection);

    public record SocketConnectionServerMessageReceivedArgs(string Message);

    public record SocketConnectionClientDisconnectedArgs(SocketConnection Connection);
}