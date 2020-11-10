using System;

namespace Adventure.Core.Networking.Abstractions
{
    public interface ISocketAgent
    {
        void Start();

        void Shutdown();

        void SendMessage(string message);

        event EventHandler<MessageReceivedArgs> OnMessageReceived;
    }
}