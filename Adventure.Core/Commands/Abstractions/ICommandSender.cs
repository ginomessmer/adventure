using System.Net.Sockets;

namespace Adventure.Core.Commands.Abstractions
{
    public interface ICommandSender
    {
        void SendCommand(ICommand command, Socket receiver);
    }
}