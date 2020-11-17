﻿using Adventure.Core.Networking.Providers;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Adventure.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Create socket
            Console.WriteLine("Starting server...");

            var serverThread = new Thread(() =>
            {
                var server = new SimpleSocketServer();
                server.OnMessageReceived += (sender, receivedArgs) =>
                {
                    Console.WriteLine("Message received from client {0}: {1}", receivedArgs.Connection.Id, receivedArgs.Message);
                };

                server.OnClientDisconnected += (sender, disconnectedArgs) =>
                {
                    Console.WriteLine("Client {0} disconnected from server", disconnectedArgs.Connection.Id);
                };

                server.Start();
            });

            serverThread.Start();
            Console.WriteLine("Server started in thread {0}", serverThread.ManagedThreadId);
        }
    }
}
