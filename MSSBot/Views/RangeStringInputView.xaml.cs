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
using BotLogic;

namespace MSSBot.Views
{
	/// <summary>
	/// Interaction logic for RangeStringInputView.xaml
	/// </summary>
	public partial class RangeStringInputView : UserControl
	{
		public RangeStringInputView()
		{
			InitializeComponent();
			this.Loaded += (s, e) => SetDataContext();
		}

		void SetDataContext()
		{
			this.DataContext = new RangeStringInputViewModel(PreflopStrategy.Instance);
		}
	}
}
