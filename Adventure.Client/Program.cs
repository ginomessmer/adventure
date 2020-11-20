using Adventure.Core.Networking.Abstractions;
using Adventure.Core.Networking.Providers;
using System;
using Adventure.Core.Game;
using Adventure.Core.Networking;

namespace Adventure.Client
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var client = new AdventureGameSocketClient();
            client.Start();

            Console.ReadLine();
        }

        private static void StartSimpleClient()
        {
            Console.WriteLine("Connecting to server...");
            SocketClient client = new SimpleSocketClient();

            client.OnConnected += (sender, eventArgs) => Console.WriteLine("Connected to server {0}", client.ServerEndPoint);

            try
            {
                client.Start();
            }
            catch (Exception ex)
            {
                HandleExceptionAndShutdownGracefully(ex, "Error while connecting to the server");
            }

            try
            {
                Console.WriteLine("Send message...");
                client.SendMessage("Hello world");
                Console.WriteLine("Message sent");
            }
            catch (Exception ex)
            {
                HandleExceptionAndShutdownGracefully(ex, "Error while sending the message");
            }

            Console.WriteLine("Press any key to exit...");
            Console.Read();
        }

        private static void HandleExceptionAndShutdownGracefully(Exception ex, string message = "An exception occurred")
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("{0}: {1}", message, ex);
            Console.ReadLine();
            Environment.Exit(1);
        }
    }
}
