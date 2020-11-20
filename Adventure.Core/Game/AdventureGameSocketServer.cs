using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Adventure.Core.Commands;
using Adventure.Core.Commands.Abstractions;
using Adventure.Core.Domain;
using Adventure.Core.Infrastructure;
using Adventure.Core.Networking;
using Adventure.Core.Networking.Abstractions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Adventure.Core.Game
{
    /// <summary>
    /// A game server based on JSON.
    /// </summary>
    public sealed class AdventureGameSocketServer : SocketServer, ICommandSender
    {
        private readonly IGameRepository _gameRepository;
        private readonly ILogger<AdventureGameSocketServer> _logger;

        public AdventureGameSocketServer(IGameRepository gameRepository, ILogger<AdventureGameSocketServer> logger)
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

            var game = await _gameRepository.GetGameAsync(connection.Id) ?? await _gameRepository.AddGameAsync(new GameSession
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

        private void GameOnSceneChanged(GameSession gameSession, Scene scene)
        {
            _logger.LogInformation("[{GameId}] SceneChanged: {SceneId}", gameSession.Id, scene.Id);

            var text = new StringBuilder()
                .AppendLine(scene.Description)
                .AppendLine("Was tust du?")
                .AppendLine("Mögliche Aktionen: ")
                .AppendJoin(',', scene.Actions.Select(x => x.Verb));

            SendCommand(new PrintTextCommand(text.ToString()), GetClientConnection(gameSession).ClientSocket);
            SendCommand(new PromptCommand(), GetClientConnection(gameSession).ClientSocket);
        }

        public void SendCommand(ICommand command, Socket receiver)
        {
            var json = JsonConvert.SerializeObject(command, JsonSocketDefaults.JsonSerializerSettings);
            SendMessage(json, receiver);
        }

        public SocketClientConnection GetClientConnection(GameSession gameSession) =>
            Connections.SingleOrDefault(x => x.Id == gameSession.Id);
    }
}