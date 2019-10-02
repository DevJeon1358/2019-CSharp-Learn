using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarBucks.Database
{
    class ConnectException : Exception
    {
        public ConnectException() { }

        public ConnectException(String message) : base(message) { }
    }
}
