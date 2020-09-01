using Soccen.Models;
using System;
using System.Collections.Generic;
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

        public ServiceExecutionPage()
        {
            InitializeComponent();
            serviceexecutionViewSource = ((CollectionViewSource)(FindResource("serviceexecutionViewSource")));
            servicesViewSource = ((CollectionViewSource)(FindResource("servicesViewSource")));
            DataContext = this;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            context.serviceexecutions.Load();
            context.services.Load();
            serviceexecutionViewSource.Source = context.serviceexecutions.Local;
            servicesViewSource.Source = context.services.Local;
 
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

        private void FilteringSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            serviceexecutionViewSource.Filter += new FilterEventHandler(ServiceExecution_ServiceFilter);
            if(periodDatePicker.SelectedDate != null) serviceexecutionViewSource.Filter += new FilterEventHandler(ServiceExecution_DateFilter);
            serviceexecutionViewSource.View.Refresh();
        }

        void ServiceExecution_ServiceFilter(object sender, FilterEventArgs e)
        {
            serviceexecution serex = e.Item as serviceexecution;
            if (serex != null)
            {
                if (serex.service == serviceComboBox.SelectedItem)
                {
                    e.Accepted = true;
                }
                else
                {
                    e.Accepted = false;
                }
            }
        }

        void ServiceExecution_DateFilter(object sender, FilterEventArgs e)
        {
            serviceexecution serex = e.Item as serviceexecution;
            if (serex != null)
            {
                if ((DateTime.Parse(serex.Date.ToString()).Year == DateTime.Parse(periodDatePicker.SelectedDate.ToString()).Year)&&(DateTime.Parse(serex.Date.ToString()).Month == DateTime.Parse(periodDatePicker.SelectedDate.ToString()).Month))
                {
                    e.Accepted = true;
                }
                else
                {
                    e.Accepted = false;
                }
            }
        }


        //private void dgsalesinvoice_layoutupdated(object sender, eventargs e)
        //{
        //    thickness t = lbltotal.margin;
        //    t.left = (serviceexecutiongrid.columns[0].actualwidth + 7);
        //    lbltotal.margin = t;
        //    lbltotal.width = serviceexecutiongrid.columns[1].actualwidth;

        //    lbltotalsalesinvoiceamount.width = serviceexecutiongrid.columns[2].actualwidth;
        //}
    }
}
