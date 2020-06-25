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
    /// Логика взаимодействия для SocialTypesPage.xaml
    /// </summary>
    public partial class SocialTypesPage : Page
    {
        soccenEntities context = new soccenEntities();
        CollectionViewSource socialTypeViewSource;
        CollectionViewSource serviceViewSource;
        CollectionViewSource socialtypeservicesocialtypesViewSource;

        public SocialTypesPage()
        {
            InitializeComponent();
            socialTypeViewSource = ((CollectionViewSource)(FindResource("socialTypeViewSource")));
            serviceViewSource = ((CollectionViewSource)(FindResource("serviceViewSource")));
            socialtypeservicesocialtypesViewSource = ((CollectionViewSource)(FindResource("socialtypeservicesocialtypesViewSource")));
            DataContext = this;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            context.socialtypes.Load();
            context.services.Load();
            socialTypeViewSource.Source = context.socialtypes.Local;
            serviceViewSource.Source = context.services.Local;
        }

        private void LastCommandHandler(object sender, RoutedEventArgs e)
        {
            socialTypeViewSource.View.MoveCurrentToLast();
        }

        private void PreviousCommandHandler(object sender, RoutedEventArgs e)
        {
            socialTypeViewSource.View.MoveCurrentToPrevious();
        }

        private void NextCommandHandler(object sender, RoutedEventArgs e)
        {
            socialTypeViewSource.View.MoveCurrentToNext();
        }

        private void FirstCommandHandler(object sender, RoutedEventArgs e)
        {
            socialTypeViewSource.View.MoveCurrentToFirst();
        }

        private void DeleteCommandHandler(object sender, RoutedEventArgs e)
        {

            if (MessageBox.Show("Ви точно хочете видалити запис?", "Підтвердження",
            MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    var cur = socialTypeViewSource.View.CurrentItem as socialtype;

                    var soc = (from sc in context.socialtypes
                               where sc.IdSocialType == cur.IdSocialType
                               select sc).FirstOrDefault();

                    if (soc != null)
                    {
                        context.socialtypes.Remove(soc);
                        foreach (servicesocialtype var in soc.servicesocialtypes)
                        {
                            context.servicesocialtypes.Remove(var);
                        }
                        foreach (customersocialtype var in soc.customersocialtypes)
                        {
                            context.customersocialtypes.Remove(var);
                        }
                    }

                    MessageBox.Show("Видалення відбулося успішно!");
                    context.SaveChanges();
                    socialTypeViewSource.View.Refresh();
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
                if (newSocialTypeGrid.IsVisible)
                {

                    socialtype newSocialTypes = new socialtype
                    {
                        Title = add_titleTextBox.Text.Trim(),
                        Description = add_descriptionTextBox.Text.Trim()
                    };
                    context.socialtypes.Local.Insert(0, newSocialTypes);
                    socialTypeViewSource.View.Refresh();
                    socialTypeViewSource.View.MoveCurrentTo(newSocialTypes);
                    newSocialTypeGrid.Visibility = Visibility.Collapsed; 
                    SocialTypeServiceGrid.Visibility = Visibility.Visible;
                    existingSocialTypeGrid.Visibility = Visibility.Visible;

                }

                context.SaveChanges();
                socialTypeViewSource.View.Refresh();
            }
        }


        private void AddCommandHandler(object sender, RoutedEventArgs e)
        {
            newSocialTypeGrid.Visibility = Visibility.Visible;
            SocialTypeServiceGrid.Visibility = Visibility.Collapsed;
            existingSocialTypeGrid.Visibility = Visibility.Collapsed;

            add_titleTextBox.Text = "";
            add_descriptionTextBox.Text = "";
        }

        private void CancelCommandHandler(object sender, RoutedEventArgs e)
        {
            add_titleTextBox.Text = "";
            add_descriptionTextBox.Text = "";
        }

        private void socialTypeDataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            existingSocialTypeGrid.Visibility = Visibility.Visible;
            newSocialTypeGrid.Visibility = Visibility.Collapsed;
            SocialTypeServiceGrid.Visibility = Visibility.Visible;
        }

        private void SelectServiceCommandHandler(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            try
            {
                if ((ServiceList.SelectedItems.Count > 0))
                {

                    var cur = socialTypeViewSource.View.CurrentItem as socialtype;
                    service transservice = (service)ServiceList.SelectedItems[0];

                    foreach (servicesocialtype var in cur.servicesocialtypes)
                    {
                        if ((var.SocialTypeId == cur.IdSocialType) && (var.ServiceIdSocialtype == transservice.IdService))
                        {
                            MessageBox.Show("Йой, та такий соціальний тип уже є!", "Помилка");
                            return;
                        }
                    }



                    cur.servicesocialtypes.Add(new servicesocialtype()
                    {
                        socialtype = cur,
                        service = transservice
                    });

                    context.SaveChanges();
                    socialtypeservicesocialtypesViewSource.View.Refresh();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Щойно відбувся оброблений виняток: " + ex.Message, "Помилка", MessageBoxButton.OK);
            }
        }
        private void DeleteServiceCommandHandler(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {

            try
            {
                servicesocialtype selectedItem = (servicesocialtype)SocialTypesServiceList.SelectedItems[0];

                if (selectedItem != null)
                {
                    context.servicesocialtypes.Remove(selectedItem);
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
}
