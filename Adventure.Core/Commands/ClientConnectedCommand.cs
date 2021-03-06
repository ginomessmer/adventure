﻿using Adventure.Core.Commands.Abstractions;
using System;
using System.Net.Sockets;

namespace Adventure.Core.Commands
{
    public class ClientConnectedCommand : ICommand
    {
        public void ExecuteClient(ICommandSender sender, Socket serverSocket)
        {
        }

        public void ExecuteServer(ICommandSender sender, Socket clientSocket)
        {
            Console.WriteLine("Client connected: {0}", clientSocket.RemoteEndPoint);
        }
    }
}