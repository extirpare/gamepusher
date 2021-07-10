using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using Path = System.IO.Path;

namespace GameUploader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        OculusUploaderGUISettings m_settings;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            m_settings.Save(OculusUploaderGUISettings.DefaultPath);
        }


        private void SelectServiceButton_Oculus_Click(object sender, RoutedEventArgs e)
        {
            ServiceFrame.Navigate(new OculusServicePage());
        }


        private void SelectServiceButton_Steam_Click(object sender, RoutedEventArgs e)
        {
            ServiceFrame.Navigate(new SteamServicePage());
        }

        private void UploadBuildButton_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
