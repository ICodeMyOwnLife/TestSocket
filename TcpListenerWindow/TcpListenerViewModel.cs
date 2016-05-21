using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
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
        private string _message;
        private readonly TcpSocketServer _tcpSocketServer = new TcpSocketServer(new TcpSocketConfiguration());
        #endregion


        #region  Constructors & Destructor
        public TcpListenerViewModel()
        {
            CancelCommand = new DelegateCommand(Cancel);
            ConnectCommand = new DelegateCommand(_tcpSocketServer.Connect);
            DisconnectCommand = new DelegateCommand(_tcpSocketServer.Disconnect);
            ReceiveFileAsyncCommand = DelegateCommand.FromAsyncHandler(ReceiveFileAsync);
            ReceiveFileWithProgressAsyncCommand = DelegateCommand.FromAsyncHandler(ReceiveFileWithProgressAsync);
            ReceiveTextAsyncCommand = DelegateCommand.FromAsyncHandler(ReceiveTextAsync);
        }
        #endregion


        #region  Properties & Indexers
        public ICommand CancelCommand { get; }
        public ICommand ConnectCommand { get; }
        public ICommand DisconnectCommand { get; }

        public string Message
        {
            get { return _message; }
            private set { SetProperty(ref _message, value); }
        }

        public FileProgressReporter ProgressReporter { get; } = new FileProgressReporter();

        public ICommand ReceiveFileAsyncCommand { get; }
        public ICommand ReceiveFileWithProgressAsyncCommand { get; }
        public ICommand ReceiveTextAsyncCommand { get; }
        #endregion


        #region Methods
        public void Cancel()
            => _cancellationTokenSource.Cancel();

        public async Task ReceiveFileAsync()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            await _tcpSocketServer.ReceiveFileAsync(GetSavePath);
            MessageBox.Show(stopwatch.Elapsed.ToString("c"));
            stopwatch.Stop();
        }

        public async Task ReceiveFileWithProgressAsync()
            => await _tcpSocketServer.ReceiveFileAsync(fileInfo =>
            {
                ProgressReporter.FileSize = fileInfo.FileSize;
                return GetSavePath(fileInfo);
            }, _cancellationTokenSource.Token, ProgressReporter);

        public async Task ReceiveTextAsync()
            => Message = await _tcpSocketServer.ReceiveTextAsync(_cancellationTokenSource.Token);
        #endregion


        #region Implementation
        private static string GetSavePath(IFileInfo fileInfo)
        {
            var saveFileDialog = new SaveFileDialog
            {
                FileName = fileInfo.FileName
            };
            return saveFileDialog.ShowDialog() == true ? saveFileDialog.FileName : null;
        }
        #endregion
    }
}