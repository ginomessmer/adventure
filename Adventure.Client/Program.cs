using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Adventure.Core.Networking;

namespace Adventure.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            // Create socket
            Console.WriteLine("Connecting to socket...");
            SocketClient client = new SimpleSocketClient();

            try
            {
                // Connect
                Console.WriteLine("Start client");
                client.Start();
            }
            catch (Exception ex)
            {
                HandleExceptionAndShutdown(ex, "Error while connecting to the server");
            }

            try
            {
                Console.WriteLine("Send message");
                client.SendMessage("Hello world");
            }
            catch (Exception ex)
            {
                HandleExceptionAndShutdown(ex, "Error while sending the message");
            }

            // Receive
            var receiveBuffer = new byte[1024];
            //socket.Receive(receiveBuffer);
            
            Console.WriteLine("Reply: {0}", Encoding.ASCII.GetString(receiveBuffer));

            Console.WriteLine("Press any key to exit...");
            Console.Read();
        }

        public static void HandleExceptionAndShutdown(Exception ex, string message = "An exception occurred")
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("{0}: {1}", message, ex);
            Console.ReadLine();
            Environment.Exit(1);
        }
    }
}
