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
    /// Логика взаимодействия для StreetPage.xaml
    /// </summary>
    public partial class StreetPage : Page
    {

        soccenEntities context = new soccenEntities();
        CollectionViewSource streetsViewSource;
        public StreetPage()
        {
            InitializeComponent();
            streetsViewSource = ((CollectionViewSource)(FindResource("streetsViewSource")));
            DataContext = this;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            context.streets.Load();
            streetsViewSource.Source = context.streets.Local;
        }

        private void LastCommandHandler(object sender, RoutedEventArgs e)
        {
            streetsViewSource.View.MoveCurrentToLast();
        }

        private void PreviousCommandHandler(object sender, RoutedEventArgs e)
        {
            streetsViewSource.View.MoveCurrentToPrevious();
        }

        private void NextCommandHandler(object sender, RoutedEventArgs e)
        {
            streetsViewSource.View.MoveCurrentToNext();
        }

        private void FirstCommandHandler(object sender, RoutedEventArgs e)
        {
            streetsViewSource.View.MoveCurrentToFirst();
        }

        private void DeleteStreetCommandHandler(object sender, RoutedEventArgs e)
        {
            try
            {
                var cur = streetsViewSource.View.CurrentItem as street;

                var str = (from s in context.streets
                            where s.IdStreet == cur.IdStreet
                            select s).FirstOrDefault();

                if (str != null)
                {
                    context.streets.Remove(str);
                }

                MessageBox.Show("Видалення відбулося успішно!");
                context.SaveChanges();
                streetsViewSource.View.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Щойно відбувся оброблений виняток: " + ex.Message, "Помилка", MessageBoxButton.OK);
            }

        }

        
        private void UpdateCommandHandler(object sender, RoutedEventArgs e)
        {
            if (newStreetGrid.IsVisible)
            {
                
                street newStreet = new street
                {
                    Title = add_titleTextBox.Text,
                };
                context.streets.Local.Insert( 0, newStreet);
                streetsViewSource.View.Refresh();
                streetsViewSource.View.MoveCurrentTo(newStreet);
                newStreetGrid.Visibility = Visibility.Collapsed;
                existingStreetGrid.Visibility = Visibility.Visible;

            }

            context.SaveChanges();
            streetsViewSource.View.Refresh();
        }

        
        private void AddCommandHandler(object sender, RoutedEventArgs e)
        {
            add_titleTextBox.Text = "";
            newStreetGrid.Visibility = Visibility.Visible;
            existingStreetGrid.Visibility = Visibility.Collapsed;
        }

        private void CancelCommandHandler(object sender, RoutedEventArgs e)
        {
            add_titleTextBox.Text = "";

        }

        private void streetDataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            existingStreetGrid.Visibility = Visibility.Visible;
            newStreetGrid.Visibility = Visibility.Collapsed;
        }
    }
}
