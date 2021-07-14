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
        SteamSettings m_settings;

		public SettingsPage()
		{
			InitializeComponent();
		}

		public void OnEntered()
		{
			m_settings = SteamSettings.Load(SteamSettings.DefaultPath);
			DataContext = m_settings;
		}
		 
		public void OnExited()
		{
			m_settings.Save(SteamSettings.DefaultPath);
		}

		private void PathToSettings_BrowseButton_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Multiselect = false;
			openFileDialog.Filter = "Exe files (*.exe)|*.exe";
			openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);

			if (openFileDialog.ShowDialog() == true)
				m_settings.PathToExe = openFileDialog.FileName;
		}
	}
}
