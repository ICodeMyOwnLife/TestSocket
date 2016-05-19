using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CB.Model.Prism;
using Microsoft.Practices.Prism.Commands;


namespace TcpClientWindow
{
    public class TcpClientViewModel: PrismViewModelBase
    {
        #region Fields
        private string _message;
        #endregion


        #region  Constructors & Destructor
        public TcpClientViewModel()
        {
            SendMessageAsyncCommand = DelegateCommand.FromAsyncHandler(SendMessageAsync);
        }
        #endregion


        #region  Properties & Indexers
        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        public ICommand SendMessageAsyncCommand { get; }
        #endregion


        #region Methods
        public async Task SendMessageAsync()
        {
            var data = Encoding.Unicode.GetBytes(Message);

            using (var tcpClient = new TcpClient(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 11000)))

                // TODO: ipAddress, port
            {
                using (var netStream = tcpClient.GetStream())
                {
                    await netStream.WriteAsync(data, 0, data.Length);
                }
            }
        }
        #endregion
    }
}