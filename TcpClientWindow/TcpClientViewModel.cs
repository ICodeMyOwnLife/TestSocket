using System.Windows.Input;
using CB.Model.Prism;
using Microsoft.Practices.Prism.Commands;


namespace TcpClientWindow
{
    public class TcpClientViewModel: PrismViewModelBase
    {
        #region Fields
        private string _fileName;

        private readonly TcpSocketClient _tcpSocketClient = new TcpSocketClient("127.0.0.1", 11000);

                                          // TODO: ipAddress, port

        private string _text;
        #endregion


        #region  Constructors & Destructor
        public TcpClientViewModel()
        {
            SendTextAsyncCommand = DelegateCommand.FromAsyncHandler(() => _tcpSocketClient.SendTextAsync(Text));
            SendFileAsyncCommand = DelegateCommand.FromAsyncHandler(() => _tcpSocketClient.SendFileAsync(FileName));
        }
        #endregion


        #region  Properties & Indexers
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
    }
}