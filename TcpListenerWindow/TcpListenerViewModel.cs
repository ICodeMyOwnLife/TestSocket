using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CB.Model.Prism;
using Microsoft.Practices.Prism.Commands;


namespace TcpListenerWindow
{
    public class TcpListenerViewModel: PrismViewModelBase
    {
        #region Fields
        private bool _listen;
        private string _message;

        private readonly TcpListener _tcpListener = new TcpListener(IPAddress.Parse("127.0.0.1"), 11000);

                                     // TODO: ipAddress, port
        #endregion


        #region  Constructors & Destructor
        public TcpListenerViewModel()
        {
            ConnectCommand = new DelegateCommand(Connect);
            DisconnectCommand = new DelegateCommand(Disconnect);
            StartListeningCommand = new DelegateCommand(StartListening);
            StartListeningAsyncCommand = DelegateCommand.FromAsyncHandler(StartListeningAsync);
            StopListeningCommand = new DelegateCommand(StopListening);
        }
        #endregion


        #region  Properties & Indexers
        public ICommand ConnectCommand { get; }
        public ICommand DisconnectCommand { get; }

        public string Message
        {
            get { return _message; }
            private set { SetProperty(ref _message, value); }
        }

        public ICommand StartListeningAsyncCommand { get; }

        public ICommand StartListeningCommand { get; }
        public ICommand StopListeningCommand { get; }
        #endregion


        #region Methods
        public void Connect() => _tcpListener.Start(); // TODO: backlog

        public void Disconnect() => _tcpListener.Stop();

        public void StartListening()
        {
            _listen = true;
            var buffer = new byte[1024]; // TODO: bufferSize
            while (_listen)
            {
                using (var client = _tcpListener.AcceptTcpClient())
                {
                    using (var netStream = client.GetStream())

                    {
                        int bytesRead;

                        while ((bytesRead = netStream.Read(buffer, 0, buffer.Length)) != 0)
                        {
                            var data = Encoding.Unicode.GetString(buffer, 0, bytesRead);
                            AddMessage(data);
                        }
                    }
                }
            }
        }

        public async Task StartListeningAsync()
        {
            _listen = true;
            var buffer = new byte[1024]; // TODO: bufferSize
            while (_listen)
            {
                using (var client = await _tcpListener.AcceptTcpClientAsync())
                {
                    using (var netStream = client.GetStream())

                    {
                        int bytesRead;

                        while ((bytesRead = await netStream.ReadAsync(buffer, 0, buffer.Length)) != 0)
                        {
                            var data = Encoding.Unicode.GetString(buffer, 0, bytesRead);
                            AddMessage(data);
                        }
                    }
                }
            }
        }

        public void StopListening()
            => _listen = false;
        #endregion


        #region Implementation
        private void AddMessage(string data) => Message = string.IsNullOrEmpty(Message) ? data : Message + data;
        #endregion
    }
}