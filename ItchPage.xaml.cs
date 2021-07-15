using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace GameUploader
{
	public partial class ItchPage : Page, IServicePage
	{
		public string ServiceName { get { return "Itch"; } }

		public ItchPage()
		{
			InitializeComponent();
		}

		public void OnEntered()
		{
			DataContext = MetaSettings.Instance;
		}
		 
		public void OnExited()
		{
			MetaSettings.Instance.Save();
		}

		private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
		{
			System.Diagnostics.Process.Start(e.Uri.AbsoluteUri);
			e.Handled = true;
		}

	}
}
