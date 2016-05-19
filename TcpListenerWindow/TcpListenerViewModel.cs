using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using CB.Model.Common;
using CB.Model.Prism;
using CB.Net.Socket;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Win32;


namespace TcpListenerWindow
{
    public class TcpListenerViewModel: PrismViewModelBase
    {
        #region Fields
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private string _fileName;
        private string _message;
        private readonly TcpSocketServer _tcpSocketServer = new TcpSocketServer();
        #endregion


        #region  Constructors & Destructor
        public TcpListenerViewModel()
        {
            ConnectCommand = new DelegateCommand(_tcpSocketServer.Connect);
            DisconnectCommand = new DelegateCommand(_tcpSocketServer.Disconnect);
            ReceiveTextAsyncCommand = DelegateCommand.FromAsyncHandler(ReceiveTextAsync);
            ReceiveFileAsyncCommand = DelegateCommand.FromAsyncHandler(ReceiveFileAsync);
        }
        #endregion


        #region  Properties & Indexers
        public ICommand ConnectCommand { get; }
        public ICommand DisconnectCommand { get; }

        public string FileName
        {
            get { return _fileName; }
            set { SetProperty(ref _fileName, value); }
        }

        public string Message
        {
            get { return _message; }
            private set { SetProperty(ref _message, value); }
        }

        public ProgressReporter<double> ProgressReporter { get; } = new ProgressReporter<double>();

        public ICommand ReceiveFileAsyncCommand { get; }
        public ICommand ReceiveTextAsyncCommand { get; }
        #endregion


        #region Methods
        public async Task ReceiveFileAsync()
        {
            await _tcpSocketServer.ReceiveFileAsync(fileName =>
            {
                var saveFileDialog = new SaveFileDialog
                {
                    FileName = fileName
                };
                return saveFileDialog.ShowDialog() == true ? saveFileDialog.FileName : null;
            }, _cancellationTokenSource.Token, ProgressReporter);
        }

        public async Task ReceiveTextAsync()
        {
            Message = await _tcpSocketServer.ReceiveTextAsync(_cancellationTokenSource.Token);
        }
        #endregion
    }
}