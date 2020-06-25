using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Data.Entity;
using System.Linq;
using Soccen.Models;
using System;

namespace Soccen.Views
{
    /// <summary>
    /// Логика взаимодействия для ServisesPage.xaml
    /// </summary>
    public partial class ServisesPage : Page
    {
        soccenEntities context = new soccenEntities();
        CollectionViewSource serviceViewSource;
        public ServisesPage()
        {
            InitializeComponent();
            serviceViewSource = ((CollectionViewSource)(FindResource("serviceViewSource")));
            DataContext = this;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            context.services.Load();
            serviceViewSource.Source = context.services.Local;
        }

        private void LastCommandHandler(object sender, RoutedEventArgs e)
        {
            serviceViewSource.View.MoveCurrentToLast();
        }

        private void PreviousCommandHandler(object sender, RoutedEventArgs e)
        {
            serviceViewSource.View.MoveCurrentToPrevious();
        }

        private void NextCommandHandler(object sender, RoutedEventArgs e)
        {
            serviceViewSource.View.MoveCurrentToNext();
        }

        private void FirstCommandHandler(object sender, RoutedEventArgs e)
        {
            serviceViewSource.View.MoveCurrentToFirst();
        }

        private void DeleteCommandHandler(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Ви точно хочете видалити запис?", "Підтвердження",
    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    var cur = serviceViewSource.View.CurrentItem as service;

                    var ser = (from sr in context.services
                               where sr.IdService == cur.IdService
                               select sr).FirstOrDefault();

                    if (ser != null)
                    {
                        context.services.Remove(ser);
                        foreach (servicesocialtype var in ser.servicesocialtypes)
                        {
                            context.servicesocialtypes.Remove(var);
                        }
                        foreach (serviceexecution var in ser.serviceexecutions)
                        {
                            context.serviceexecutions.Remove(var);
                        }
                    }

                    context.SaveChanges();
                    serviceViewSource.View.Refresh();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Щойно відбувся оброблений виняток: " + ex.Message, "Помилка", MessageBoxButton.OK);
                }
            }
            

        }


        private void UpdateCommandHandler(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Ви точно хочете зберегти запис?", "Підтвердження",
    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                if (newServiceGrid.IsVisible)
                {

                    service newService = new service
                    {
                        Title = add_titleTextBox.Text.Trim(),
                        Description = add_descriptionTextBox.Text.Trim()

                    };
                    if(add_moneyHelpTextBox.Text != "")
                    {
                        try
                        {
                            newService.MoneyHelp = Int32.Parse((string)add_moneyHelpTextBox.Text.Trim());
                        }
                        catch
                        {
                            MessageBox.Show("Грошова допомога має складатися з цифр!");
                            return;
                        }
                    }
                    
                    context.services.Local.Insert(0, newService);
                    serviceViewSource.View.Refresh();
                    serviceViewSource.View.MoveCurrentTo(newService);
                    newServiceGrid.Visibility = Visibility.Collapsed;
                    existingServiceGrid.Visibility = Visibility.Visible;

                }

                context.SaveChanges();
                serviceViewSource.View.Refresh();
            }
        }


        private void AddCommandHandler(object sender, RoutedEventArgs e)
        {
            newServiceGrid.Visibility = Visibility.Visible;
            existingServiceGrid.Visibility = Visibility.Collapsed;

            add_titleTextBox.Text = "";
            add_descriptionTextBox.Text = "";
            add_moneyHelpTextBox.Text = "";
        }

        private void CancelCommandHandler(object sender, RoutedEventArgs e)
        {
            add_titleTextBox.Text = "";
            add_descriptionTextBox.Text = "";
            add_moneyHelpTextBox.Text = "";
        }

        private void serviceDataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            existingServiceGrid.Visibility = Visibility.Visible;
            newServiceGrid.Visibility = Visibility.Collapsed;
        }
    }
}
