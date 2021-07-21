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
	public partial class SettingsPage : Page, IServicePage
	{
		public string ServiceName { get { return "Settings"; } }

		public SettingsPage()
		{
			InitializeComponent();
		}

		public void SetParentWindow(MainWindow window)
		{
			//settings page don't care
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

		private void SavePasswordsBox_Checked(object sender, RoutedEventArgs e)
		{
			MetaSettings.Instance.SavePasswords = true;
		}

		private void SavePasswordsBox_Unchecked(object sender, RoutedEventArgs e)
		{
			MetaSettings.Instance.SavePasswords = false;
		}
	}
}
