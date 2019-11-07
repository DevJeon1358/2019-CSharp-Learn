using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace StarBucks.Network
{
    public class messageArgs : EventArgs
    {
        public string message;
        public messageArgs(string msg)
        {
            message = msg;
        }
    }

    public class socket
    {
        public static StarBucksSocket _sck = new StarBucksSocket();
        static NetworkStream stream = default(NetworkStream);
        public event EventHandler<messageArgs> messageEvent;
        public socket(string url, int port)
        {
            try
            {
                _sck.Connect(url, port);
                stream = _sck.GetStream();
                _sck.recentTime = DateTime.Now;
                ListenforMessage();
            }
            catch (Exception)
            {
                throw new Exceptions.socketConnect("서버에 연결할 수 없습니다. 서버 주소, 인터넷 연결을 확인하십시오.");
            }
        }

        public StarBucksSocket GetSocketInstance()
        {
            return _sck;
        }

        private void ListenforMessage()
        {
            if (_sck.Connected == false)
            {
                throw new Exceptions.socketConnect("연결이 수립되지 않았습니다.");
            }

            Thread listenThread = new Thread(new ThreadStart(delegate
            {
                while (true)
                {
                    try
                    {
                        var stream = _sck.GetStream();
                        int BUFFERSIZE = _sck.ReceiveBufferSize;
                        byte[] buffer = new byte[BUFFERSIZE];

                        int bytes = stream.Read(buffer, 0, buffer.Length);
                        string message = Encoding.UTF8.GetString(buffer, 0, bytes);
                        messageEvent?.Invoke(this, new messageArgs(message));
                    }
                    catch (Exception)
                    {
                        throw new Exceptions.socketConnect("서버와의 연결이 끊겼습니다.");
                    }
                }
            }));

            listenThread.Start();
        }


        public void sendMessage(string message)
        {
            if (_sck.Connected == false)
            {
                throw new Exceptions.socketConnect("연결이 수립되지 않았습니다.");
            }

            byte[] buffer = Encoding.UTF8.GetBytes(message);
            stream.Write(buffer, 0, buffer.Length);
            stream.Flush();
        }
    }
}
