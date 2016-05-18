using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CB.Model.Prism;
using CB.Net.Socket;
using Microsoft.Practices.Prism.Commands;
using SocketInfrastructure;


namespace SocketClient
{
    public class ClientViewModel: PrismViewModelBase
    {
        #region Fields
        private Socket _client;

        private string _message;

        private readonly SocketManager _clientManager = new SocketManager
        {
            IpAddress = SocketConfig.GetIpAddress(),
            Port = SocketConfig.GetPort(),
            Backlog = SocketConfig.GetBacklog()
        };
        #endregion


        #region  Constructors & Destructor
        public ClientViewModel()
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
            _client = await _clientManager.CreateSenderAsync();
            var data = Encoding.Unicode.GetBytes(Message);
            await _client.SendAsync(data, 0, data.Length, SocketFlags.None);
        }
        #endregion
    }
}