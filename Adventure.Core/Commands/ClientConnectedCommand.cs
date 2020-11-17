using Adventure.Core.Commands.Abstractions;
using System;

namespace Adventure.Core.Commands
{
    public class ClientConnectedCommand : ICommand
    {
        public void ExecuteClient(ICommandSender sender)
        {
            throw new NotImplementedException();
        }

        public void ExecuteServer(ICommandSender sender)
        {
            throw new NotImplementedException();
        }
    }
}