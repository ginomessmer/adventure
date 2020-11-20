using Adventure.Core.Commands.Abstractions;
using System;
using System.Net.Sockets;

namespace Adventure.Core.Commands
{
    public class PrintTextCommand : ICommand
    {
        public string Message { get; }

        public PrintTextCommand(string message)
        {
            Message = message;
        }

        public void ExecuteClient(ICommandSender sender, Socket serverSocket)
        {
            Console.WriteLine(Message);
        }

        public void ExecuteServer(ICommandSender sender, Socket clientSocket)
        {
        }
    }
}