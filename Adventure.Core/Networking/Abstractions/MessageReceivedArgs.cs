namespace Adventure.Core.Networking.Abstractions
{
    public class MessageReceivedArgs
    {
        public string Message { get; }

        public MessageReceivedArgs(string message)
        {
            Message = message;
        }
    }
}