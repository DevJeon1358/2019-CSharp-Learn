using System;

namespace StarBucks.Database
{
    class ConnectException : Exception
    {
        public ConnectException() { }

        public ConnectException(String message) : base(message) { }
    }
}
