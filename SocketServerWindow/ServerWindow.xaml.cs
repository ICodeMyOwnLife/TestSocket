using System.Net.Sockets;
using System.Windows;
using CB.Net.Socket;
using SocketInfrastructure;


namespace SocketServerWindow
{
    public partial class ServerWindow
    {
        #region Fields
        private readonly Socket _listener;
        #endregion


        #region  Constructors & Destructor
        public ServerWindow()
        {
            InitializeComponent();
            var socketManager = new SocketManager
            {
                Backlog = SocketConfig.GetBacklog(),
                Eof = SocketConfig.GetEof(),
                IpAddress = SocketConfig.GetIpAddress(),
                Port = SocketConfig.GetPort()
            };
            _listener = socketManager.CreateListener();
        }
        #endregion


        #region Event Handlers
        private async void CmdStartListening_OnClick(object sender, RoutedEventArgs e)
        {
            var buffer = new byte[SocketConfig.GetBufferSize()];
            while (true)
            {
                var handler = await _listener.AcceptAsync();
                while (true)
                {
                    var bytesRead = await handler.ReceiveAsync(buffer, 0, buffer.Length, SocketFlags.None);
                }
            }
        }
        #endregion
    }
}