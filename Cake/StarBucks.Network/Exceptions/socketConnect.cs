using System;

namespace StarBucks.Network.Exceptions
{
    class socketConnect : Exception
    {
        public socketConnect() { }

        public socketConnect(String message) : base(message) { }
    }
}
