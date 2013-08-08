using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MSSBot.ViewModels;
using OHConnectivity;

namespace MSSBot.Views
{
	/// <summary>
	/// Interaction logic for ConnectView.xaml
	/// </summary>
	public partial class ConnectView : UserControl
	{
		public ConnectView()
		{
			InitializeComponent();
			this.Loaded += (s, e) => this.DataContext = new ConnectViewModel(TCPConnection.Instance());
		}
	}
}
