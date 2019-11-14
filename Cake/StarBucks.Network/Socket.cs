using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace StarBucks.Network
{
    // 서버로 부터 메시지를 받으면 전달하는 EventArg
    public class messageArgs : EventArgs
    {
        public string message;
        public messageArgs(string msg)
        {
            message = msg;
        }
    }

    // 서버와 연결이 끊기면 전달하는 EventArg
    public class lostConnectionArgs : EventArgs
    {
        public string message;
        public string remote;
        public int port;
        public lostConnectionArgs(string msg)
        {
            message = msg;
        }

        public lostConnectionArgs(string msg, string remote, int port)
        {
            message = msg;
            this.remote = remote;
            this.port = port;
        }
    }

    // 서버와 연결이 되면 전달하는 EventArg
    public class connectedArgs : EventArgs
    {
        public string remote;
        public int port;
        public bool connected = false;

        public connectedArgs(string remote, int port, bool connected)
        {
            this.remote = remote;
            this.port = port;
            this.connected = connected;
        }
    }

    public class AsyncSocketObject
    {
        public Byte[] buffer;
        public Socket WorkingSocket;
        public AsyncSocketObject(int bufferSize)
        {
            buffer = new byte[bufferSize];
        }
    }

    public class socket : IDisposable
    {
        public static StarBucksSocket _sck = new StarBucksSocket();
        public event EventHandler<messageArgs> messageEvent;
        public event EventHandler<lostConnectionArgs> lostEvent;
        public event EventHandler<connectedArgs> connectEvent;
        private bool disposed = false;

        public socket(string url, int port)
        {
            try
            {
                if (_sck.Connected == true)
                {
                    _sck.Client.Shutdown(SocketShutdown.Both);
                }

                _sck = null;
                _sck = new StarBucksSocket();
                _sck.remote = url;
                _sck.port = port;

                // Timeout 조절
                _sck.SendTimeout = 3;
                _sck.ReceiveTimeout = 3;
                _sck.recentTime = DateTime.Now;
                _sck.finalTime = DateTime.Now;

                // Socket 연결
                var result = _sck.BeginConnect(url, port, null, null);
                var success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(2), true);

                if (!success)
                {
                    connectEvent?.Invoke(this, new connectedArgs(url, port, false));
                    throw new Exception("Timeout");
                }

                _sck.recentTime = DateTime.Now;
                connectEvent?.Invoke(this, new connectedArgs(url, port, true));
                ListenforMessage();
            }
            catch (Exception)
            {
                throw new Exceptions.socketConnect("서버에 연결할 수 없습니다. 서버 주소, 인터넷 연결을 확인하십시오.");
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Socket 연결을 해제하고 Dispose 처리
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    // 관리 리소스 해지
                    if (_sck.Connected == true)
                    {
                        // 소캣 연결 해제
                        _sck.Client.Shutdown(SocketShutdown.Both);
                    }

                    _sck = null;
                    _sck = new StarBucksSocket();
                }

                this.disposed = true;
            }
        }

        /// <summary>
        /// Socket 인스턴스 구하기
        /// </summary>
        /// <returns>StarBucksSocket</returns>
        public StarBucksSocket GetSocketInstance()
        {
            return _sck;
        }

        /// <summary>
        /// 소켓 메시지를 받음
        /// </summary>
        private void ListenforMessage()
        {
            if (_sck.Connected == false)
            {
                // 소켓이 연결되어 있지 않음
                throw new Exceptions.socketConnect("연결이 수립되지 않았습니다.");
            }

            AsyncSocketObject socketObject = new AsyncSocketObject(_sck.ReceiveBufferSize);
            socketObject.WorkingSocket = _sck.Client;
            _sck.Client.BeginReceive(socketObject.buffer, 0, socketObject.buffer.Length, SocketFlags.None, new AsyncCallback(onSocketReceiveCallback), socketObject);
        }

        /// <summary>
        /// 서버로 부터 메시지를 받음
        /// </summary>
        /// <param name="socketObject"></param>
        private void onSocketReceiveCallback(IAsyncResult socketObject)
        {
            try
            {
                // 추가 정보 불러오기
                AsyncSocketObject socketResult = (AsyncSocketObject)socketObject.AsyncState;
                int receivedBytes = _sck.Client.EndReceive(socketObject);
                string message = Encoding.UTF8.GetString(socketResult.buffer, 0, receivedBytes);

                if (receivedBytes > 0)
                {
                    // Debug: 테스트를 위해 shutdown 이라는 메시지가 들어오면 임의로 소켓 연결 재설정
                    if (message != "shutdown")
                    {
                        _sck.lastSendtime = DateTime.Now;
                        messageEvent?.Invoke(this, new messageArgs(message));

                        AsyncSocketObject newsocketObject = new AsyncSocketObject(_sck.ReceiveBufferSize);
                        _sck.Client.BeginReceive(newsocketObject.buffer, 0, newsocketObject.buffer.Length, SocketFlags.None, new AsyncCallback(onSocketReceiveCallback), newsocketObject);

                        return;
                    }
                }

                // 소캣 연결 종료 이벤트 발생
                _sck.finalTime = DateTime.Now;
                lostEvent?.Invoke(this, new lostConnectionArgs("서버 연결 종료됨", _sck.remote, _sck.port));
            }
            catch (Exception)
            {
                // 소캣 연결 종료 이벤트 발생
                _sck.finalTime = DateTime.Now;
                lostEvent?.Invoke(this, new lostConnectionArgs("서버 연결 종료됨", _sck.remote, _sck.port));
            }
        }

        private void onSocketSendCallback(IAsyncResult socketObject)
        {
            _sck.Client.EndSend(socketObject);
        }

        public void sendMessage(string message)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            AsyncSocketObject socketObject = new AsyncSocketObject(_sck.ReceiveBufferSize);
            socketObject.WorkingSocket = _sck.Client;
            socketObject.buffer = buffer;

            try
            {
                _sck.Client.BeginSend(socketObject.buffer, 0, socketObject.buffer.Length, SocketFlags.None, new AsyncCallback(onSocketSendCallback), socketObject);
                _sck.lastSendtime = DateTime.Now;
            }
            catch (Exception)
            {
                _sck.finalTime = DateTime.Now;
                lostEvent?.Invoke(this, new lostConnectionArgs("서버 연결 종료"));
            }   
        }

        /// <summary>
        /// 소켓 다시 연결
        /// </summary>
        /// <param name="url"></param>
        /// <param name="port"></param>
        public void reconnect(string url, int port)
        {
            try
            {
                _sck.reconnectAttempt++;
                if (_sck.Connected == true)
                {
                    // 소켓이 연결이 되어있다면 소켓 연결 종료
                    _sck.Client.Shutdown(SocketShutdown.Both);
                    _sck.Client.Close();
                    _sck.Close();
                    _sck = null;
                }

                // 소캣 재설정
                _sck = new StarBucksSocket();
                _sck.remote = url;
                _sck.port = port;

                // Timeout 조절
                _sck.recentTime = DateTime.Now;
                _sck.finalTime = DateTime.Now;

                // 서버로 다시 연결
                var result = _sck.BeginConnect(url, port, null, null);
                var timeOutResult = result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(3), false);
                if (!timeOutResult)
                {
                    connectEvent?.Invoke(this, new connectedArgs(_sck.remote, _sck.port, false));
                    return;
                }

                _sck.recentTime = DateTime.Now;
                _sck.reconnectAttempt = 0;

                connectEvent?.Invoke(this, new connectedArgs(_sck.remote, _sck.port, true));
                ListenforMessage();
            }
            catch (Exception)
            {

            }
        }

        // private void onSocketReconnectCallback(IAsyncResult socketObject)
        // {
        // }
    }
}
