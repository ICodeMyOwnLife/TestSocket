using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using SocketInfrastructure;


namespace SocketServer
{
    public class Server
    {
        #region Fields
        private static int _backlog = SocketConfig.GetBacklog();
        private static readonly int _bufferSize = SocketConfig.GetBufferSize();
        private static ManualResetEvent _doneResetEvent = new ManualResetEvent(false);
        private static readonly string _ipAddress = SocketConfig.GetIpAddress();
        private static readonly int _port = SocketConfig.GetPort();
        #endregion


        #region Methods
        public static void StartListening()
        {
            var listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            var localEndPoint = new IPEndPoint(IPAddress.Parse(_ipAddress), _port);
            var buffer = new byte[_bufferSize];

            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(_backlog);

                while (true)
                {
                    _doneResetEvent.Reset();
                    listener.BeginAccept(AcceptCallback, listener);
                    _doneResetEvent.WaitOne();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static void AcceptCallback(IAsyncResult ar)
        {
            _doneResetEvent.Set();
            var listener = (Socket)ar.AsyncState;
            var handler = listener.EndAccept(ar);

        }
        #endregion
    }
}