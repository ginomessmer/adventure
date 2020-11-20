using System;
using Adventure.Core.Commands;
using Adventure.Core.Commands.Abstractions;
using Adventure.Core.Networking.Abstractions;
using Newtonsoft.Json;
using System.Net.Sockets;
using Adventure.Core.Networking.Abstractions.Events;

namespace Adventure.Core.Networking.Providers
{
    public sealed class JsonSocketClient : SocketClient, ICommandSender
    {
        public JsonSocketClient()
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