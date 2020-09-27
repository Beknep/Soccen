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
        CollectionViewSource serviceExecutionViewSource;
        CollectionViewSource servicesViewSource;

        public ServiceExecutionPage()
        {
            InitializeComponent();
            serviceExecutionViewSource = ((CollectionViewSource)(FindResource("serviceexecutionViewSource")));
            servicesViewSource = ((CollectionViewSource)(FindResource("servicesViewSource")));
            DataContext = this;
            
            serviceExecutionViewSource.Filter += ServiceExecution_Filter;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            context.serviceexecutions.Load();
            context.services.Load();
            serviceExecutionViewSource.Source = context.serviceexecutions.Local;
            servicesViewSource.Source = context.services.Local;
            
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
           // Create some exectuion of services

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
                if((periodDatePicker.SelectedDate.ToString() != null) && (periodDatePicker.SelectedDate.ToString() != "") && (serviceComboBox.SelectedIndex != 0))
                {
                    if (((periodDatePicker.SelectedDate.ToString()) == null) && (periodDatePicker.SelectedDate.ToString() == "")) e.Accepted = serex.service == serviceComboBox.SelectedItem;
                    else if (serviceComboBox.SelectedIndex == 0) e.Accepted = (DateTime.Parse(serex.Date.ToString()).Year == DateTime.Parse(periodDatePicker.SelectedDate.ToString()).Year) && (DateTime.Parse(serex.Date.ToString()).Month == DateTime.Parse(periodDatePicker.SelectedDate.ToString()).Month);
                        else e.Accepted = (serex.service == serviceComboBox.SelectedItem) && ((DateTime.Parse(serex.Date.ToString()).Year == DateTime.Parse(periodDatePicker.SelectedDate.ToString()).Year) && (DateTime.Parse(serex.Date.ToString()).Month == DateTime.Parse(periodDatePicker.SelectedDate.ToString()).Month));
                }
                else e.Accepted = true;
            }
        }

        // ServiceExecuted statistic function
        private void serviceExecutionGrid_LayoutUpdated(object sender, EventArgs e)
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

                int templateMaleServiceExecuted= (from item in serviceExecutionViewSource.View.Cast<serviceexecution>()
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
    }
}
