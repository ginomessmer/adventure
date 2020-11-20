using System.Net.Sockets;

namespace Adventure.Core.Commands.Abstractions
{
    public interface ICommand
    {
        void ExecuteClient(ICommandSender sender, Socket serverSocket);

        void ExecuteServer(ICommandSender sender, Socket clientSocket);
    }
}
