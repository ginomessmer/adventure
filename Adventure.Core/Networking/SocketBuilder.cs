using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace Adventure.Core.Networking
{
    public class SocketBuilder
    {
        public IPAddress IpAddress { get; private set; }
        public int Port { get; private set; }
        public IPEndPoint Endpoint { get; private set; }

        public SocketBuilder WithHostEntry(string host)
        {
            var entry = Dns.GetHostEntry(host);
            IpAddress = entry.AddressList.First();
            return this;
        }

        public SocketBuilder WithIpAddress(IPAddress address)
        {
            IpAddress = address;
            return this;
        }

        public SocketBuilder WithPort(int port)
        {
            Port = port;
            return this;
        }

        public Socket Build()
        {
            Endpoint = new IPEndPoint(IpAddress, Port);
            return new Socket(Endpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        }
    }
}