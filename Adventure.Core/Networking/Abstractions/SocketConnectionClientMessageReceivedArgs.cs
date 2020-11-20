namespace Adventure.Core.Networking.Abstractions
{
    public record SocketConnectionClientMessageReceivedArgs(string Message, SocketClientConnection ClientConnection);

    public record SocketConnectionServerMessageReceivedArgs(string Message);

    public record SocketConnectionClientDisconnectedArgs(SocketClientConnection ClientConnection);

    public record SocketConnectionClientConnectedArgs(SocketClientConnection ClientConnection);
}