using System;
using System.IO;
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
        private string _fileName;
        private string _message;
        private readonly TcpSocketServer _tcpSocketServer;
        #endregion


        #region  Constructors & Destructor
        public TcpListenerViewModel()
        {
            _tcpSocketServer = new TcpSocketServer(this);
            ConnectCommand = new DelegateCommand(_tcpSocketServer.Connect);
            DisconnectCommand = new DelegateCommand(_tcpSocketServer.Disconnect);
            StartReceiveMessageAsyncCommand = DelegateCommand.FromAsyncHandler(_tcpSocketServer.StartReceiveMessageAsync);
            StopListeningCommand = new DelegateCommand(_tcpSocketServer.StopListening);
            ReceiveFileAsyncCommand = DelegateCommand.FromAsyncHandler(_tcpSocketServer.ReceiveFileAsync);
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

        public ICommand ReceiveFileAsyncCommand { get; }
        public ICommand StartReceiveMessageAsyncCommand { get; }
        public ICommand StopListeningCommand { get; }
        #endregion
        

        // TODO: ipAddress, port
    }
}