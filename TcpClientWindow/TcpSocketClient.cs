using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using CB.Model.Common;


namespace TcpClientWindow
{
    public class TcpSocketClient
    {
        #region Fields
        private readonly string _ipAddress;
        private readonly int _port;
        #endregion


        #region  Constructors & Destructor
        public TcpSocketClient(string ipAddress, int port)
        {
            _ipAddress = ipAddress;
            _port = port;
        }
        #endregion


        #region Methods
        public void SendFile(string fileName) => SendData(GetFileData(fileName));
        public async Task SendFileAsync(string fileName) => await SendDataAsync(GetFileData(fileName));
        public void SendObject<T>(T obj) => SendText(Serialize(obj));
        public async Task SendObjectAsync<T>(T obj) => await SendTextAsync(Serialize(obj));
        public void SendText(string text) => SendData(GetTextData(text));
        public async Task SendTextAsync(string text) => await SendDataAsync(GetTextData(text));
        #endregion


        #region Implementation
        private static byte[] GetFileData(string fileName)
        {
            return File.ReadAllBytes(fileName);
        }

        private static byte[] GetTextData(string text)
        {
            return Encoding.Unicode.GetBytes(text);
        }

        private void SendData(byte[] data)
        {
            using (var tcpClient = new TcpClient(_ipAddress, _port))
            {
                using (var netStream = tcpClient.GetStream())
                {
                    netStream.Write(data, 0, data.Length);
                }
            }
        }

        private async Task SendDataAsync(byte[] data)
        {
            using (var tcpClient = new TcpClient(_ipAddress, _port))
            {
                using (var netStream = tcpClient.GetStream())
                {
                    await netStream.WriteAsync(data, 0, data.Length);
                }
            }
        }

        private static string Serialize<T>(T obj)
            => new JsonModelSerializer().Serialize(obj);
        #endregion
    }
}