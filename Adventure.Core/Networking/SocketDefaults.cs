using System;
using System.Collections.Generic;
using System.Text;

namespace Adventure.Core.Networking
{
    public static class SocketDefaults
    {
        /// <summary>
        /// The default message size in bytes.
        /// </summary>
        public const int MessageSize = 1024;

        public const int HeaderSize = 128;

        public const string LengthHeaderName = "l:";
    }
}
