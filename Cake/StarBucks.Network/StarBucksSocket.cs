using System;
using System.Net.Sockets;

namespace StarBucks.Network
{
    public class StarBucksSocket : TcpClient
    {
        public DateTime recentTime;
        public DateTime finalTime;

        public StarBucksSocket() : base() { }
        public StarBucksSocket(string hostname, int port) : base (hostname, port) { }
    }
}
