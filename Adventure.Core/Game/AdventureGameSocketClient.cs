using System.Net.Sockets;
using Adventure.Core.Commands;
using Adventure.Core.Commands.Abstractions;
using Adventure.Core.Networking;
using Adventure.Core.Networking.Abstractions;
using Adventure.Core.Networking.Abstractions.Events;
using Newtonsoft.Json;

namespace Adventure.Core.Game
{
    public sealed class AdventureGameSocketClient : SocketClient, ICommandSender
    {
        public AdventureGameSocketClient()
        {
            OnMessageReceived += HandleClientCommandMessage;
        }

        public override void SendInitialMessage() => SendCommand(new ClientConnectedCommand(), null);

        private void HandleClientCommandMessage(object? sender, SocketConnectionServerMessageReceivedArgs e)
        {
            var command = JsonConvert.DeserializeObject<ICommand>(e.Message, JsonSocketDefaults.JsonSerializerSettings);
            command?.ExecuteClient(this, ServerSocket);
        }

        public void SendCommand(ICommand command, Socket receiver)
        {
            // TODO: better way to ignore receiver parameter

            var json = JsonConvert.SerializeObject(command, JsonSocketDefaults.JsonSerializerSettings);
            SendMessage(json);
        }
    }
}