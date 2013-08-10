using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MVVMBase;
using OHConnectivity;
using System.Windows.Input;
using System.IO;

namespace MSSBot.ViewModels
{
	public class ConnectViewModel : ViewModelBase
	{
		TCPConnection connection;
		ICommand connectCommand;
		string connectButtonContent;
		string log;
		int port;

		public ConnectViewModel(TCPConnection connection)
		{
			this.connection = connection;
			OnConnectedChanged();
			this.connection.ConnectedChanged += (s, e) => OnConnectedChanged();
			this.connection.MessageReceived += (s, e) => OnMessageReceived(e.ReceivedData);
			this.connection.ConnectionLost += (s, e) => connection.Connect(Port);

			Port = 4444;
		}

		public ICommand ConnectCommand
		{
			get
			{
				if (connectCommand == null)
					connectCommand = new RelayCommand(param => ConnectCommandExecute());

				return connectCommand;
			}
		}

		public string ConnectButtonContent
		{
			get
			{ return connectButtonContent; }

			set
			{
				connectButtonContent = value;
				OnPropertyChanged("ConnectButtonContent");
			}
		}

		public string Log
		{
			get
			{ return log; }

			set
			{
				log = value;
				OnPropertyChanged("Log");
			}
		}

		public int Port
		{
			get
			{ return port; }

			set
			{
				port = value;
				OnPropertyChanged("Port");
			}
		}

		void OnConnectedChanged()
		{
			ConnectButtonContent = connection.IsConnected ? "Disconnect" : "Connect";
		}

		void OnMessageReceived(byte[] receivedData)
		{	
			HoldemState receivedState = new HoldemState(receivedData);

			Log += string.Format("{0}\n", receivedState);
			connection.Send(new AutoplayerInstruction(AutoplayerAction.Fold, -1, 0));
		}

		void ConnectCommandExecute()
		{
			if (connection.IsConnected)
				connection.Disconnect();
			else
				connection.Connect(Port);
		}
	}
}
