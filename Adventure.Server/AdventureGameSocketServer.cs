using Adventure.Core.Commands;
using Adventure.Core.Commands.Abstractions;
using Adventure.Core.Domain;
using Adventure.Core.Infrastructure;
using Adventure.Core.Networking;
using Adventure.Core.Networking.Abstractions;
using Adventure.Core.Resources;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Adventure.Server
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

        /// <summary>
        /// Processes incoming messages as JSON commands.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Handler that executes when a client changes a scene.
        /// </summary>
        /// <param name="gameSession"></param>
        /// <param name="scene"></param>
        private void GameOnSceneChanged(GameSession gameSession, Scene scene)
        {
            _logger.LogInformation("[{GameId}] SceneChanged: {SceneId}", gameSession.Id, scene.Id);

            var text = new StringBuilder()
                .AppendLine(scene.Description)
                .AppendLine(MenuResources.CallToAction)
                .AppendLine(MenuResources.AvailableActions)
                .AppendJoin(',', scene.Actions.Select(x => $"{x.Verb} [ {string.Join(" | ", x.AllowedParameters)} ]"));

            SendCommand(new PrintTextCommand(text.ToString()), GetClientConnection(gameSession));
            SendCommand(new PromptCommand(), GetClientConnection(gameSession));
        }

        /// <summary>
        /// Sends a command to a socket receiver.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="receiver"></param>
        public void SendCommand(ICommand command, Socket receiver)
        {
            var json = JsonConvert.SerializeObject(command, JsonSocketDefaults.JsonSerializerSettings);
            SendMessage(json, receiver);
        }

        /// <summary>
        /// An alias to <seealso cref="SendCommand(Adventure.Core.Commands.Abstractions.ICommand,System.Net.Sockets.Socket)"/>.
        /// <inheritdoc cref="SendCommand(Adventure.Core.Commands.Abstractions.ICommand,System.Net.Sockets.Socket)"/>
        /// </summary>
        /// <param name="command"></param>
        /// <param name="connection"></param>
        public void SendCommand(ICommand command, SocketClientConnection connection) =>
            SendCommand(command, connection.ClientSocket);

        /// <summary>
        /// Retrieves an existing client connection based on a game session.
        /// </summary>
        /// <param name="gameSession"></param>
        /// <returns></returns>
        public SocketClientConnection GetClientConnection(GameSession gameSession) =>
            Connections.SingleOrDefault(x => x.Id == gameSession.Id);
    }
}