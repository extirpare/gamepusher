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
            SetServicePage(new OculusServicePage());
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SetServicePage(null);
        }

        private void SelectServiceButton_Oculus_Click(object sender, RoutedEventArgs e)
        {
            SetServicePage(new OculusServicePage());
        }

        private void SelectServiceButton_Steam_Click(object sender, RoutedEventArgs e)
        {
            SetServicePage(new SteamServicePage());
        }

        void SetServicePage(IServicePage newPage)
		{
            if (newPage == m_currPage) return;
            if (m_currPage != null && newPage != null && newPage.GetType() == m_currPage.GetType()) return;

            if (m_currPage != null)
                m_currPage.OnExited();

            m_currPage = newPage;
            ServiceFrame.Navigate(m_currPage);
            m_currPage?.OnEntered();
        }
    }
}
