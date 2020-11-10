using System;
using System.Collections.Generic;
using System.Text;

namespace Adventure.Core.Networking
{
    public static class SocketDefaults
    {
        /// <summary>
        /// The default port for the socket application.
        /// </summary>
        public const int Port = 14500;

        /// <summary>
        /// The default message size in bytes.
        /// </summary>
        public const int MessageSize = 1024;

        /// <summary>
        /// The default header size in bytes.
        /// </summary>
        public const int HeaderSize = 128;

        /// <summary>
        /// The default header key name for the message's length.
        /// </summary>
        public const string LengthHeaderName = "L";

        /// <summary>
        /// The default loopback address.
        /// </summary>
        public const string LoopbackAddress = "127.0.0.1";
    }
}
