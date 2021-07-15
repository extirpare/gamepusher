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
        IServicePage m_currPage;

        public MainWindow()
        {
            InitializeComponent();
            
            DataContext = MetaSettings.Instance;

			switch (MetaSettings.Instance.CurrPage)
			{
                case "Steam": SetServicePage(new SteamPage()); break;
                case "Itch": SetServicePage(new ItchPage()); break;
                case "Oculus": SetServicePage(new OculusPage()); break;
                case "Settings": default: SetServicePage(new SettingsPage()); break;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MetaSettings.Instance.Save();
            SetServicePage(null);
        }

        private void SelectServiceButton_Oculus_Click(object sender, RoutedEventArgs e)
        {
            SetServicePage(new OculusPage());
        }

        private void SelectServiceButton_Steam_Click(object sender, RoutedEventArgs e)
        {
            SetServicePage(new SteamPage());
        }

        private void SelectServiceButton_Itch_Click(object sender, RoutedEventArgs e)
        {
            SetServicePage(new ItchPage());
        }

        private void SelectServiceButton_Settings_Click(object sender, RoutedEventArgs e)
        {
            SetServicePage(new SettingsPage());
        }

        void SetServicePage(IServicePage newPage)
		{
            if (newPage == m_currPage) return;
            if (m_currPage != null && newPage != null && newPage.GetType() == m_currPage.GetType()) return;

            if (m_currPage != null)
                m_currPage.OnExited();

            m_currPage = newPage;
            ServiceFrame.Navigate(m_currPage);

            if (m_currPage != null)
			{
                m_currPage.OnEntered();
                MetaSettings.Instance.CurrPage = m_currPage.ServiceName;
            }
        }
    }
}
