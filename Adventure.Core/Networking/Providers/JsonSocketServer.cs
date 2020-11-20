using Adventure.Core.Commands.Abstractions;
using Adventure.Core.Networking.Abstractions;
using Newtonsoft.Json;
using System.Net.Sockets;

namespace Adventure.Core.Networking.Providers
{
    public sealed class JsonSocketServer : SocketServer, ICommandSender
    {
        public JsonSocketServer()
        {
            OnMessageReceived += HandleServerCommandMessage;
        }

        private void HandleServerCommandMessage(object? sender, SocketConnectionClientMessageReceivedArgs e)
        {
            var command = JsonConvert.DeserializeObject<ICommand>(e.Message, JsonSocketDefaults.JsonSerializerSettings);
            command?.ExecuteServer(this, e.ClientConnection.ClientSocket);
        }

        public void SendCommand(ICommand command, Socket receiver)
        {
            var json = JsonConvert.SerializeObject(command, JsonSocketDefaults.JsonSerializerSettings);
            SendMessage(json, receiver);
        }
    }
}