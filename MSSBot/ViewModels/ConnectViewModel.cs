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
			StringBuilder sb = new StringBuilder();
			int count = 0;

			for(int i = 0; i < 552; i++)
			{
				sb.Append(string.Format("0x{0:X}, ", receivedData[i]));
				if (i % 16 == 15)
					sb.Append("\n");
			}

			File.WriteAllText("data.txt", sb.ToString());

			using (FileStream stream = File.Create("data"))
				stream.Write(receivedData, 0, 552);

			HoldemState receivedState = new HoldemState(receivedData);
			Log += receivedState.TableTitle;
			
			Log += "\n";
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
