using System;

namespace Adventure.Core.Networking
{
    public interface ISocketAgent
    {
        void Start();

        void Shutdown();

        void SendMessage(string message);

        event EventHandler<string> OnMessageReceived;
    }
}