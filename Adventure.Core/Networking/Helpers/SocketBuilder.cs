using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace Adventure.Core.Networking.Helpers
{
    /// <summary>
    /// A builder class that builds a Socket.
    /// </summary>
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

        /// <summary>
        /// Appends the IP address based on a host name.
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        public SocketBuilder WithHostEntry(string host)
        {
            var entry = Dns.GetHostEntry(host);
            var address = entry.AddressList.FirstOrDefault();

            if (address is null)
                throw new Exception("No suitable host name was found");

            return WithIpAddress(address);
        }

        /// <summary>
        /// Appends an IP address.
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public SocketBuilder WithIpAddress(IPAddress address)
        {
            IpAddress = address;
            return this;
        }

        /// <summary>
        /// Alias of <seealso cref="WithHostEntry"/>.
        /// <inheritdoc cref="WithHostEntry"/>
        /// </summary>
        public SocketBuilder WithEndpoint(string host) => WithHostEntry(host);

        /// <summary>
        /// Alias of <seealso cref="WithIpAddress"/>.
        /// <inheritdoc cref="WithIpAddress"/>
        /// </summary>
        public SocketBuilder WithEndpoint(IPAddress address) => WithIpAddress(address);

        /// <summary>
        /// Appends the dedicated port.
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public SocketBuilder WithPort(int port)
        {
            Port = port;
            return this;
        }

        /// <summary>
        /// Builds and returns a new Socket object passed on the information passed before.
        /// </summary>
        /// <returns>A brand new socket</returns>
        public Socket Build()
        {
            Endpoint = new IPEndPoint(IpAddress, Port);
            return new Socket(Endpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        }
    }
}