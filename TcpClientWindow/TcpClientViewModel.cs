using System.Threading;
using System.Windows;
using System.Windows.Input;
using CB.Model.Prism;
using CB.Net.Socket;
using Microsoft.Practices.Prism.Commands;


namespace TcpClientWindow
{
    public class TcpClientViewModel: PrismViewModelBase
    {
        #region Fields
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private string _fileName;
        private readonly TcpSocketClient _tcpSocketClient = new TcpSocketClient(); // TODO: ipAddress, port
        private string _text;
        #endregion


        #region  Constructors & Destructor
        public TcpClientViewModel()
        {
            CancelCommand = new DelegateCommand(Cancel);
            DropCommand = new DelegateCommand<IDataObject>(Drop);
            SendFileAsyncCommand = DelegateCommand.FromAsyncHandler(
                () => _tcpSocketClient.SendFileAsync(FileName, _cancellationTokenSource.Token));
            SendTextAsyncCommand = DelegateCommand.FromAsyncHandler(
                () => _tcpSocketClient.SendTextAsync(Text, _cancellationTokenSource.Token));
        }
        #endregion


        #region  Properties & Indexers
        public ICommand CancelCommand { get; }
        public ICommand DropCommand { get; }

        public string FileName
        {
            get { return _fileName; }
            set { SetProperty(ref _fileName, value); }
        }

        public ICommand SendFileAsyncCommand { get; }
        public ICommand SendTextAsyncCommand { get; }

        public string Text
        {
            get { return _text; }
            set { SetProperty(ref _text, value); }
        }
        #endregion


        #region Methods
        public void Cancel()
            => _cancellationTokenSource.Cancel();

        public void Drop(IDataObject data)
        {
            var filesDrop = data.GetData(DataFormats.FileDrop, true) as string[];
            if (filesDrop?.Length > 0) FileName = filesDrop[0];
        }
        #endregion
    }
}


// TODO: Fix Cancel()