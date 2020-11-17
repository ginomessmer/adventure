namespace Adventure.Core.Networking.Abstractions
{
    public class ServerConnectionMessageReceivedArgs
    {
        public string Message { get; init; }

        public ServerConnectionMessageReceivedArgs(string message)
        {
            Message = message;
        }
    }
}