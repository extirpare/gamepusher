using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GameUploader
{
	public partial class SteamServicePage : Page, IServicePage
	{
        SteamSettings m_settings;

		public SteamServicePage()
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

		private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
		{
			System.Diagnostics.Process.Start(e.Uri.AbsoluteUri);
		}

		private void PathToExe_BrowseButton_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Multiselect = false;
			openFileDialog.Filter = "Exe files (*.exe)|*.exe";
			openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);

			if (openFileDialog.ShowDialog() == true)
				m_settings.PathToExe = openFileDialog.FileName;
		}

		private void PathToBuild_Button_Click(object sender, RoutedEventArgs e)
		{

		}

		private void OptionalSettingsDropdown_Button_Click(object sender, RoutedEventArgs e)
		{

		}

		private void UploadButton_Click(object sender, RoutedEventArgs e)
		{

		}
	}
}
