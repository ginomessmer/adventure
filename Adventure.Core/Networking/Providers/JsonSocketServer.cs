using System;
using System.Linq;
using Adventure.Core.Commands.Abstractions;
using Adventure.Core.Networking.Abstractions;
using Newtonsoft.Json;
using System.Net.Sockets;
using System.Threading.Tasks;
using Adventure.Core.Commands;
using Adventure.Core.Domain;
using Adventure.Core.Infrastructure;
using Microsoft.Extensions.Logging;

namespace Adventure.Core.Networking.Providers
{
    public sealed class JsonSocketServer : SocketServer, ICommandSender
    {
        private readonly IGameRepository _gameRepository;
        private readonly ILogger<JsonSocketServer> _logger;

        public JsonSocketServer(IGameRepository gameRepository, ILogger<JsonSocketServer> logger)
        {
            _gameRepository = gameRepository;
            _logger = logger;

            MessageReceived += async (_, args) => await HandleServerCommandMessageAsync(args.Message, args.ClientConnection);

            ClientConnected += (_, args) => _logger.LogInformation("Client {ClientId} {ClientEndpoint} connected to server",
                args.ClientConnection.Id, args.ClientConnection.ClientEndpoint);
        }

        private async Task HandleServerCommandMessageAsync(string message, SocketClientConnection connection)
        {
            var command = JsonConvert.DeserializeObject<ICommand>(message, JsonSocketDefaults.JsonSerializerSettings);

            var game = await _gameRepository.GetGameAsync(connection.Id) ?? await _gameRepository.AddGameAsync(new Game
            {
                Id = connection.Id
            });

            game.SceneChanged += GameOnSceneChanged;

            switch (command)
            {
                case ClientConnectedCommand:
                    game.Start();
                    break;
                case PrintTextCommand:
                    break;
            }
        }

        private void GameOnSceneChanged(Game game, Scene scene)
        {
            _logger.LogInformation("[{GameId}] - SceneChanged: {SceneId}", game.Id, scene.Id);
            SendCommand(new PrintTextCommand(scene.Description), GetClientConnection(game).ClientSocket);
        }

        public void SendCommand(ICommand command, Socket receiver)
        {
            var json = JsonConvert.SerializeObject(command, JsonSocketDefaults.JsonSerializerSettings);
            SendMessage(json, receiver);
        }

        public SocketClientConnection GetClientConnection(Game game) =>
            Connections.SingleOrDefault(x => x.Id == game.Id);
    }
}