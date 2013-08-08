using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Windows.Forms;

namespace OHConnectivity
{
	public class TCPConnection
	{
		public event EventHandler<MessageReceivedEventArgs> MessageReceived;
		public event EventHandler ConnectedChanged;

		static TCPConnection instance;
		Socket socket;
		byte[] buffer;
		bool isConnected;

		TCPConnection()
		{
			buffer = new byte[1024];
			isConnected = false;
		}

		public static TCPConnection Instance()
		{
			if (instance == null)
				instance = new TCPConnection();

			return instance;
		}

		public bool IsConnected
		{
			get
			{ return isConnected; }

			private set
			{
				if (value != isConnected)
				{
					isConnected = value;

					if (ConnectedChanged != null)
						ConnectedChanged(this, EventArgs.Empty);
				}
			}
		}

		public void Connect(int port)
		{
			try
			{
				socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

				IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
				IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, port);

				socket.BeginConnect(ipEndPoint, new AsyncCallback(OnConnect), null);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Connect failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		public void Disconnect()
		{
			try
			{
				if (socket != null)
					socket.Close();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Disconnect failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			IsConnected = socket.Connected;
		}

		void OnConnect(IAsyncResult ar)
		{
			try
			{
				socket.EndConnect(ar);
				StartReceive();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "OnConnect failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			IsConnected = socket.Connected;
		}

		void StartReceive()
		{
			Array.Clear(buffer, 0, buffer.Length);

			//Start listening for messages
			socket.BeginReceive(buffer,
								0,
								buffer.Length,
								SocketFlags.None,
								new AsyncCallback(OnReceive),
								null);

		}

		void OnReceive(IAsyncResult ar)
		{
			try
			{
				socket.EndReceive(ar);

				if (MessageReceived != null)
					MessageReceived(this, new MessageReceivedEventArgs(buffer));

				Array.Clear(buffer, 0, buffer.Length);

				socket.BeginReceive(buffer,
									0,
									buffer.Length,
									SocketFlags.None,
									new AsyncCallback(OnReceive),
									socket);

			}
			catch (ObjectDisposedException)
			{ }
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "OnReceive failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			IsConnected = socket.Connected;
		}
	}

	public class MessageReceivedEventArgs : EventArgs
	{
		public MessageReceivedEventArgs(byte[] receivedData)
		{
			ReceivedData = receivedData;
		}

		public byte[] ReceivedData { get; private set; }
	}
}
