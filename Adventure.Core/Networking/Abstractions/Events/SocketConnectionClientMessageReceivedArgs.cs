namespace Adventure.Core.Networking.Abstractions.Events
{
    public record SocketConnectionClientMessageReceivedArgs(string Message, SocketClientConnection ClientConnection);
}