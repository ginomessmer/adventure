using System;
using System.Threading.Tasks;
using Adventure.GrpcServer;
using Grpc.Net.Client;

namespace Adventure.GrpcClient
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var greeterClient = new Greeter.GreeterClient(channel);
            var reply = await greeterClient.SayHelloAsync(new HelloRequest
            {
                Name = Console.ReadLine()
            });

            Console.WriteLine(reply.Message);
            Console.ReadLine();
        }
    }
}
