using Adventure.Core.Networking;
using Adventure.Core.Networking.Providers;
using System;

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
    }
}
