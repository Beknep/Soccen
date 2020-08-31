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

namespace Soccen
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()

        {
            InitializeComponent();
        }

        public void OnMinimizeButtonClick(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void OnMaximizeRestoreButtonClick(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
            }
            else
            {
                this.WindowState = WindowState.Maximized;
            }
        }

        private void OnCloseButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void RefreshMaximizeRestoreButton()
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.maximizeButton.Visibility = Visibility.Collapsed;
                this.restoreButton.Visibility = Visibility.Visible;
            }
            else
            {
                this.maximizeButton.Visibility = Visibility.Visible;
                this.restoreButton.Visibility = Visibility.Collapsed;
            }
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            this.RefreshMaximizeRestoreButton();
        }

        private void StreetsPageButton_Click(object sender, RoutedEventArgs e)
        {
            Uri myUri = new Uri("StreetsPage.xaml", UriKind.Relative);
            MainFrame.Source = myUri;
            
        }

        private void CustomersPageButton_Click(object sender, RoutedEventArgs e)
        {
            Uri myUri = new Uri("CustomersPage.xaml", UriKind.Relative);
            MainFrame.Source = myUri;
        }

        private void ServicesPageButton_Click(object sender, RoutedEventArgs e)
        {
            Uri myUri = new Uri("ServisesPage.xaml", UriKind.Relative);
            MainFrame.Source = myUri;
        }

        private void BanksPageButton_Click(object sender, RoutedEventArgs e)
        {
            Uri myUri = new Uri("BanksPage.xaml", UriKind.Relative);
            MainFrame.Source = myUri;
        }

        private void SocialTypesPageButton_Click(object sender, RoutedEventArgs e)
        {
            Uri myUri = new Uri("SocialTypesPage.xaml", UriKind.Relative);
            MainFrame.Source = myUri;
        }

        private void ServiseExecutionButton_Click(object sender, RoutedEventArgs e)
        {
            Uri myUri = new Uri("ServiceExecutionPage.xaml", UriKind.Relative);
            MainFrame.Source = myUri;
        }
    }
}
