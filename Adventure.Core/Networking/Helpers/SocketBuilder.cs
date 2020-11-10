using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace Adventure.Core.Networking.Helpers
{
    public class SocketBuilder
    {
        public IPAddress IpAddress { get; private set; }

        public int Port { get; private set; }

        public IPEndPoint Endpoint { get; private set; }

        public SocketBuilder()
        {
        }

        public SocketBuilder(IPAddress ipAddress, int port)
        {
            IpAddress = ipAddress;
            Port = port;
        }

        public SocketBuilder WithHostEntry(string host)
        {
            var entry = Dns.GetHostEntry(host);
            var address = entry.AddressList.FirstOrDefault();

            if (address is null)
                throw new Exception("No suitable host name was found");

            return WithIpAddress(address);
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