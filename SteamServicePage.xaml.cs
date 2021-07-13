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
	public partial class SteamServicePage : Page, IServicePage
	{
        SteamSettings m_settings;

		static string VDFPath
		{ get {
			return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "GameUploader", "steam-build-script.vdf");
		} }

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

		private void BuildPath_BrowseButton_Click(object sender, RoutedEventArgs e)
		{
			CommonOpenFileDialog openPathDialog = new CommonOpenFileDialog();
			openPathDialog.Multiselect = false;
			openPathDialog.IsFolderPicker = true;
			openPathDialog.InitialDirectory = !string.IsNullOrEmpty(m_settings.BuildPath) 
				? Path.GetFullPath(m_settings.BuildPath) 
				: Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);

			if (openPathDialog.ShowDialog() == CommonFileDialogResult.Ok)
				m_settings.BuildPath = openPathDialog.FileName;
		}

		private void ViewPasswordButton_Click(object sender, RoutedEventArgs e)
		{

		}

		private void UploadButton_Click(object sender, RoutedEventArgs e)
		{
			m_settings.Save(SteamSettings.DefaultPath);
			GenerateVDF();

			/*
			 * todo if you write to log as you're uploading because it's part of upload folder.... shit gets weird
			var cmdStr = new StringBuilder();
			cmdStr.Append(m_settings.PathToExe);
			cmdStr.Append($" +login {m_settings.AccountName} {m_settings.AccountPassword}");
			cmdStr.Append($" +run_app_build {VDFPath}");
			cmdStr.Append($" +quit");

			CmdHelper.RunCmd(cmdStr.ToString(), true);
			*/
		}

		private void GenerateVDF()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine($"\"AppBuild\" {{");
			sb.AppendLine($" \"AppID\" \"{m_settings.AppID}\"");
			sb.AppendLine($" \"Desc\" \"{m_settings.BuildDescription}\"");
			sb.AppendLine($" \"ContentRoot\" \"{m_settings.BuildPath}\"");
			sb.AppendLine($" \"BuildOutput\" \"{Path.Combine(m_settings.BuildPath, "output")}\"");
			sb.AppendLine($" \"Depots\" {{");
			sb.AppendLine($"  \"{m_settings.DepotID}\" {{");
			sb.AppendLine($"   \"FileMapping\" {{");
			sb.AppendLine($"    \"LocalPath\" \"*\""); //all files from the BuildPath folder...
			sb.AppendLine($"    \"DepotPath\" \".\""); //...are mapped into the root of our depot...
			sb.AppendLine($"    \"recursive\" \"1\""); //...including subfolders
			sb.AppendLine($"   }}");
			sb.AppendLine($"  }}");
			sb.AppendLine($" }}");
			sb.AppendLine($"}}");

            Directory.CreateDirectory(Path.GetDirectoryName(VDFPath));
			File.WriteAllText(VDFPath, sb.ToString());
		}
	}
}
