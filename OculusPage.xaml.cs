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
	public partial class OculusPage : Page, IServicePage
	{
		public string ServiceName { get { return "Oculus"; } }
        OculusSettings m_settings;

        public OculusPage()
		{
			InitializeComponent();
        }

        public void OnEntered()
        {
            m_settings = OculusSettings.Load(OculusSettings.DefaultPath);
            DataContext = m_settings;
        }

        public void OnExited()
        {
            m_settings.Save(OculusSettings.DefaultPath);
        }

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Uri.AbsoluteUri);
        }

        private void PathToOculusExe_BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;
            openFileDialog.Filter = "Exe files (*.exe)|*.exe";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);

            if (openFileDialog.ShowDialog() == true)
                m_settings.PathToOculusExe = openFileDialog.FileName;
        }

        private void PathToAPK_BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;
            openFileDialog.Filter = "APK files (*.apk)|*.apk";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);

            if (openFileDialog.ShowDialog() == true)
                m_settings.PathToAPK = openFileDialog.FileName;
        }

        private void OptionalSettingsDropdownButton_Click(object sender, RoutedEventArgs e)
        {
            //credit to https://social.msdn.microsoft.com/Forums/vstudio/en-US/633b9bb0-c3cb-4ab2-aff3-df48065a14f4/how-to-make-a-drop-down-menu-in-wpf?forum=wpf
            (sender as Button).ContextMenu.IsEnabled = true;
            (sender as Button).ContextMenu.PlacementTarget = (sender as Button);
            (sender as Button).ContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
            (sender as Button).ContextMenu.IsOpen = true;
        }

        private void UploadBuildButton_Click(object sender, RoutedEventArgs e)
        {
            m_settings.Save(OculusSettings.DefaultPath);

            if (!IsValidOculusExe(m_settings.PathToOculusExe))
            {
                MessageBox.Show("We aren't getting expected behavior from Path to ovr-platform-util.exe . Check that it's really a valid ovr-platform-util.exe.",
                    "Upload Aborted",
                    MessageBoxButton.OK,
                    MessageBoxImage.Stop);
                return;
            }

            var cmdStr = new StringBuilder();
            cmdStr.Append(m_settings.PathToOculusExe);
            cmdStr.Append(' ');
            switch (m_settings.Platform)
            {
                case OculusSettings.ePlatform.Quest: cmdStr.Append("upload-quest-build"); break;
                case OculusSettings.ePlatform.Rift: cmdStr.Append("upload-rift-build"); break;
                default: throw new Exception();
            }
            cmdStr.Append(" --app_id \"" + m_settings.AppID + '"');
            switch (m_settings.CredentialsSource)
            {
                case OculusSettings.eCredentialsSource.AppSecret: cmdStr.Append(" --app_secret " + m_settings.AppSecret); break;
                case OculusSettings.eCredentialsSource.UserToken: cmdStr.Append(" --token " + m_settings.UserToken); break;
                default: throw new Exception();
            }
            cmdStr.Append(" --apk \"" + m_settings.PathToAPK + '"');
            cmdStr.Append(" --channel \"" + m_settings.ReleaseChannel + '"');

            if (m_settings.WantsReleaseNotes)
                cmdStr.Append(" --notes \"" + m_settings.ReleaseNotes + '"');

            CmdHelper.RunCmd(cmdStr.ToString(), true);
        }

        private void ToggleWantsAssetsDir_Click(object sender, RoutedEventArgs e) { m_settings.WantsAssetsDir = !m_settings.WantsAssetsDir; }
        private void ToggleWantsAssetFilesConfig_Click(object sender, RoutedEventArgs e) { m_settings.WantsAssetFilesConfig = !m_settings.WantsAssetFilesConfig; }
        private void ToggleWantsOBB_Click(object sender, RoutedEventArgs e) { m_settings.WantsOBB = !m_settings.WantsOBB; }
        private void ToggleWantsReleaseNotes_Click(object sender, RoutedEventArgs e) { m_settings.WantsReleaseNotes = !m_settings.WantsReleaseNotes; }
        private void ToggleWantsLanguagePacks_Click(object sender, RoutedEventArgs e) { m_settings.WantsLanguagePacks = !m_settings.WantsLanguagePacks; }
        private void ToggleWantsDebugSymbols_Click(object sender, RoutedEventArgs e) { m_settings.WantsDebugSymbols = !m_settings.WantsDebugSymbols; }
        private void ToggleWantsDebugSymbolsPattern_Click(object sender, RoutedEventArgs e) { m_settings.WantsDebugSymbolsPattern = !m_settings.WantsDebugSymbolsPattern; }

        bool IsValidOculusExe(string pathToOculusExe)
        {
            string versionStr = "";
            try
            {
                versionStr = CmdHelper.GetCmdOutput(m_settings.PathToOculusExe + " version");
            }
            catch
            {
                //do nothing: we'll still fail out for not having a valid version string
            }

            //okay, we don't even check the version number, we just check that the output is valid-ish
            return versionStr.Contains("Oculus Platform Command Line Utility");
        }
	}
}
