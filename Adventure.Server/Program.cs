﻿using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Adventure.Core.Networking;

namespace Adventure.Server
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            // Create socket
            Console.WriteLine("Opening socket...");
            var entry = await Dns.GetHostEntryAsync("127.0.0.1");
            var endpoint = new IPEndPoint(entry.AddressList.First(), 14500);
            var socket = new Socket(endpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            // Bind
            Console.WriteLine("BIND");
            socket.Bind(endpoint);

            // Listen
            Console.WriteLine("LISTEN");
            socket.Listen(int.MaxValue);
            Console.WriteLine("Listening on {0}...", endpoint);

            // Accept
            Console.WriteLine("ACCEPT");
            var receiveSocket = socket.Accept();

            var buffer = new byte[SocketDefaults.MessageSize];
            var data = "";

            // Receive
            Console.WriteLine("RECEIVE...");
            while (true)
            {
                try
                {
                    receiveSocket.Receive(buffer);
                    data += Encoding.ASCII.GetString(buffer);

                    // Get header length value
                    var headerIndex = data.IndexOf(SocketDefaults.LengthHeaderName, StringComparison.Ordinal);
                    if (headerIndex > -1)
                    {
                        var header = data.Substring(headerIndex, SocketDefaults.HeaderSize);

                        // Split or regex
                        var headerKeyValue = header.Split(':');
                        var length = Convert.ToInt32(headerKeyValue[1]);

                        Console.WriteLine("Received new message");
                        Console.WriteLine("- Length: {0}", length);

                        var payload = data.Substring(headerIndex + SocketDefaults.HeaderSize, length);

                        Console.WriteLine("- Payload: {0}", payload);

                        receiveSocket.Send(Encoding.ASCII.GetBytes("Great success"));
                    }
                }
                catch (SocketException ex)
                {
                    Console.WriteLine("A socket exception ({0}) occurred: {1}", ex.SocketErrorCode, ex);
                    receiveSocket.Close();
                    receiveSocket.Dispose();
                }
            }

            Console.WriteLine("Press any key to exit...");
            Console.Read();
        }
    }
}
