using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Windows.Threading;

namespace GameUploader
{
	public partial class ItchPage : Page, IServicePage
	{
		public string ServiceName { get { return "Itch"; } }
		ItchSettings m_settings;
		MainWindow m_parentWindow;

		public ItchPage()
		{
			InitializeComponent();
		}

		public void SetParentWindow(MainWindow window)
		{
			m_parentWindow = window;
		}

		public void OnEntered()
		{
			m_settings = ItchSettings.Load();
			DataContext = m_settings;
		}

		public void OnExited()
		{
			m_settings.Save();
		}

		private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
		{
			System.Diagnostics.Process.Start(e.Uri.AbsoluteUri);
			e.Handled = true;
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

		private void PathToUpload_BrowseButton_Click(object sender, RoutedEventArgs e)
		{
			CommonOpenFileDialog openPathDialog = new CommonOpenFileDialog();
			openPathDialog.Multiselect = false;
			openPathDialog.IsFolderPicker = true;
			openPathDialog.InitialDirectory = !string.IsNullOrEmpty(m_settings.PathToUpload)
				? Path.GetFullPath(m_settings.PathToUpload)
				: Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);

			if (openPathDialog.ShowDialog() == CommonFileDialogResult.Ok)
				m_settings.PathToUpload = openPathDialog.FileName;
		}

		private void LoginButton_Click(object sender, RoutedEventArgs e)
		{
			m_parentWindow.RunBlockingCmd(m_settings.PathToExe + " login", () => PushPropertyChanges());
		}

		private void LogoutButton_Click(object sender, RoutedEventArgs e)
		{
			m_parentWindow.RunBlockingCmd(m_settings.PathToExe + " logout --assume-yes", () => PushPropertyChanges());
		}

		private void UploadButton_Click(object sender, RoutedEventArgs e)
		{
			m_settings.Save();
			m_parentWindow.RunBlockingCmd(GenerateCommand());
		}

		private void CopyCommandButton_Click(object sender, RoutedEventArgs e)
		{
			Clipboard.SetText(GenerateCommand());
		}

		private string GenerateCommand()
		{
			var cmdStr = new StringBuilder();
			cmdStr.Append('"' + m_settings.PathToExe + '"');
			cmdStr.Append(" push ");
			cmdStr.Append('"' + m_settings.PathToUpload + '"');
			cmdStr.Append($"  {m_settings.ProjectUsername}/{m_settings.ProjectName}:{m_settings.ChannelName}");

			if (m_settings.WantsOverrideVersionNum)
				cmdStr.Append(" --userversion \"" + m_settings.OverrideVersionNum + '"');

			if (m_settings.WantsIgnorePattern)
			{
				var ignorePatternSubstrs = m_settings.IgnorePattern.Split(',');
				foreach (var substr in ignorePatternSubstrs)
					cmdStr.Append(" --ignore \"" + substr.Trim() + '"');
			}

			return cmdStr.ToString();
		}
		
		private void OptionalSettingsDropdownButton_Click(object sender, RoutedEventArgs e)
		{
			//credit to https://social.msdn.microsoft.com/Forums/vstudio/en-US/633b9bb0-c3cb-4ab2-aff3-df48065a14f4/how-to-make-a-drop-down-menu-in-wpf?forum=wpf
			(sender as Button).ContextMenu.IsEnabled = true;
			(sender as Button).ContextMenu.PlacementTarget = (sender as Button);
			(sender as Button).ContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
			(sender as Button).ContextMenu.IsOpen = true;
		}

		private void ToggleWantsOverrideVersionNum_Click(object sender, RoutedEventArgs e) { m_settings.WantsOverrideVersionNum = !m_settings.WantsOverrideVersionNum; }
		private void ToggleWantsIgnorePattern_Click(object sender, RoutedEventArgs e) { m_settings.WantsIgnorePattern = !m_settings.WantsIgnorePattern; }

		private void PushPropertyChanges()
		{
			m_settings.PushPropertyChanges();
		}
	}
}
