using Adventure.Core.Commands.Abstractions;
using Newtonsoft.Json;
using System;
using System.Net.Sockets;

namespace Adventure.Core.Commands
{
    public class PromptCommand : ICommand
    {
        [JsonProperty]
        public string Input { get; private set; }

        public void ExecuteClient(ICommandSender sender, Socket serverSocket)
        {
            Input = Console.ReadLine();
            sender.SendCommand(this, serverSocket);
        }

        public void ExecuteServer(ICommandSender sender, Socket clientSocket)
        {
            Console.WriteLine("Player responded with '{0}'", Input);
        }
    }
}