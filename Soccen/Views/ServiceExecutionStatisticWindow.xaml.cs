using System.Windows;
using Soccen.Models;
using System;
using System.Linq;

namespace Soccen.Views
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class ServiceExecutionStatisticWindow : Window
    {
        public ServiceExecutionStatisticWindow(System.Windows.Data.CollectionViewSource collectionView, string serviceName, string date)
        {
            InitializeComponent();
            this.WindowTitle.Content += $" - {serviceName} - {date}";
            getServiceExecutionsStatistic(collectionView);
        }

        // ServiceExecuted statistic function
        private void getServiceExecutionsStatistic(System.Windows.Data.CollectionViewSource serviceExecutionViewSource)
        {
            if (serviceExecutionViewSource.View != null)
            {
                int templateServiceExecution = serviceExecutionViewSource.View.Cast<serviceexecution>().Count();

                LabelTotalExecutionServicesAmount.Content = templateServiceExecution.ToString();

                int templateServiceExected = (from item in serviceExecutionViewSource.View.Cast<serviceexecution>()
                                              where item.Status == 1
                                              select item).Count();

                LabelTotalExecutedServicesAmount.Content = templateServiceExected.ToString();

                LabelTotalNoExecutedServicesAmount.Content = (templateServiceExecution - templateServiceExected).ToString();

                int templatePayedServicesExecution = (from item in serviceExecutionViewSource.View.Cast<serviceexecution>()
                                                      where item.PayOrFree == 1
                                                      select item).Count();

                LabelTotalPayedServicesAmount.Content = templatePayedServicesExecution.ToString();

                LabelTotalFreeServicesAmount.Content = (templateServiceExecution - templatePayedServicesExecution).ToString();



                int templateMaleServiceExecution = (from item in serviceExecutionViewSource.View.Cast<serviceexecution>()
                                                    where item.customer.Gender == "Чоловік"
                                                    select item).Count();

                LabelTotalMaleExecutionServicesAmount.Content = templateMaleServiceExecution.ToString();

                int templateMaleServiceExecuted = (from item in serviceExecutionViewSource.View.Cast<serviceexecution>()
                                                   where item.customer.Gender == "Чоловік" && item.Status == 1
                                                   select item).Count();

                LabelTotalMaleExecutedServicesAmount.Content = templateMaleServiceExecuted.ToString();

                LabelTotalMaleNoExecutedServicesAmount.Content = (templateMaleServiceExecution - templateMaleServiceExecuted).ToString();

                int templateMalePayedServicesExecution = (from item in serviceExecutionViewSource.View.Cast<serviceexecution>()
                                                          where item.PayOrFree == 1 && item.customer.Gender == "Чоловік"
                                                          select item).Count();

                LabelTotalMalePayedServicesAmount.Content = templateMalePayedServicesExecution.ToString();

                LabelTotalMaleFreeServicesAmount.Content = (templateMaleServiceExecution - templateMalePayedServicesExecution).ToString();



                int templateFemaleServiceExecution = (from item in serviceExecutionViewSource.View.Cast<serviceexecution>()
                                                      where item.customer.Gender == "Жінка"
                                                      select item).Count();

                LabelTotalFemaleExecutionServicesAmount.Content = templateFemaleServiceExecution.ToString();

                int templateFemaleServiceExecuted = (from item in serviceExecutionViewSource.View.Cast<serviceexecution>()
                                                     where item.customer.Gender == "Жінка" && item.Status == 1
                                                     select item).Count();

                LabelTotalFemaleExecutedServicesAmount.Content = templateFemaleServiceExecuted.ToString();

                LabelTotalFemaleNoExecutedServicesAmount.Content = (templateFemaleServiceExecution - templateFemaleServiceExecuted).ToString();

                int templateFemalePayedServicesExecution = (from item in serviceExecutionViewSource.View.Cast<serviceexecution>()
                                                            where item.PayOrFree == 1 && item.customer.Gender == "Жінка"
                                                            select item).Count();

                LabelTotalFemalePayedServicesAmount.Content = templateFemalePayedServicesExecution.ToString();

                LabelTotalFemaleFreeServicesAmount.Content = (templateFemaleServiceExecution - templateFemalePayedServicesExecution).ToString();
            }
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
    }
}
