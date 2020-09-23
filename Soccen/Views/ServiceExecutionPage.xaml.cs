using Soccen.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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

namespace Soccen.Views
{
    /// <summary>
    /// Interaction logic for ServiceExecutionPage.xaml
    /// </summary>
    public partial class ServiceExecutionPage : Page
    {
        soccenEntities context = new soccenEntities();
        CollectionViewSource serviceexecutionViewSource;
        CollectionViewSource servicesViewSource;
        int serviceExecutionCount;
        int serviceExecutionAcceptedCount;

        public ServiceExecutionPage()
        {
            InitializeComponent();
            serviceexecutionViewSource = ((CollectionViewSource)(FindResource("serviceexecutionViewSource")));
            servicesViewSource = ((CollectionViewSource)(FindResource("servicesViewSource")));
            DataContext = this;
            
            serviceexecutionViewSource.Filter += ServiceExecution_Filter;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            context.serviceexecutions.Load();
            context.services.Load();
            serviceexecutionViewSource.Source = context.serviceexecutions.Local;
            servicesViewSource.Source = context.services.Local;
            SetSummary();
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            context.SaveChanges();
        }

        private void LastCommandHandler(object sender, RoutedEventArgs e)
        {
            serviceexecutionViewSource.View.MoveCurrentToLast();
        }

        private void PreviousCommandHandler(object sender, RoutedEventArgs e)
        {
            serviceexecutionViewSource.View.MoveCurrentToPrevious();
        }

        private void NextCommandHandler(object sender, RoutedEventArgs e)
        {
            serviceexecutionViewSource.View.MoveCurrentToNext();
        }

        private void FirstCommandHandler(object sender, RoutedEventArgs e)
        {
            serviceexecutionViewSource.View.MoveCurrentToFirst();
        }

        private void SaveComandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            context.SaveChanges();
        }

        private void ServiceComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (serviceexecutionViewSource != null)
            {
                serviceexecutionViewSource.View.Refresh();
                SetSummary();
            }
            
        }

        private void FilteringSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (serviceexecutionViewSource != null)
            {
                serviceexecutionViewSource.View.Refresh();
                SetSummary();
            }

        }

        void ServiceExecution_Filter(object sender, FilterEventArgs e)
        {
            serviceexecution serex = e.Item as serviceexecution;
            if (serex != null)
            {
                if((periodDatePicker.SelectedDate.ToString() != null) && (periodDatePicker.SelectedDate.ToString() != "") && (serviceComboBox.SelectedIndex != 0))
                {
                    if (((periodDatePicker.SelectedDate.ToString()) == null) && (periodDatePicker.SelectedDate.ToString() == "")) e.Accepted = serex.service == serviceComboBox.SelectedItem;
                    else if (serviceComboBox.SelectedIndex == 0) e.Accepted = (DateTime.Parse(serex.Date.ToString()).Year == DateTime.Parse(periodDatePicker.SelectedDate.ToString()).Year) && (DateTime.Parse(serex.Date.ToString()).Month == DateTime.Parse(periodDatePicker.SelectedDate.ToString()).Month);
                        else e.Accepted = (serex.service == serviceComboBox.SelectedItem) && ((DateTime.Parse(serex.Date.ToString()).Year == DateTime.Parse(periodDatePicker.SelectedDate.ToString()).Year) && (DateTime.Parse(serex.Date.ToString()).Month == DateTime.Parse(periodDatePicker.SelectedDate.ToString()).Month));
                }
                else e.Accepted = true;
            }
        }

        private void serviceExecutionGrid_LayoutUpdated(object sender, EventArgs e)
        {
        //    Thickness t = lblTotal.Margin;
        //    t.Left = (serviceExecutionGrid.Columns[0].ActualWidth + 7);
        //    lblTotal.Margin = t;
        //    lblTotal.Width = serviceExecutionGrid.Columns[1].ActualWidth;
              //lblTotalExecutionServicesAmount.Text = serviceExecutionCount.ToString();
              //lblTotalExecutedServisesAmount.Text = serviceExecutionAcceptedCount.ToString();
              //lblTotalNoExecutedServisesAmount.Text = (serviceExecutionCount - serviceExecutionAcceptedCount).ToString();
            //    lblTotalServiceExecutionAmount.Width = serviceExecutionGrid.Columns[2].ActualWidth;
        }

        private void SetSummary()
        {
            serviceExecutionCount = 0;
            serviceExecutionAcceptedCount = 0;
            foreach (serviceexecution item in serviceexecutionViewSource.View)
            {
                serviceExecutionCount++;
                if (item.Status == 1) serviceExecutionAcceptedCount++;
            }
        }
    }
}
