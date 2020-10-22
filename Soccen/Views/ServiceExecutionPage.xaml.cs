using Soccen.Models;
using System;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Soccen.Views
{
    /// <summary>
    /// Interaction logic for ServiceExecutionPage.xaml
    /// </summary>
    public partial class ServiceExecutionPage : Page
    {
        soccenEntities context = new soccenEntities();
        CollectionViewSource customersViewSource;
        CollectionViewSource serviceExecutionViewSource;
        CollectionViewSource servicesViewSource;

        public ServiceExecutionPage()
        {
            InitializeComponent();
            serviceExecutionViewSource = ((CollectionViewSource)(FindResource("serviceexecutionViewSource")));
            servicesViewSource = ((CollectionViewSource)(FindResource("servicesViewSource")));
            customersViewSource = ((CollectionViewSource)(FindResource("customersViewSource")));
            DataContext = this;

            serviceExecutionViewSource.Filter += ServiceExecution_Filter;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            context.serviceexecutions.Load();
            context.services.Load();
            context.customers.Load();
            serviceExecutionViewSource.Source = context.serviceexecutions.Local;
            servicesViewSource.Source = context.services.Local;
            customersViewSource.Source = context.customers.Local;

        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            context.SaveChanges();
        }

        private void LastCommandHandler(object sender, RoutedEventArgs e)
        {
            serviceExecutionViewSource.View.MoveCurrentToLast();
        }

        private void PreviousCommandHandler(object sender, RoutedEventArgs e)
        {
            serviceExecutionViewSource.View.MoveCurrentToPrevious();
        }

        private void NextCommandHandler(object sender, RoutedEventArgs e)
        {
            serviceExecutionViewSource.View.MoveCurrentToNext();
        }

        private void FirstCommandHandler(object sender, RoutedEventArgs e)
        {
            serviceExecutionViewSource.View.MoveCurrentToFirst();
        }

        private void SaveComandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            context.SaveChanges();
        }

        private void CreateServiceExecutionsCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            CreateServiceExecutionsWindow window = new CreateServiceExecutionsWindow();
            window.ShowDialog();
        }

        private void OpenServiceExecutionStatisticWindowButtonCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            if ((serviceExecutionViewSource != null) && (periodDatePicker.SelectedDate.ToString() != null) && (periodDatePicker.SelectedDate.ToString() != ""))
            {
                string tempComboBoxText;
                if (serviceComboBox.SelectedIndex == 0) 
                    tempComboBoxText = serviceComboBox.Text; 
                else
                    tempComboBoxText = ((service)serviceComboBox.SelectedItem).Title; 

                ServiceExecutionStatisticWindow window = new ServiceExecutionStatisticWindow(serviceExecutionViewSource, tempComboBoxText, ((DateTime)periodDatePicker.SelectedDate).ToString("MMMM yyyy", CultureInfo.CreateSpecificCulture("uk-UA")));
                window.ShowDialog();
            }
        }

        private void ServiceComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (serviceExecutionViewSource != null)
            {
                serviceExecutionViewSource.View.Refresh();

            }

        }

        private void FilteringSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (serviceExecutionViewSource != null)
            {
                serviceExecutionViewSource.View.Refresh();

            }

        }

        // Filter for 
        void ServiceExecution_Filter(object sender, FilterEventArgs e)
        {
            serviceexecution serex = e.Item as serviceexecution;
            if (serex != null)
            {
                if ((periodDatePicker.SelectedDate.ToString() != null) && (periodDatePicker.SelectedDate.ToString() != "") && (serviceComboBox.SelectedIndex != 0))
                {
                    if (((periodDatePicker.SelectedDate.ToString()) == null) && (periodDatePicker.SelectedDate.ToString() == "")) e.Accepted = serex.service == serviceComboBox.SelectedItem;
                    else if (serviceComboBox.SelectedIndex == 0) e.Accepted = (DateTime.Parse(serex.Date.ToString()).Year == DateTime.Parse(periodDatePicker.SelectedDate.ToString()).Year) && (DateTime.Parse(serex.Date.ToString()).Month == DateTime.Parse(periodDatePicker.SelectedDate.ToString()).Month);
                    else e.Accepted = (serex.service == serviceComboBox.SelectedItem) && ((DateTime.Parse(serex.Date.ToString()).Year == DateTime.Parse(periodDatePicker.SelectedDate.ToString()).Year) && (DateTime.Parse(serex.Date.ToString()).Month == DateTime.Parse(periodDatePicker.SelectedDate.ToString()).Month));
                }
                else e.Accepted = true;
            }
        }
    }
}
