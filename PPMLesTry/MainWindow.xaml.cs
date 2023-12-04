using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PPMLesTry
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Frame> allPages = new List<Frame>();
        private int currentPage = 0;

        public MainWindow()
        {
            InitializeComponent();
            allPages.AddRange(new List<Frame>
            {
                MainContent, EncodeContent, DecodeContent
            });
        }

        private void EnterPage(object sender, RoutedEventArgs e)
        {
            MenuItem s = (MenuItem)sender;

            int switchedPage = 0;
            switch(s.Header)
            {
                case "Main Page":
                    switchedPage = 0;
                    break;
                case "Encode":
                    switchedPage = 1;
                    break;
                case "Decode":
                    switchedPage = 2;
                    break;
            }
            if (switchedPage == currentPage) return;

            allPages[currentPage].Visibility = Visibility.Collapsed;
            allPages[switchedPage].Visibility = Visibility.Visible;
            currentPage = switchedPage;
        }
    }
}