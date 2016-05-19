using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;


namespace TcpListenerWindow
{
    internal class TcpSocketParams
    {
        #region Fields
        internal const int BACKLOG = 100;
        internal const string IP_ADDRESS = "127.0.0.1";
        internal const int PORT = 11000;
        #endregion
    }

    public class TcpSocketServer
    {
        #region Fields
        private readonly int _backlog;
        private readonly string _ipAddress;
        private bool _listen;
        private readonly int _port;
        private TcpListener _tcpListener;
        #endregion


        #region  Constructors & Destructor
        public TcpSocketServer(string ipAddress, int port, int backlog)
        {
            _ipAddress = ipAddress;
            _port = port;
            _backlog = backlog;
        }
        #endregion


        #region Methods
        public void Connect()
        {
            _tcpListener = new TcpListener(IPAddress.Parse(_ipAddress), _port);
            _tcpListener.Start(100); // TODO: backlog
        }

        public void Disconnect()
        {
            _tcpListener.Stop();
        }

        public async Task ReceiveFileAsync(string fileName)
        {
            using (var writer = File.OpenWrite(fileName))
            {
                await ReceiveData((buffer, bytesRead) => { writer.Write(buffer, 0, bytesRead); });
            }
        }

        public async Task StartReceiveMessageAsync()
            => await StartListeningAsync((buffer, bytesRead) =>
            {
                var text = Encoding.Unicode.GetString(buffer, 0, bytesRead);
                AddMessage(text);
            });

        public void StopListening()
            => _listen = false;
        #endregion


        #region Implementation
        private async Task ReceiveData(Action<byte[], int> onReceiveData)
        {
            var buffer = new byte[1024]; // TODO: bufferSize

            using (var client = await _tcpListener.AcceptTcpClientAsync())
            {
                using (var netStream = client.GetStream())
                {
                    int bytesRead;
                    while ((bytesRead = await netStream.ReadAsync(buffer, 0, buffer.Length)) != 0)
                    {
                        onReceiveData(buffer, bytesRead);
                    }
                }
            }
        }

        private async Task StartListeningAsync(Action<byte[], int> onReceiveData)
        {
            _listen = true;
            while (_listen)
            {
                await ReceiveData(onReceiveData);
            }
        }
        #endregion
    }
}