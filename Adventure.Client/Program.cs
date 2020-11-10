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
            var entry = await Dns.GetHostEntryAsync("127.0.0.1");
            var endpoint = new IPEndPoint(entry.AddressList.First(), SocketDefaults.Port);
            var socket = new Socket(endpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                // Connect
                Console.WriteLine("CONNECT");
                await socket.ConnectAsync(endpoint);
                Console.WriteLine("Connected to {0}", endpoint);
            }
            catch (Exception ex)
            {
                HandleExceptionAndShutdown(ex, "Error while connecting to the server");
            }

            try
            {
                // Send
                Console.WriteLine("SEND");

                var payload = "Hello world";
                var payloadBuffer = Encoding.ASCII.GetBytes(payload);
                var payloadSize = payloadBuffer.Length;

                var header = $"{SocketDefaults.LengthHeaderName}{payloadSize}";
                var headerBuffer = new byte[SocketDefaults.HeaderSize]; // Fixed header size
                var headerSize = Encoding.ASCII.GetBytes(header, headerBuffer);

                // Send over socket
                var message = new List<byte>();
                message.AddRange(headerBuffer);
                message.AddRange(payloadBuffer);
                socket.Send(message.ToArray());
            }
            catch (Exception ex)
            {
                HandleExceptionAndShutdown(ex, "Error while sending the message");
            }

            // Receive
            var receiveBuffer = new byte[1024];
            socket.Receive(receiveBuffer);
            
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
